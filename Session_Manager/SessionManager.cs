using DAL;
using PublicLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Session_Manager
{
    public class SessionManager
    {
        private static object lockObj = new object();
        public static Dictionary<string, string> SessionIdBuff = new Dictionary<string, string>();

        public static void SaveSession(HttpContext context, ISessionObject session)
        {
            //IoCContainer.Current.GetInstance<DAL.ILogger>().WriteMessage(LogLevel.Fatal, "写session时的sessionId是" + context.Session.SessionID);
            
            if (context.Session != null)
            {
                context.Session.Clear();
            }
            //HttpContext.Current.Session["user"] = session;
            context.Session["user"] = session;
            context.Session.Add("au_id", session.AU_ID);
            context.Session.Add("au_name", session.AU_Name);
            context.Session.Add("AU_UserAccount", session.AU_UserAccount);
            context.Session.Add("r_id", session.R_ID);

            context.Response.Cookies.Clear();
            context.Response.Cookies.Add(new HttpCookie("au_id", session.AU_ID));
            context.Response.Cookies.Add(new HttpCookie("au_name", session.AU_Name));
            context.Response.Cookies.Add(new HttpCookie("au_useraccount",session.AU_UserAccount));
            context.Response.Cookies.Add(new HttpCookie("r_id", session.R_ID));

            if (SessionIdBuff.ContainsKey(session.AU_ID))
            {
                SessionIdBuff[session.AU_ID] = context.Session.SessionID;
            }
            else
            {
                SessionIdBuff.Add(session.AU_ID, context.Session.SessionID);
            }
        }
        public static ISessionObject GetSession(HttpContext context)
        {
            //IoCContainer.Current.GetInstance<DAL.ILogger>().WriteMessage(LogLevel.Fatal, "获取session时的sessionId是" + context.Session.SessionID);

            if (context.Session == null || string.IsNullOrWhiteSpace(context.Session.SessionID)) { return null; }
            var session = SessionFactory.Create();
            try
            {
                session.AU_ID = context.Session["au_id"].ToString();
                session.AU_Name = context.Session["au_name"].ToString();
                session.AU_UserAccount = context.Session["au_useraccount"].ToString();
                session.R_ID = context.Session["r_id"].ToString();
            }
            catch (Exception e)
            {
                //IoCContainer.Current.GetInstance<DAL.ILogger>().WriteException(LogLevel.Fatal, e);
                return null;
            }
            return session;
        }
        public static void RefreshSesion(HttpContext context)
        {
            var userId = GetSession(context, "au_id");
            UserAccountManagerInfo userMgr = new UserAccountManagerInfo();
            userMgr.refreshUserInfo(Convert.ToInt32(userId));

            context.Session["r_id"] = userMgr.r_id.ToString();
            context.Session["au_name"] = userMgr.au_name;
            context.Session["au_useraccount"] = userMgr.au_useraccount;
            context.Session["au_id"] = userMgr.au_id;

            context.Response.Cookies.Set(new HttpCookie("au_id", userMgr.au_id.ToString()));
            context.Response.Cookies.Set(new HttpCookie("r_id", userMgr.r_id.ToString()));
            context.Response.Cookies.Set(new HttpCookie("au_name", userMgr.au_name.ToString()));
            context.Response.Cookies.Set(new HttpCookie("au_useraccount", userMgr.au_useraccount.ToString()));
        }
        public static void AddSession(HttpContext context, string key, object value)
        {
            context.Session.Add(key, value);
        }
        public static void RemoveSession(HttpContext context, string key)
        {
            if (context.Session.Keys.Cast<string>().Contains(key))
                context.Session.Remove(key);
        }
        public static object GetSession(HttpContext context, string key)
        {
            if (context.Session.Keys.Cast<string>().Contains(key))
                return context.Session[key];
            return null;
        }
        public static void RecordLogin(HttpContext context, string username, DateTime datetime)
        {
            lock (lockObj)
            {
                var users = HttpRuntime.Cache["loginUser"] as Dictionary<string, DateTime>;
                if (users == null)
                {
                    users = new Dictionary<string, DateTime>();
                    users.Add(username, datetime);
                    HttpRuntime.Cache.Add("loginUser",
                        users,
                        null,
                        System.Web.Caching.Cache.NoAbsoluteExpiration,
                        System.Web.Caching.Cache.NoSlidingExpiration,
                        System.Web.Caching.CacheItemPriority.High,
                        null
                        );
                }
                else
                {
                    if (users.Any(p => p.Key == username)) { users.Remove(username); }
                    users.Add(username, datetime);

                    HttpRuntime.Cache.Remove("loginUser");
                    HttpRuntime.Cache.Add("loginUser",
                        users,
                        null,
                        System.Web.Caching.Cache.NoAbsoluteExpiration,
                        System.Web.Caching.Cache.NoSlidingExpiration,
                        System.Web.Caching.CacheItemPriority.High,
                        null
                        );
                }
            }
        }
        public static void RemoveRecordedLogin(HttpContext context)
        {
            lock (lockObj)
            {
                var username = string.Empty;
                var sessionObj = SessionManager.GetSession(context);
                if (sessionObj != null) { username = sessionObj.AU_Name; }
                var users = HttpRuntime.Cache["loginUser"] as Dictionary<string, DateTime>;
                if (users != null && users.Any(p => p.Key == username))
                {
                    users.Remove(username);
                    HttpRuntime.Cache.Remove("loginUser");
                    HttpRuntime.Cache.Add("loginUser",
                        users,
                        null,
                        System.Web.Caching.Cache.NoAbsoluteExpiration,
                        System.Web.Caching.Cache.NoSlidingExpiration,
                        System.Web.Caching.CacheItemPriority.High,
                        null
                        );
                }
            }
        }
        public static Dictionary<string, DateTime> GetLoginedUser(HttpContext context)
        {
            return HttpRuntime.Cache["loginUser"] as Dictionary<string, DateTime>;
        }

        //用户是否已重新登陆
        public static bool CheckUserIsRepeatLogin(HttpContext context)
        {
            var userId = GetSession(context).AU_ID;
            var sessionId = context.Session.SessionID;
            if (SessionIdBuff.ContainsKey(userId))
            {
                return SessionIdBuff[userId] != sessionId;
            }
            return false;
        }
    }

    /// <summary>
    /// session工厂
    /// </summary>
    public class SessionFactory
    {
        public static ISessionObject Create()
        {
            return new SessionObject();
        }

        public static ISessionObject Create(string AU_ID , string R_ID, string AU_Name , string AU_UserAccount)
        {
            return new SessionObject()
            {
                AU_ID = AU_ID,
                AU_Name = AU_Name,
                R_ID = R_ID,
                AU_UserAccount = AU_UserAccount,
            };
        }
    }

    //session接口
    public interface ISessionObject
    {
        /// <summary>
        /// 后台登陆用户id
        /// </summary>
        string AU_ID { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        string R_ID { get; set; }
        /// <summary>
        /// 登陆账号
        /// </summary>
        string AU_UserAccount { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        string AU_Name { get; set; }
    }

    public class SessionObject : ISessionObject
    {
        public string AU_ID { get; set; }
        public string R_ID { get; set; }
        public string AU_UserAccount { get; set; }
        public string AU_Name { get; set; }
    }

    public class UserAccountManagerInfo
    {
        public int au_id = 0;
        public int r_id = 0;
        public int au_useraccount = 0;
        public int au_name = 0;
        public void refreshUserInfo(int subUserId)
        {

            string sql = "select CRM_User_Account.rec_no,crm_sub_user.user_name,CRM_Sub_User.Department_rec,CRM_Department.level_no as departmentlevelno,crm_role.level_no,crm_account.companyname,CRM_User_Account.useraccount,crm_account.rec_no as account_rec,crm_user_account.role_rec,crm_sub_user.dealer_rec "
                + " from crm_account inner join CRM_User_Account on crm_account.rec_no=crm_user_account.account_rec inner join crm_sub_user on CRM_User_Account.rec_no=crm_sub_user.rec_no join crm_role on crm_user_account.role_rec=crm_role.rec_no left join crm_department on crm_sub_user.department_rec=crm_department.rec_no"
                + " where getdate() between isnull(crm_user_account.start_date,'1900-01-01') and isnull(crm_user_account.end_date,'2050-12-30') and CRM_User_Account.rec_no=" + subUserId;

            SqlDataReader reader = DBHelper.GetExecuteReader(sql);
            //if (reader.Read())
            //{
            //    account_rec = int.Parse(reader["account_rec"].ToString());
            //    userid = int.Parse(reader["rec_no"].ToString());
            //    employeename = reader["user_name"].ToString();
            //    department = reader["department_rec"].ToString();
            //    rolelevel = reader["level_no"].ToString();
            //    companyname = reader["companyname"].ToString();
            //    accountname = reader["useraccount"].ToString();
            //    role_rec = int.Parse(reader["role_rec"].ToString());
            //    departmentlevelno = SetUnit.ToString(reader["departmentlevelno"]);
            //    dealer_rec = reader["dealer_rec"] != DBNull.Value ? Convert.ToInt32(reader["dealer_rec"]) : 0;
            //}
            reader.Close();
        }
    }
}
