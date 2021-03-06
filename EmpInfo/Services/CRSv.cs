﻿using EmpInfo.FlowSvr;
using EmpInfo.Interfaces;
using EmpInfo.Models;
using EmpInfo.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpInfo.Services
{
    public class CRSv:BillSv,IApplyEntryQueue
    {
        ei_CRApply bill;
        public CRSv() { }
        public CRSv(string sysNo)
        {
            bill = db.ei_CRApply.Single(c => c.sys_no == sysNo);
        }
        public override string BillType
        {
            get { return "CR"; }
        }

        public override string BillTypeName
        {
            get { return "考勤补记申请单"; }
        }

        public override List<ApplyMenuItemModel> GetApplyMenuItems(UserInfo userInfo)
        {
            var list = base.GetApplyMenuItems(userInfo);

            list.Insert(0, new ApplyMenuItemModel()
            {
                text = "打卡记录查询",
                iconFont = "fa-search",
                url = "../ApplyExtra/CheckMyKQRecord",
            });

            if (db.ei_department.Where(d => d.FReporter.Contains(userInfo.cardNo)).Count() > 0) {
                list.Add(new ApplyMenuItemModel()
                {
                    text = "考勤补记报表",
                    iconFont = "fa-file-text-o",
                    url = "../Report/CRReport",
                    linkClass = "screen_900"
                });
            }
            return list;
        }

        public override object GetInfoBeforeApply(UserInfo userInfo, UserInfoDetail userInfoDetail)
        {
            CRSVBeforeApplyModel m = new CRSVBeforeApplyModel();
            var appliedBills = db.ei_askLeave.Where(a => a.applier_num == userInfo.cardNo).ToList();
            if (appliedBills.Count() > 0) {
                var ab = appliedBills.OrderByDescending(a => a.id).First();
                if (ab.dep_long_name.Equals(GetDepLongNameByNum(ab.dep_no))) {
                    m.depName = ab.dep_long_name;
                    m.depNum = ab.dep_no;
                    m.depId = ab.dep_id;
                }
            }
            m.sysNum = GetNextSysNum(BillType, 4);
            return m;
        }

        public override void SaveApply(System.Web.Mvc.FormCollection fc, UserInfo userInfo)
        {
            bill = new ei_CRApply();
            MyUtils.SetFieldValueToModel(fc, bill);
            bill.applier_name = userInfo.name;
            bill.applier_num = userInfo.cardNo;
            bill.apply_time = DateTime.Now;

            if (bill.has_attachment == false && bill.reason.Equals("补办厂牌")) {
                throw new Exception("原因是补办厂牌的，必须上传附件！");
            }

            //2021-03-13 罗继旺要求的限制，因为有人选错，跑到陈胜能那里
            if (bill.dep_no.StartsWith("202")) {
                throw new Exception("请确认部门后再提交，人力资源部不需处理楼刷卡流程");
            }

            //处理一下审核队列,将姓名（厂牌）格式更换为厂牌
            if (!string.IsNullOrEmpty(bill.auditor_queues)) {
                var queueList = JsonConvert.DeserializeObject<List<flow_applyEntryQueue>>(bill.auditor_queues);
                queueList.ForEach(q => q.auditors = GetUserCardByNameAndCardNum(q.auditors));
                if (queueList.Where(q => q.auditors.Contains("离职")).Count() > 0) {
                    throw new Exception("流程审核人中存在已离职的员工，请联系部门管理员处理" );
                }
                bill.auditor_queues = JsonConvert.SerializeObject(queueList);
            }

            if (!bill.dep_long_name.Contains(@"/")) {
                throw new Exception("请点击文件夹图标展开子部门，选择到具体部门再提交！");
            }

            FlowSvrSoapClient client = new FlowSvrSoapClient();
            var result = client.StartWorkFlow(JsonConvert.SerializeObject(bill), BillType, userInfo.cardNo, bill.sys_no, ((DateTime)bill.forgot_date).ToString("yyyy-MM-dd"), bill.time1 ?? "" + " " + bill.time2 ?? "" + " " + bill.time3 ?? "" + " " + bill.time4 ?? "");
            if (result.suc) {
                try {
                    db.ei_CRApply.Add(bill);

                    db.SaveChanges();
                }
                catch (Exception ex) {
                    //将生成的流程表记录删除
                    client.DeleteApplyForFailure(bill.sys_no);
                    throw new Exception("申请提交失败，原因：" + ex.Message );
                }

                SendNotification(result);
            }
            else {
                throw new Exception("申请提交失败，原因是：" + result.msg );
            }
        }

        public override object GetBill()
        {            
            return bill;
        }

        public override SimpleResultModel HandleApply(System.Web.Mvc.FormCollection fc, UserInfo userInfo)
        {
            int step = Int32.Parse(fc.Get("step"));
            bool isPass = bool.Parse(fc.Get("isPass"));
            string opinion = fc.Get("opinion");

            string formJson = JsonConvert.SerializeObject(bill);
            FlowSvrSoapClient flow = new FlowSvrSoapClient();
            var result = flow.BeginAudit(bill.sys_no, step, userInfo.cardNo, isPass, opinion, formJson);
            if (result.suc) {
                //发送通知到下一级审核人
                SendNotification(result);
            }
            
            return new SimpleResultModel() { suc = result.suc, msg = result.msg };
        }

        public override void SendNotification(FlowResultModel model)
        {
            if (model.suc) {
                if (model.msg.Contains("完成") || model.msg.Contains("NG")) {
                    bool isSuc = model.msg.Contains("NG") ? false : true;

                    SendEmailForCompleted(
                        bill.sys_no,
                        BillTypeName + "已" + (isSuc ? "批准" : "被拒绝"),
                        bill.applier_name,
                        string.Format("你申请的单号为【{0}】的{1}已{2}，请知悉。", bill.sys_no, BillTypeName, (isSuc ? "批准" : "被拒绝")),
                        GetUserEmailByCardNum(bill.applier_num)
                        );

                    //SendWxMessageForCompleted(
                    //    BillTypeName,
                    //    bill.sys_no,
                    //    (isSuc ? "批准" : "被拒绝"),
                    //    db.vw_push_users.Where(v => v.card_number == bill.applier_num).FirstOrDefault()
                    //    );
                    SendQywxMessageForCompleted(
                        BillTypeName, 
                        bill.sys_no,
                        (isSuc ? "审批通过" : "审批不通过"),
                        new List<string>() { bill.applier_num }
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
                    //    string.Format("漏刷日期：{0:yyyy-MM-dd}；漏刷时间：{1} {2} {3} {4}", bill.forgot_date,bill.time1,bill.time2,bill.time3,bill.time4),
                    //    db.vw_push_users.Where(p => nextAuditors.Contains(p.card_number)).ToList()
                    //    );

                    SendQywxMessageToNextAuditor(
                        BillTypeName,
                        bill.sys_no,
                        result.step,
                        result.stepName,
                        bill.applier_name,
                        ((DateTime)bill.apply_time).ToString("yyyy-MM-dd HH:mm"),
                        string.Format("漏刷日期：{0:yyyy-MM-dd}；漏刷时间：{1} {2} {3} {4}", bill.forgot_date,bill.time1,bill.time2,bill.time3,bill.time4),
                        nextAuditors.ToList()
                        );

                }
            }
        }

        public List<flow_applyEntryQueue> GetApplyEntryQueue(System.Web.Mvc.FormCollection fc, UserInfo userInfo)
        {
            bill = new ei_CRApply();
            MyUtils.SetFieldValueToModel(fc, bill);
            bill.applier_name = userInfo.name;
            bill.applier_num = userInfo.cardNo;
            bill.apply_time = DateTime.Now;

            //if (!bill.dep_no.StartsWith("104")) {
            //    throw new Exception("此流程的部门只支持选择信利电子子部门" );
            //}

            FlowSvrSoapClient client = new FlowSvrSoapClient();
            var result = client.GetFlowQueue(JsonConvert.SerializeObject(bill), BillType);
            List<flow_applyEntryQueue> queue = null;

            if (result.suc) {
                queue = JsonConvert.DeserializeObject<List<flow_applyEntryQueue>>(result.msg);
                queue.ForEach(q => q.auditors = GetUserNameAndCardByCardNum(q.auditors));
                return queue;
            }
            else {
                throw new Exception(result.msg);
            }
        }

    }
}