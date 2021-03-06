﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmpInfo.Models;
using EmpInfo.Interfaces;
using EmpInfo.EmpWebSvr;
using EmpInfo.Util;
using EmpInfo.FlowSvr;
using Newtonsoft.Json;

namespace EmpInfo.Services
{
    public class SJSv:BillSv,IBeginAuditOtherInfo
    {
        ei_sjApply bill;

        public SJSv() { }
        public SJSv(string sysNo)
        {
            bill = db.ei_sjApply.Single(j => j.sys_no == sysNo);
        }
        public override string BillType
        {
            get { return "SJ"; }
        }

        public override string BillTypeName
        {
            get { return "员工调动流程"; }
        }

        public override List<ApplyNavigatorModel> GetApplyNavigatorLinks()
        {
            var list = base.GetApplyNavigatorLinks();
            list.Add(
                new ApplyNavigatorModel()
                {
                    text = "无纸化流程",
                    url = "Home/NoPaperProcess"
                }
            );
            return list;
        }
        
        public override List<ApplyMenuItemModel> GetApplyMenuItems(UserInfo userInfo)
        {
            var menus = base.GetApplyMenuItems(userInfo);
            //请假流程的统计员权限就能看离职报表
            if (db.ei_department.Where(d => d.FReporter.Contains(userInfo.cardNo)).Count() > 0) {
                menus.Add(new ApplyMenuItemModel()
                {
                    text = "查询报表",
                    iconFont = "fa-file-text-o",
                    url = "../Report/SJReport"
                });                
            }

            if(MyUtils.hasGotPower(userInfo.id,"Admin","UpdateHREmpDept")){
                menus.Add(new ApplyMenuItemModel()
                {
                    text = "人工调动",
                    iconFont = "fa-refresh",
                    url = "../Admin/UpdateHREmpDept"
                });
            }
            return menus;
        }

        public override string AuditViewName()
        {
            return "BeginAuditSJApply";
        }

        public override object GetInfoBeforeApply(UserInfo userInfo, UserInfoDetail userInfoDetail)
        {
            if (userInfoDetail.depLongName.Contains("ADD") || userInfoDetail.depLongName.Contains("AUTO") || userInfoDetail.depLongName.Contains("信息管理部") || userInfoDetail.depLongName.Contains("TDD")) {
                return GetNextSysNum(BillType);
            }
            throw new Exception("测试阶段只允许ADD和TDD部门的员工申请");
        }

        public override void SaveApply(System.Web.Mvc.FormCollection fc, UserInfo userInfo)
        {            
            bill = JsonConvert.DeserializeObject<ei_sjApply>(fc.Get("head"));
            var entrys = JsonConvert.DeserializeObject<List<ei_sjApplyEntry>>(fc.Get("entry"));

            //公司内、跨公司调动判断部门
            string[] copKeys = new string[] { "信利半导体", "信利光电股份", "信利电子", "信利仪器", "信利工业", "信元光电", "第三方", "光电仁寿" };
            foreach (var e in entrys) {
                
                if (e.out_dep_position != null && e.out_dep_position.Length > 50) throw new Exception(e.name+ ":调出部门岗位内容太多，请删减");
                if (e.in_dep_position != null && e.in_dep_position.Length > 50) throw new Exception(e.name+":调入部门岗位内容太多，请删减");
                foreach (var k in copKeys) {
                    if (bill.switch_type == "公司内") {
                        if (e.out_dep_name.Contains(k) && !e.in_dep_name.Contains(k)) {
                            throw new Exception(e.name + ":调入部门和调出部门不属于同一公司，调动类型必须选择跨公司");
                        }
                    }
                    if (bill.switch_type == "跨公司") {
                        if (e.out_dep_name.Contains(k) && e.in_dep_name.Contains(k)) {
                            throw new Exception(e.name + ":调入部门和调出部门属于同一公司，调动类型必须选择公司内");
                        }
                    }
                }

                e.is_agree = true;
            }

            
            if (string.IsNullOrWhiteSpace(bill.in_clerk_name)) throw new Exception("必须选择调入部门文员");
            if (string.IsNullOrWhiteSpace(bill.out_clerk_name)) throw new Exception("必须选择调出部门文员");
            if (string.IsNullOrWhiteSpace(bill.in_manager_name)) throw new Exception("必须选择调入部门主管/经理");
            if (string.IsNullOrWhiteSpace(bill.out_manager_name)) throw new Exception("必须选择调出部门主管/经理");

            bill.in_clerk_num = GetUserCardByNameAndCardNum(bill.in_clerk_name);
            bill.out_clerk_num = GetUserCardByNameAndCardNum(bill.out_clerk_name);
            bill.in_manager_num = GetUserCardByNameAndCardNum(bill.in_manager_name);
            bill.out_manager_num = GetUserCardByNameAndCardNum(bill.out_manager_name);

            if (!"计件".Equals(bill.salary_type)) {
                if (string.IsNullOrWhiteSpace(bill.in_minister_name)) throw new Exception("必须选择调入部门部长/助理");
                if (string.IsNullOrWhiteSpace(bill.out_minister_name)) throw new Exception("必须选择调出部门部长/助理");

                bill.in_minister_num = GetUserCardByNameAndCardNum(bill.in_minister_name);
                bill.out_minister_num = GetUserCardByNameAndCardNum(bill.out_minister_name);
            }            

            bill.applier_name = userInfo.name;
            bill.applier_num = userInfo.cardNo;
            bill.apply_time = DateTime.Now;

            FlowSvrSoapClient client = new FlowSvrSoapClient();
            var result = client.StartWorkFlow(JsonConvert.SerializeObject(bill), BillType, userInfo.cardNo, bill.sys_no, entrys.First().out_dep_name + "-->" + entrys.First().in_dep_name, entrys.First().name + "调动申请");
            if (result.suc) {
                try {
                    db.ei_sjApply.Add(bill);
                    foreach (var e in entrys) {
                        e.ei_sjApply = bill;
                        db.ei_sjApplyEntry.Add(e);
                    }
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
                throw new Exception("申请提交失败，原因：" + result.msg);
            }

        }

        public override object GetBill()
        {
            return bill;
        }                

        public override SimpleResultModel HandleApply(System.Web.Mvc.FormCollection fc, UserInfo userInfo)
        {
            int step = Int32.Parse(fc.Get("step"));
            string stepName = fc.Get("stepName");
            bool isPass = bool.Parse(fc.Get("isPass"));
            string opinion = fc.Get("opinion");

            if (!isPass) {
                //拒绝后，将表体的状态也设置为拒绝
                foreach (var e in bill.ei_sjApplyEntry.Where(se => se.is_agree == true).ToList()) {
                    e.is_agree = false;
                    e.not_agree_name = userInfo.name;
                }
            }
            else {
                if (bill.ei_sjApplyEntry.Where(se => se.is_agree == true).Count() == 0) {
                    throw new Exception("所有员工调动都被拒绝的情况下，不能同意此申请，请拒绝此申请");
                }
            }

            JsonSerializerSettings js = new JsonSerializerSettings();
            js.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string formJson = JsonConvert.SerializeObject(bill,js);
            FlowSvrSoapClient flow = new FlowSvrSoapClient();
            var result = flow.BeginAudit(bill.sys_no, step, userInfo.cardNo, isPass, opinion, formJson);
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
                    string ccEmails = "";
                    //List<vw_push_users> pushUsers = db.vw_push_users.Where(v => v.card_number == bill.applier_num).ToList();
                    List<string> pushUsers = new List<string>() { bill.applier_num };
                    if (isSuc) {
                        List<string> additionCardNumber = new List<string>(); //需要额外通知的厂牌
                        additionCardNumber.Add(bill.in_clerk_num); //调入部门文员
                        additionCardNumber.Add(bill.out_clerk_num); //调出部门文员
                        if ("计件".Equals(bill.salary_type)) {
                            additionCardNumber.AddRange(db.flow_notifyUsers.Where(n => n.bill_type == BillType && n.cond1 == "计件").Select(n => n.card_number));
                        }
                        else {
                            additionCardNumber.AddRange(db.flow_notifyUsers.Where(n => n.bill_type == BillType && n.cond1 == "月薪").Select(n => n.card_number));
                        }

                        pushUsers.AddRange(additionCardNumber);
                        //pushUsers.AddRange(
                        //    from v in db.vw_push_users
                        //    where v.wx_push_flow_info == true
                        //    && additionCardNumber.Contains(v.card_number)
                        //    select v
                        //    );
                    }

                    SendEmailForCompleted(
                        bill.sys_no,
                        BillTypeName + "已" + (isSuc ? "批准" : "被拒绝"),
                        bill.applier_name,
                        string.Format("你申请的单号为【{0}】的{1}已{2}，请知悉。", bill.sys_no, BillTypeName, (isSuc ? "批准" : "被拒绝")),
                        GetUserEmailByCardNum(bill.applier_num),
                        ccEmails
                        );

                    //SendWxMessageForCompleted(
                    //    BillTypeName,
                    //    bill.sys_no,
                    //    (isSuc ? "批准" : "被拒绝"),
                    //    pushUsers
                    //    );
                    SendQywxMessageForCompleted(
                        BillTypeName,
                        bill.sys_no,
                        (isSuc ? "批准" : "被拒绝"),
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
                    //SendWxMessageToNextAuditor(
                    //    BillTypeName,
                    //    bill.sys_no,
                    //    result.step,
                    //    result.stepName,
                    //    bill.applier_name,
                    //    ((DateTime)bill.apply_time).ToString("yyyy-MM-dd HH:mm"),
                    //    string.Format("{0}{1}的调动申请", bill.ei_sjApplyEntry.First().name, bill.ei_sjApplyEntry.Count() > 1 ? ("等" + bill.ei_sjApplyEntry.Count() + "人") : ""),
                    //    db.vw_push_users.Where(p => nextAuditors.Contains(p.card_number)).ToList()
                    //    );
                    SendQywxMessageToNextAuditor(
                        BillTypeName,
                        bill.sys_no,
                        result.step,
                        result.stepName,
                        bill.applier_name,
                        ((DateTime)bill.apply_time).ToString("yyyy-MM-dd HH:mm"),
                        string.Format("{0}{1}的调动申请", bill.ei_sjApplyEntry.First().name, bill.ei_sjApplyEntry.Count() > 1 ? ("等" + bill.ei_sjApplyEntry.Count() + "人") : ""),
                        nextAuditors.ToList()
                        );
                }
            }
        }

        public void UpdateHREmpDept(string cardNumber, int depId, DateTime inDate, string position)
        {
            db.UpdateHrEmpDept(cardNumber, depId, inDate, position);
        }


        public object GetBeginAuditOtherInfo(string sysNo, int step)
        {
            return new SJSv(sysNo).GetBill();
        }

        public void UpdateIsAgree(int[] entryIds,string opUser)
        {
            foreach (var e in db.ei_sjApplyEntry.Where(se => entryIds.Contains(se.id) && se.is_agree == true)) {
                e.is_agree = false;
                e.not_agree_name = opUser;
            }
            db.SaveChanges();
        }

        public List<ei_sjApplyEntry> GetEntrys(int sjId)
        {
            return db.ei_sjApplyEntry.Where(e => e.sj_id == sjId).ToList();
        }

    }
}