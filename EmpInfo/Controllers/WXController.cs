﻿using EmpInfo.Models;
using EmpInfo.Services;
using EmpInfo.Util;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;
using EmpInfo.Interfaces;
using EmpInfo.FlowSvr;

namespace EmpInfo.Controllers
{
    public class WXController : BaseController
    {
        #region 微信公众号接口 2020-7-1停用，转到企业微信

        private string redirectIndex = "http://emp.truly.com.cn/Emp/WX/WIndex?cardnumber={0}&secret={1}";
        private string wLoginUrl = "http://emp.truly.com.cn/Emp/WX/WLogin?openId=";
        private string wLogoutUrl = "http://emp.truly.com.cn/Emp/WX/WLogout?openId=";
        private string wSystemLoginUrl = "http://emp.truly.com.cn/Emp/Account/Login?from=wx";
        private string wWorkUrl = "http://emp.truly.com.cn/Emp/WX/WIndex?cardnumber={0}&secret={1}&controllerName=Home&actionName=WorkGroupIndex";

        //查询openid是否已登记
        public string QueryOpenId(string openId)
        {
            try {
                openId = MyUtils.AESDecrypt(openId);
            }
            catch {
                return JsonConvert.SerializeObject(new { suc = 0 });
            }

            var emps = db.ei_users.Where(u => u.wx_openid == openId && u.wx_easy_login == true);
            if (emps.Count() == 0) {
                //没有绑定过的跳转到绑定界面
                return JsonConvert.SerializeObject(new { suc = 0, url = MyUtils.AESEncrypt(wLoginUrl), logUrl = MyUtils.AESEncrypt(wSystemLoginUrl) });
            }
            var emp = emps.First();
            string url = string.Format(redirectIndex, emp.card_number, MyUtils.getMD5(emp.card_number));

            //已绑定过的，返回系统主界面和解除绑定界面，由服务号选择使用
            return JsonConvert.SerializeObject(new { suc = 1, url = MyUtils.AESEncrypt(url), ourl = MyUtils.AESEncrypt(wLogoutUrl), cardNo = MyUtils.AESEncrypt(emp.card_number) });
        }

        //进入智慧办公的接口
        public string WorkGroupInterface(string openId)
        {
            try {
                openId = MyUtils.AESDecrypt(openId);
            }
            catch {
                return JsonConvert.SerializeObject(new { suc = 0, msg = "openid不合法" });
            }
            var emps = db.ei_users.Where(u => u.wx_openid == openId && u.wx_easy_login == true);
            if (emps.Count() == 0) {
                return JsonConvert.SerializeObject(new { suc = 0, msg = "用户未绑定" });
            }
            var emp = emps.First();
            string url = string.Format(wWorkUrl, emp.card_number, MyUtils.getMD5(emp.card_number));

            //已绑定过的，返回系统主界面和解除绑定界面，由服务号选择使用
            return JsonConvert.SerializeObject(new { suc = 1, url = MyUtils.AESEncrypt(url) });
        }

        //查询宿舍的接口
        public string QueryDormInfo(string openId)
        {
            try {
                openId = MyUtils.AESDecrypt(openId);
            }
            catch {
                return JsonConvert.SerializeObject(new { suc = 0, msg = "openId不合法" });
            }
            var emps = db.ei_users.Where(u => u.wx_openid == openId && u.wx_easy_login == true);
            if (emps.Count() == 0) {
                return JsonConvert.SerializeObject(new { suc = 0, msg = "请绑定微信公众号【信利e家】后再操作" });
            }
            var emp = emps.First();

            //测试
            if (HasGotPower("ModuelTest",emp.id)) {
                return JsonConvert.SerializeObject(
                new
                {
                    suc = 1,
                    cardNumber = emp.card_number,
                    userName = emp.name,
                    dormNumber = "A103",
                    areaNumber = "一区",
                    imgLink = "http://emp.truly.com.cn/Emp/Home/GetEmpPortrait?card_no=" + emp.card_number
                });
            }

            var info = db.GetEmpDormInfo(emp.card_number).ToList();
            if (info.Count() == 0) {
                return JsonConvert.SerializeObject(new { suc = 0, msg = "员工当前没有住宿，不能申请快件寄存" });
            }
            return JsonConvert.SerializeObject(
                new
                {
                    suc = 1,
                    cardNumber = emp.card_number,
                    userName = emp.name,
                    dormNumber = info.First().dorm_number,
                    areaNumber = info.First().area,
                    imgLink = "http://emp.truly.com.cn/Emp/Home/GetEmpPortrait?card_no=" + emp.card_number
                });
        }

        //微信openid认证通过跳转的页面
        public ActionResult WIndex(string cardnumber, string secret, string controllerName = "Home", string actionName = "Index",string param="")
        {
            return Content("微信公众号中的移动办公功能已于2020-07-01全部停用，请移步企业微信操作");            

            string msg = "";
            bool suc = false;
            if (!MyUtils.getMD5(cardnumber).Equals(secret)) {
                msg = "验证不通过，授权失败";
            }
            else {
                var users = db.ei_users.Where(u => u.card_number == cardnumber).ToList();
                if (users.Count() == 0) {
                    msg = "厂牌号不存在";
                }
                else {
                    var user = users.First();
                    if (user.forbit_flag == true) {
                        msg = "用户已被禁用";
                    }
                    else {
                        if (!new HRDBSv().EmpIsNotQuit(cardnumber)) {
                            msg = "用户已离职，不能使用此系统";
                        }
                        else {
                            suc = true;
                            msg = "授权成功";
                            user.last_login_date = DateTime.Now;
                            setcookie(user, 100);
                            Session["fromwx"] = true;
                        }
                    }
                }
            }
            WriteEventLog("微信公众号", "登陆" + (suc ? "成功" : "失败") + ";msg:" + msg + ";controler:" + controllerName + ";action:" + actionName);
            if (!suc) {
                ViewBag.tip = msg;
                return View("Error");
            }
            return RedirectToAction(actionName, controllerName, new { param = param });
        }

        //绑定openid页面
        public ActionResult WLogin(string openId)
        {
            if (string.IsNullOrEmpty(openId)) {
                ViewBag.tip = "openId不能为空";
                return View("Error");
            }
            try {
                openId = MyUtils.AESDecrypt(openId);
            }
            catch {
                ViewBag.tip = "openId不合法";
                return View("Error");
            }
            ViewData["openid"] = openId;
            return View();
        }

        //开始绑定
        public JsonResult BindOpenId(string cardNumber, string password, string openid, bool checkSalaryInfo, bool pushSalaryInfo, bool pushConsumeInfo, bool pushFlowInfo)
        {
            string msg = "";
            bool suc = false;
            if (string.IsNullOrEmpty(cardNumber)) {
                msg = "厂牌号不能为空";
            }
            else {
                var users = db.ei_users.Where(u => u.card_number == cardNumber).ToList();
                if (users.Count() == 0) {
                    msg = "厂牌号不存在，请注册后再绑定";
                }
                else {
                    var user = users.First();
                    int maxFailTimes = 5;
                    if (!MyUtils.getMD5(password).Equals(user.password)) {
                        user.fail_times = user.fail_times == null ? 1 : user.fail_times + 1;
                        if (user.fail_times >= maxFailTimes) {
                            user.fail_times = 0;
                            user.forbit_flag = true;
                            msg = "连续" + maxFailTimes + "次密码输入错误，已禁用";
                        }
                        else {
                            msg = "已连续" + user.fail_times + "次密码错误，你还剩下" + (maxFailTimes - user.fail_times) + "次尝试机会。";
                        }
                    }
                    else {
                        string passwordInfo = MyUtils.ValidatePassword(user.password);
                        if (!string.IsNullOrEmpty(passwordInfo)) {
                            msg = "密码不符合复杂度规则验证，请先登录系统修改密码后再绑定";
                        }
                        else {
                            suc = true;
                            user.wx_openid = openid;
                            user.wx_bind_date = DateTime.Now;
                            user.wx_easy_login = true;
                            user.wx_should_push_msg = true;

                            user.wx_push_consume_info = pushConsumeInfo;//推送消费信息
                            user.wx_push_flow_info = pushFlowInfo;//推送业务流程信息
                            user.wx_push_salary_info = pushSalaryInfo;//推送工资信息
                            user.wx_check_salary_info = checkSalaryInfo;//免密工资查看
                        }
                    }
                }
            }
            WriteEventLog("微信服务号", "绑定openid：" + openid + ";card:" + cardNumber);
            return Json(new { suc = suc ? 1 : 0, msg = msg });
        }

        //解除绑定界面
        public ActionResult WLogOut(string openId)
        {
            if (string.IsNullOrEmpty(openId)) {
                ViewBag.tip = "openId 不能为空";
                return View("Error");
            }
            try {
                openId = MyUtils.AESDecrypt(openId);
            }
            catch {
                ViewBag.tip = "openId不合法";
                return View("Error");
            }
            var user = db.ei_users.Where(u => u.wx_openid == openId).ToList();
            if (user.Count() == 0) {
                ViewBag.tip = "没有绑定过，不能解绑";
                return View("Error");
            }
            ViewData["bingDate"] = ((DateTime)user.First().wx_bind_date).ToString("yyyy-MM-dd HH:mm:ss");
            ViewData["userName"] = user.First().name;
            ViewData["userNumber"] = user.First().card_number;
            ViewData["openid"] = openId;
            return View();
        }

        public JsonResult UnBindOpenId(bool pushFlag, string openid)
        {
            var users = db.ei_users.Where(u => u.wx_openid == openid);
            if (users.Count() == 0) {
                return Json(new { suc = false, msg = "用户未绑定过" });
            }
            var user = users.First();

            user.wx_easy_login = false;
            user.wx_openid = null;
            user.wx_check_salary_info = false;//免密工资查看
            if (pushFlag) {
                user.wx_should_push_msg = false;
                user.wx_push_consume_info = false; //推送消费信息
                user.wx_push_flow_info = false; //推送业务流程信息
                user.wx_push_salary_info = false; //推送工资信息
            }
            db.SaveChanges();
            WriteEventLog("微信服务号", "解除绑定：" + openid);
            return Json(new { suc = true });
        }

        //取消关注公众号事件中需要调用的接口
        public string UnsubscribeWx(string openId)
        {
            if (string.IsNullOrEmpty(openId)) {
                return JsonConvert.SerializeObject(new { suc = false, msg = "openId不能为空" });
            }
            try {
                openId = MyUtils.AESDecrypt(openId);
            }
            catch {
                return JsonConvert.SerializeObject(new { suc = false, msg = "openId不合法" });
            }

            var users = db.ei_users.Where(u => u.wx_openid == openId);
            if (users.Count() == 0) {
                return JsonConvert.SerializeObject(new { suc = false, msg = "用户未绑定过" });
            }
            var user = users.First();

            user.wx_easy_login = false;
            user.wx_check_salary_info = false;//免密工资查看            
            user.wx_should_push_msg = false;
            user.wx_push_consume_info = false; //推送消费信息
            user.wx_push_flow_info = false; //推送业务流程信息
            user.wx_push_salary_info = false; //推送工资信息
            
            db.SaveChanges();
            WriteEventLog("微信服务号", "取消关注：" + openId);
            return JsonConvert.SerializeObject(new { suc = true });
        }
        
        public JsonResult HasBindWx()
        {
            bool hasBind = db.vw_push_users.Where(v => v.card_number == userInfo.cardNo).Count() > 0;
            string msg = hasBind ? "" : "必须绑定【信利e家】微信公众号之后才能进行申请";
            return Json(new SimpleResultModel() { suc = hasBind, msg = msg });
        }

        public string AES(string str)
        {
            return Uri.EscapeDataString(MyUtils.AESEncrypt(str));
        }

        public string AESD(string str)
        {
            return MyUtils.AESDecrypt(str);
        }

        #endregion

        #region 微信公众号js接口
        public ActionResult GetLocation()
        {
            WxSv sv = new WxSv();
            WxConfigParam p = new WxConfigParam();
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            p.timestamp = Convert.ToInt64(ts.TotalSeconds);
            p.appId = sv.APPID;
            p.nonceStr = MyUtils.CreateValidateNumber(8);
            try {
                p.signature = sv.GetSignature(p.nonceStr, p.timestamp.ToString(), Request.Url.ToString());
            }
            catch (Exception ex) {
                ViewBag.tip = ex.Message;
                return View("Error");
            }
            ViewData["wxConfigParam"] = p;

            return View();
        }

        public ActionResult JsInterface(string actionType, string debug = "false")
        {
            WxSv sv = new WxSv();
            WxConfigParam p = new WxConfigParam();
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            p.timestamp = Convert.ToInt64(ts.TotalSeconds);
            p.appId = sv.APPID;
            p.nonceStr = MyUtils.CreateValidateNumber(8);
            p.debug = debug;
            p.actionType = actionType;
            
            try {
                p.signature = sv.GetSignature(p.nonceStr, p.timestamp.ToString(), Request.Url.ToString());
            }
            catch (Exception ex) {
                ViewBag.tip = ex.Message;
                return View("Error");
            }
            ViewData["wxConfigParam"] = p;

            return View();
        }

        /// <summary>
        /// 处理从微信JS接口返回的结果
        /// </summary>
        /// <param name="actionType">js接口类型，例如scanQRCode为扫一扫</param>
        /// <param name="result">格式是单据类型:扫描结果，例如IT:IT200222001</param>
        /// <returns></returns>
        public ActionResult HandleJsResult(string actionType, string result)
        {
            try {
                switch (actionType) {
                    case "scanQRCode":
                        var resultArr = result.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        SetBillByType(resultArr[0]);
                        var iJs = bill as IJsInterface;
                        if (iJs != null) {
                            var iJsResult = iJs.HandleJsInterface(resultArr[1], userInfo);
                            return RedirectToAction(iJsResult.actionName, iJsResult.controllerName, iJsResult.routetValues);
                        }
                        break;
                }
            }
            catch (Exception ex) {
                return Content("二维码内容:" + result + ";提示信息:" + ex.Message);
            }
            return Content(result);
        }

        #endregion

        #region 厂外宿舍维修流程

        //测试地址：http://localhost/emp/WX/DormRepairOutside?openId=72G4jHFXZ7QVQcUVz0cnLMIaTjDeQHVdkUr2aHcx76U%3D
        public ActionResult DormRepairOutside(string openId)
        {
            try {
                openId = AESD(openId);
            }
            catch (Exception ex) {
                ViewData["tip"] = ex.Message;
                return View("Error");
            }

            ei_users user = new ei_users();//厂外的没有用户表信息，模拟一个
            user.card_number = openId;
            user.id = 0;

            var apply = db.flow_apply.Where(f => f.create_user == openId && f.success == null).FirstOrDefault();
            if (apply != null) {
                user.name = db.ei_dormRepair.Where(d => d.sys_no == apply.sys_no).First().applier_name;
                setcookie(user, 3);

                //存在已申请未结束的流程
                var isMyStep = apply.flow_applyEntry.Where(f => f.auditors == openId && f.pass == null).FirstOrDefault();
                if (isMyStep != null) {
                    //之前的申请是未评价的，直接跳到评价界面
                    return RedirectToAction("BeginAuditApply", "Apply", new { sysNo = apply.sys_no, step = isMyStep.step });
                }
                else {
                    //跳到查询界面
                    return RedirectToAction("CheckDPApplyInDormSys", "Apply", new { sysNo = apply.sys_no });
                }
            }

            setcookie(user, 3);

            //需要新增的
            DormRepairBeginApplyModel m = new DormRepairBeginApplyModel();
            m.sysNo = GetNextSysNum("DP");
            m.roomMaleList = "";

            ViewData["infoBeforeApply"] = m;

            return View();
        }

        public JsonResult BeginApplyDPOutside(FormCollection fc)
        {
            var bill = new ei_dormRepair();
            MyUtils.SetFieldValueToModel(fc, bill);

            bill.is_outside = true;
            bill.apply_time = DateTime.Now;
            bill.applier_num = userInfo.cardNo;
            bill.applier_name = bill.contact_name;
            bill.is_outside = true;

            if (!"预约维修".Equals(bill.repair_type)) {
                bill.repair_time = null;
            }

            if (bill.area_name.Contains("红草") && !bill.dorm_num.Contains("HC")) {
                bill.dorm_num = "HC" + bill.dorm_num;
            }

            if (bill.contact_name.Length < 2) {
                return Json(new SimpleResultModel(false, "联系人姓名至少要有2个字符"));
            }

            //验证宿舍区、宿舍和房主是否匹配,并通过引用方式带回费用承担方式和需要扣费的empid
            try {
                new DormSv().validateDPOutside(bill);
            }
            catch (Exception ex) {
                return Json(new SimpleResultModel(ex));
            }

            FlowSvrSoapClient client = new FlowSvrSoapClient();
            var result = client.StartWorkFlow(JsonConvert.SerializeObject(bill), "DP", userInfo.cardNo, bill.sys_no, bill.repair_content, bill.area_name + "_" + bill.dorm_num);
            if (result.suc) {
                try {
                    db.ei_dormRepair.Add(bill);
                    db.SaveChanges();
                }
                catch (Exception ex) {
                    //将生成的流程表记录删除
                    client.DeleteApplyForFailure(bill.sys_no);
                    return Json(new SimpleResultModel(false,"申请提交失败，原因：" + ex.Message));
                }

                //发送通知到下一环节                
                new DPSv().SendNotification(result);
            }
            else {
                return Json(new SimpleResultModel(false,"申请提交失败，原因：" + result.msg));
            }

            return Json(new SimpleResultModel(true, "流水号：" + bill.sys_no));
        }

        #endregion

    }
}
