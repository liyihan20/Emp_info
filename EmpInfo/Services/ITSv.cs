﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmpInfo.Models;
using EmpInfo.FlowSvr;
using Newtonsoft.Json;
using EmpInfo.Util;
using EmpInfo.Interfaces;

namespace EmpInfo.Services
{
    public class ITSv:BillSv,IBeginAuditOtherInfo
    {
        ei_itApply bill;

        public ITSv() { }
        public ITSv(string sysNo)
        {
            bill = db.ei_itApply.Single(s => s.sys_no == sysNo);
        }
        public override string BillType
        {
            get { return "IT"; }
        }

        public override string BillTypeName
        {
            get { return "电脑故障报修申请"; }
        }

        public override string AuditViewName()
        {
            return "BeginAuditITApply";
        }

        public override List<ApplyMenuItemModel> GetApplyMenuItems(UserInfo userInfo)
        {
            var menus = base.GetApplyMenuItems(userInfo);
            //if (HasGotPower("APReport", userInfo.id)) {
            //    menus.Add(new ApplyMenuItemModel()
            //    {
            //        text = "查询报表",
            //        iconFont = "fa-file-text-o",
            //        url = "../Report/APReport"
            //    });
            //}

            if (db.ei_flowAuthority.Where(f => f.bill_type == BillType && f.relate_type == "维修项目" && f.relate_value == userInfo.cardNo).Count() > 0) {
                menus.Add(new ApplyMenuItemModel()
                {
                    text = "维修项目管理",
                    iconFont = "fa-list",
                    url = "../ApplyExtra/ManageITItems"
                });
            }
            return menus;
        }

        public override object GetInfoBeforeApply(Models.UserInfo userInfo, Models.UserInfoDetail userInfoDetail)
        {
            var hr = new HRDBSv().GetHREmpDetailInfo(userInfo.cardNo);
            var info = new
            {
                sys_no = GetNextSysNum(BillType),
                applier_name = userInfo.name,
                dep_name = hr.dep_name,
                emp_position = new HRDBSv().GetHREmpInfo(userInfo.cardNo).job_level,
                applier_phone = string.IsNullOrWhiteSpace(userInfoDetail.shortPhone) ? userInfoDetail.phone : userInfoDetail.shortPhone,
            };
            return JsonConvert.SerializeObject(info);
        }

        public override void SaveApply(System.Web.Mvc.FormCollection fc, Models.UserInfo userInfo)
        {
            bill = JsonConvert.DeserializeObject<ei_itApply>(fc.Get("obj"));
            bill.apply_time = DateTime.Now;
            bill.applier_num = userInfo.cardNo;
            bill.dep_charger_no = GetUserCardByNameAndCardNum(bill.dep_charger_name);
            bill.dep_charger_name = GetUserNameByNameAndCardNum(bill.dep_charger_name);
            bill.emp_position = bill.emp_position ?? "";
            bill.fixed_items = "[]";

            if (bill.dep_name.Contains("总裁办")) {
                bill.priority = 5;
            }
            else if (bill.emp_position.Contains("部长")) {
                bill.priority = 4;
            }
            else if (bill.emp_position.Contains("经理")) {
                bill.priority = 3;
            }
            else {
                bill.priority = 1;
            }

            FlowSvrSoapClient client = new FlowSvrSoapClient();
            var result = client.StartWorkFlow(JsonConvert.SerializeObject(bill), BillType, userInfo.cardNo, bill.sys_no, bill.priority.ToString(), bill.dep_name);
            if (result.suc) {
                try {
                    db.ei_itApply.Add(bill);

                    db.SaveChanges();
                }
                catch (Exception ex) {
                    //将生成的流程表记录删除
                    client.DeleteApplyForFailure(bill.sys_no);
                    throw new Exception("申请提交失败，原因：" + ex.Message);
                }

                SendNotification(result);
            }
            else {
                throw new Exception("申请提交失败，原因是：" + result.msg);
            }

        }

        public override object GetBill()
        {
            return bill;
        }

        public override Models.SimpleResultModel HandleApply(System.Web.Mvc.FormCollection fc, Models.UserInfo userInfo)
        {
            int step = Int32.Parse(fc.Get("step"));
            string stepName = fc.Get("stepName");
            bool isPass = bool.Parse(fc.Get("isPass"));
            string opinion = fc.Get("opinion");

            if (stepName.Equals("IT部维修处理")) {
                MyUtils.SetFieldValueToModel(fc, bill);
                bill.repair_man = userInfo.name;
                bill.repair_time = DateTime.Now;
            }
            else if (stepName.Equals("服务评价")) {
                MyUtils.SetFieldValueToModel(fc, bill);
                bill.evaluation_time = DateTime.Now;
            }

            FlowSvrSoapClient flow = new FlowSvrSoapClient();
            var result = flow.BeginAudit(bill.sys_no, step, userInfo.cardNo, isPass, opinion, JsonConvert.SerializeObject(bill));
            if (result.suc) {
                db.SaveChanges();
                //发送通知到下一级审核人
                SendNotification(result);
            }
            return new SimpleResultModel() { suc = result.suc, msg = result.msg };
        }

        public override void SendNotification(FlowSvr.FlowResultModel model)
        {
            if (model.suc) {
                if (model.msg.Contains("完成") || model.msg.Contains("NG")) {
                    bool isSuc = model.msg.Contains("NG") ? false : true;
                    List<vw_push_users> pushUsers = db.vw_push_users.Where(v => v.card_number == bill.applier_num).ToList();
                                        

                    SendEmailForCompleted(
                        bill.sys_no,
                        BillTypeName + "已" + (isSuc ? "批准" : "被拒绝"),
                        bill.applier_name,
                        string.Format("你申请的单号为【{0}】的{1}已{2}，请知悉。", bill.sys_no, BillTypeName, (isSuc ? "维修完成" : "被拒绝")),
                        GetUserEmailByCardNum(bill.applier_num)
                        );

                    SendWxMessageForCompleted(
                        BillTypeName,
                        bill.sys_no,
                        (isSuc ? "维修完成" : "被拒绝"),
                        pushUsers
                        );
                }
                else {
                    FlowSvrSoapClient flow = new FlowSvrSoapClient();
                    var result = flow.GetCurrentStep(bill.sys_no);

                    SendEmailToNextAuditor(
                        bill.sys_no,
                        result.step,
                        string.Format("你有一张待审批的{0}", BillTypeName),
                        GetUserNameByCardNum(model.nextAuditors),
                        string.Format("你有一张待处理的单号为【{0}】的{1}，请尽快登陆系统处理。", bill.sys_no, BillTypeName),
                        GetUserEmailByCardNum(model.nextAuditors)
                        );

                    string[] nextAuditors = model.nextAuditors.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    SendWxMessageToNextAuditor(
                        BillTypeName,
                        bill.sys_no,
                        result.step,
                        result.stepName,
                        bill.applier_name,
                        ((DateTime)bill.apply_time).ToString("yyyy-MM-dd HH:mm"),
                        string.Format("所在部门：{0};计算机名/IP地址：{1}/{2}", bill.dep_name, bill.computer_name ?? "", bill.ip_addr ?? ""),
                        db.vw_push_users.Where(p => nextAuditors.Contains(p.card_number)).ToList()
                        );
                }
            }
        }

        public override IComparer<FlowAuditListModel> GetAuditListComparer()
        {
            return new BillComparer();
        }

        private class BillComparer : IComparer<FlowAuditListModel>
        {
            public int Compare(FlowAuditListModel x, FlowAuditListModel y)
            {
                if (x == null && y == null) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                //这里的title字段要保存优先级别，1~5，数字大的优先级大，排在前面
                if(!x.title.Equals(y.title)){
                    return -string.Compare(x.title,y.title);
                }
                else {
                    if (x.applyTime == null) return 1;
                    if (y.applyTime == null) return -1;
                    return x.applyTime > y.applyTime ? 1 : -1;
                }
            }
        }

        #region IT维修项目管理

        public List<ei_itItems> GetITItems()
        {
            return db.ei_itItems.OrderBy(i => i.item_type).ThenBy(i => i.item_name).ToList();
        }

        public void SaveItItem(ei_itItems item)
        {
            if (item.id == 0) {
                db.ei_itItems.Add(item);
            }
            else {
                var _item = db.ei_itItems.Single(i => i.id == item.id);
                MyUtils.CopyPropertyValue(item, _item);
            }
            db.SaveChanges();
        }

        public void RemoveItItem(int id)
        {
            db.ei_itItems.Remove(db.ei_itItems.SingleOrDefault(i => i.id == id));
            db.SaveChanges();
        }

        #endregion

        public object GetBeginAuditOtherInfo(string sysNo, int step)
        {
            return db.ei_itApply.Single(i => i.sys_no == sysNo).faulty_items;
        }
    }
}