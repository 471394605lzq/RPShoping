using DAL;
using PublicLibrary;
using Session_Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Session_Manager
{
    public class UserManager
    {
        public int userid = 0;
        public int role_id = 0;
        public string adminuseraccount = "";
        public string adminusername = "";

        private string SyncInitialize(int accountID, int userID, string context, string rightkey, string ip)
        {
            var sql = "UPDATE dbo.CRM_Account SET SyncHasInit=0";
            DBHelper.ExecuteSql(sql);
            return "CallSuccess:true";
        }
        //验证用户名是否存在
        public bool isExist(string comaccount, string useraccount, string password)
        {
            DataSet ds = null;
            bool isExists = false;
            string sql = string.Format(@"SELECT AU_ID,AU_UserAccount,AU_Name,R_ID,AU_Password FROM dbo.tb_AdminUser WHERE AU_Name={0} and AU_Password={1}", useraccount,password);
            ds = DBHelper.GetDsBySql(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                isExists = true;
                userid = int.Parse(ds.Tables[0].Rows[0]["AU_ID"].ToString());
                role_id = int.Parse(ds.Tables[0].Rows[0]["R_ID"].ToString());
                adminuseraccount = ds.Tables[0].Rows[0]["AU_UserAccount"].ToString();
                adminusername = ds.Tables[0].Rows[0]["AU_Name"].ToString();
            }
            return isExists;
        }

        public bool isExistS(string useraccount, string password)
        {

            bool isExists = false;

            string sql = string.Format(@"SELECT AU_ID,AU_UserAccount,AU_Name,R_ID,AU_Password FROM dbo.tb_AdminUser WHERE AU_UserAccount='{0}'", useraccount);
            SqlDataReader reader = DBHelper.GetDRSql(sql);
            if (reader.Read())
            {
                //c4ca4238a0b923820dcc509a6f75849b
                var pass = Convert.ToString(reader["AU_Password"]);
                var md5pass = Unite.ToMD5New(password);
                if (md5pass != pass)
                {
                    return false;
                }
                else
                {

                    isExists = true;
                    userid = int.Parse(reader["AU_ID"].ToString());
                    role_id = int.Parse(reader["R_ID"].ToString());
                    adminuseraccount = reader["AU_UserAccount"].ToString();
                    adminusername = reader["AU_Name"].ToString();
                }
            }
            reader.Close();
            //  GC.Collect();
            return isExists;
        }
        //获取权限数据

        public void refreshUserInfo(int subUserId)
        {
            string sql = "select CRM_User_Account.rec_no,crm_sub_user.user_name,CRM_Sub_User.Department_rec,CRM_Department.level_no as departmentlevelno,crm_role.level_no,crm_account.companyname,CRM_User_Account.useraccount,crm_account.rec_no as account_rec,crm_user_account.role_rec "
                + " from crm_account inner join CRM_User_Account on crm_account.rec_no=crm_user_account.account_rec inner join crm_sub_user on CRM_User_Account.rec_no=crm_sub_user.rec_no join crm_role on crm_user_account.role_rec=crm_role.rec_no left join crm_department on crm_sub_user.department_rec=crm_department.rec_no"
                + " where getdate() between isnull(crm_user_account.start_date,'1900-01-01') and isnull(crm_user_account.end_date,'2050-12-30') and CRM_User_Account.rec_no=" + subUserId;

            SqlDataReader reader = DBHelper.GetDRSql(sql);
            if (reader.Read())
            {
                userid = int.Parse(reader["AU_ID"].ToString());
                adminuseraccount = reader["AU_UserAccount"].ToString();
                role_id = int.Parse(reader["R_ID"].ToString());
                adminusername = reader["AU_Name"].ToString();
                //employeename = reader["user_name"].ToString();
                //department = reader["department_rec"].ToString();
                //rolelevel = reader["level_no"].ToString();
                //companyname = reader["companyname"].ToString();
                //departmentlevelno = SetUnit.ToString(reader["departmentlevelno"]);
            }
            reader.Close();
        }


        //
        public void AddLoginLog(ISessionObject session)
        {
            var sql = "INSERT INTO login_log(login_time,login_userid,login_ip,login_type) VALUES(@login_time,@login_userid,@login_ip,@login_type)";
            DBHelper.ExecuteSql(sql,
                new SqlParameter("@login_time", DateTime.Now),
                new SqlParameter("@login_userid", session.AU_ID),
                new SqlParameter("@login_ip", HttpContext.Current.Request.UserHostAddress),
                new SqlParameter("@login_type", 0)
                );
        }
    }
}
