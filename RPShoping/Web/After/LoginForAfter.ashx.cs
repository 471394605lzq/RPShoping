using DAL;
using Model;
using PublicLibrary;
using Session_Manager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace RPShoping.Web.After
{
    /// <summary>
    /// LoginForAfter 的摘要说明
    /// </summary>
    public class LoginForAfter : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string ajaxtype = context.Request["ajax"].ToString();
            string returnstr = string.Empty;
            switch (ajaxtype)
            {
                case "IsPostBack":
                    /*InitControl(context);
                    returnstr = AutoLogin(context);*/
                    break;
                case "Login":
                    returnstr = HandleLogin(context);
                    break;
                case "Register":
                    returnstr = Register(context);
                    break;
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnstr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        protected string HandleLogin(HttpContext context)
        {
            bool result = false;
            string errorInfo = "";
            var rememberPwd = false;
            var usename = context.Request["usname"].ToString();
            var password = context.Request["pwd"].ToString();
            errorInfo = VerificationLogin(usename, password, out result, context);
            if (!result)
                return "111110***" + errorInfo;
            return "111111***成功";
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Register(HttpContext context)
        {
            var username = context.Request["regUsername"].ToString().Trim();
            var validateCode = context.Request["regValidCode"].ToString().Trim();
            var password = context.Request["regPwd"].ToString().Trim();
            var confirmPwd = context.Request["regConfirmPwd"].ToString().Trim();
            var remember = context.Request["regRememberPwd"].ToString().Trim();
            var sessionValidateCode = context.Session["validateCode"];

            if ((!username.IsNumber()) && (!username.IsEmailAddress())) { return "111110***输入的手机号不正确"; }

            if (string.IsNullOrWhiteSpace(validateCode) || sessionValidateCode == null || !sessionValidateCode.ToString().Split(',').Contains(validateCode))
                return "输入的验证码不正确，请重试";

            var outParam = new SqlParameter("@result", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Output, 0, 0, "", System.Data.DataRowVersion.Current, true, null, null, null, null);
            DBHelper.RunProcedure("CRM_RegUserAccount",
                new SqlParameter("@mobile", username),
                new SqlParameter("@password", password),
                outParam
                );
            var result = outParam.Value.ToString();
            if (result == "opResult:1")
                return "111111***注册成功!";
            return "111110***" + result;
        }

        private string VerificationLogin(string username, string password, out bool result, HttpContext context)
        {
            var dt = DateTime.Now;
            result = false;
            bool isExists = false;
            string errorInfo = string.Empty;
            UserManager uservalidate = new UserManager();

            //if (string.IsNullOrWhiteSpace(companyAccount)) return "公司帐号不能为空";
            if (string.IsNullOrWhiteSpace(username)) return "用户名不能为空";
            if (string.IsNullOrWhiteSpace(password)) return "用户密码不能为空";
            //if (!new EncryptManager().QueryEncryptHardwareState()) return "请检查加密狗或授权是否正确";
            //if (UserManager.ValidUserCount(companyAccount) > 0 && username.ToLower() != "admin") { return "用户数已超最大限制,请用admin帐号删除部分用户."; }
            try
            {
                //设置错误提示信息
                errorInfo = string.Format("登陆失败，参数详情:userAccount:{0},password:{1}", username, password);
                isExists = uservalidate.isExistS(username, password);
            }
            catch (Exception ex)
            {
            }
            if (!isExists)
            {
                return "用户或密码不正确！";
            }


            //缓存Session
            var session = SessionFactory.Create(uservalidate.userid.ToString(), uservalidate.role_id.ToString(),
                uservalidate.adminusername,
                uservalidate.adminuseraccount.ToString()
                );
            SessionManager.SaveSession(HttpContext.Current, session);
            SessionManager.RecordLogin(HttpContext.Current, uservalidate.adminusername, DateTime.Now);

            //记住密码
            WriteAutoLoginCookies(username, password, context);
            result = true;
            //写登陆日志
            uservalidate.AddLoginLog(session);

            return string.Empty;
        }

        /// <summary>
        /// 保存当前用户账号密码为期15天
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="companyAccount"></param>
        /// <param name="context"></param>
        private void WriteAutoLoginCookies(string username, string password, HttpContext context)
        {
            HttpCookie cookies = new HttpCookie("autoLogin");
            string us = Unite.ToMD5New(username);
            string p_d = Unite.ToMD5New(password);
            string pname = Unite.ToMD5New("pwd");
            cookies.Values.Add("username", us);
            cookies.Values.Add(pname, p_d);
            //cookies.Values.Add("companyAccount", companyAccount);
            cookies.Expires = DateTime.Now.AddDays(14);
            context.Response.Cookies.Add(cookies);
        }
    }
}