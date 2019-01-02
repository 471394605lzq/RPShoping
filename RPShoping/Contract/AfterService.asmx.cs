using DAL;
using Model;
using Newtonsoft.Json.Linq;
using PublicLibrary;
using APICloud;
using Session_Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.IO;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Text;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Domain;
using Aop.Api.Response;
using System.Security.Cryptography;
using System.Web.UI;

namespace RPShoping.Contract
{
    /// <summary>
    /// AfterService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class AfterService : System.Web.Services.WebService
    {

        #region jqgrid加载数据列表时用到的方法
        public struct JQGridResults
        {
            public int page;
            public int total;
            public int records;
            public JQGridRow[] rows;

        }
        public struct JQGridRow
        {
            public int id;
            public string[] cell;
        }
        #endregion

        #region 根据角色获取后台模块菜单
        [WebMethod(EnableSession = true, Description = "根据角色获取后台模块菜单")]
        public string GetAfterModuleMenu(string role_id,string isparement)
        {
            string sql =string.Format(@"SELECT m.M_Name,m.M_EditUrl,m.isparement,m.parement_id,m.M_ID,m.imagecode FROM dbo.tb_Module m
                                        JOIN dbo.tb_RoleModule rm ON rm.module_id=m.M_ID
                                        JOIN dbo.tb_Role r ON r.R_ID=rm.role_id WHERE r.R_ID={0} AND m.isparement={1} ORDER BY reorder ASC ", role_id, isparement);
            string cmdsql = sql;
            DataTable dt = DBHelper.GetDataTable(cmdsql);
            var jsonData = Unite.ToJson(dt);
            //jsonData = "{ \"page\":\" 1\", \"total\":\" 10\", \"records\": \"10\", \"rows\": " + jsonData + "}";
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region 修改用户密码
        [WebMethod(EnableSession = true, Description = "修改用户密码")]
        public string UpdateUserPwd(string oldpwd, string newpwd, string userid)
        {
            string result = "0";//最终执行结果
            string upsql = "";
            try
            {

                string sql = string.Format(@"SELECT AU_ID,AU_UserAccount,AU_Name,R_ID,AU_Password FROM dbo.tb_AdminUser WHERE AU_ID='{0}'", userid);
                SqlDataReader reader = DBHelper.GetDRSql(sql);
                if (reader.Read())
                {
                    var pass = Convert.ToString(reader["AU_Password"]);
                    var md5pass = Unite.ToMD5New(oldpwd);
                    if (md5pass != pass)
                    {
                        result = "旧密码不正确！";
                    }
                    else
                    {
                        var newmd5pwd= Unite.ToMD5New(newpwd);
                        upsql =string.Format("UPDATE dbo.tb_AdminUser SET AU_Password='{0}' WHERE AU_ID={1}", newmd5pwd, userid);
                        var rsObj = DBHelper.ExecuteSql(upsql);//sql语句执行影响行数
                        if (rsObj <= 0)
                            result = "0";
                        else
                            result = rsObj.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region 获取后台用户列表
        [WebMethod(EnableSession = true, Description = "获取后台用户列表")]
        public string GetAfterUserList(string where)
        {
            //string accountname=SessionManager.GetSession(HttpContext.Current).AU_ID;
            int auid = Convert.ToInt32(HttpContext.Current.Session["au_id"]);//当前用户账号id
            string sql = @"SELECT AU_ID,rl.R_Name,AU_UserAccount,AU_Name,AU_Remark,rl.R_ID,AU_Password FROM dbo.tb_AdminUser
                           LEFT JOIN dbo.tb_Role rl ON rl.R_ID = dbo.tb_AdminUser.R_ID where AU_IsDelete=0";
            string cmdsql=sql + where;
            DataTable dt = DBHelper.GetDataTable(cmdsql);
            var jsonData = Unite.ToJson(dt);
            //jsonData = "{ \"page\":\" 1\", \"total\":\" 10\", \"records\": \"10\", \"rows\": " + jsonData + "}";
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region  保存后台账号信息
        [WebMethod(EnableSession = true, Description = "保存后台账号信息")]
        public string SaveAfterAccount(string context,string dbcmd,string au_id)
        {
            string result = "0";//最终执行结果
            try
            {
                string sql = "";//最终执行sql语句
                var jsonstrtemp = HttpUtility.UrlDecode(context);//将明细数据进行解码
                var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//转json格式
                                                                                                               //var detialdatalist = formatjsonfordetial.ToArray();//将json转换成数组
                var pwd = jsondataformain[0].SelectToken("pwd").ToString();
                var newmd5pwd = Unite.ToMD5New(pwd);
                //var formatjsonformain = DataConvert.FormatClinetContextForData(context);//格式化主档数据
                //var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(formatjsonformain) as JContainer;//将格式化后的字符串转换为json格式
                if (dbcmd == "add")
                {
                    sql = @"INSERT INTO dbo.tb_AdminUser( R_ID ,AU_UserAccount ,AU_Name ,AU_Password ,AU_Remark ,AU_IsDelete)
            VALUES({0},N'{1}', N'{2}',N'{3}',N'{4}',{5})";
                }
                else {
                    sql = @"UPDATE dbo.tb_AdminUser SET R_ID={0},AU_UserAccount='{1}',AU_Name='{2}',AU_Password='{3}',AU_Remark='{4}',AU_IsDelete={5} WHERE AU_ID=" + au_id;
                }
                string cmdsql = string.Format(sql,
            jsondataformain[0].SelectToken("roe_id").ToString(),
            jsondataformain[0].SelectToken("account_name").ToString(),
            jsondataformain[0].SelectToken("user_name").ToString(),
            newmd5pwd,
            jsondataformain[0].SelectToken("remark").ToString(),
                     0);
                var rsObj = DBHelper.ExecuteSql(cmdsql);//sql语句执行影响行数
                if (rsObj <= 0)
                    result = "0";
                else
                    result = rsObj.ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region 获取后台用户角色列表
        [WebMethod(EnableSession = true, Description = "获取后台用户角色列表")]
        public string GetAfterUserRoleList(string where)
        {
            string sql = @"SELECT R_ID,R_Name,R_Remark,R_Key FROM dbo.tb_Role";
            string cmdsql = sql + where;
            DataTable dt = DBHelper.GetDataTable(cmdsql);
            var jsonData = Unite.ToJson(dt);
            //jsonData = "{ \"page\":\" 1\", \"total\":\" 10\", \"records\": \"10\", \"rows\": " + jsonData + "}";
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region  保存后台角色信息
        [WebMethod(EnableSession = true, Description = "保存后台角色信息")]
        public string SaveAfterRole(string context, string dbcmd, string id)
        {
            string result = "0";//最终执行结果
            try
            {
                string sql = "";//最终执行sql语句
                var jsonstrtemp = HttpUtility.UrlDecode(context);//将明细数据进行解码
                var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//转json格式
                //var detialdatalist = formatjsonfordetial.ToArray();//将json转换成数组

                //var formatjsonformain = DataConvert.FormatClinetContextForData(context);//格式化主档数据
                //var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(formatjsonformain) as JContainer;//将格式化后的字符串转换为json格式
                if (dbcmd == "add")
                {
                    sql = @"INSERT INTO dbo.tb_Role( R_Name, R_Remark, R_Key )
                            VALUES  ( N'{0}',N'{1}', N'{2}')";
                }
                else
                {
                    sql = @"UPDATE dbo.tb_Role SET R_Name='{0}',R_Remark='{1}',R_Key='{2}' WHERE R_ID=" + id;
                }
                string cmdsql = string.Format(sql,
            jsondataformain[0].SelectToken("rolename").ToString(),
            jsondataformain[0].SelectToken("remark").ToString(),
            jsondataformain[0].SelectToken("rolekey").ToString()
                     );
                var rsObj = DBHelper.ExecuteSql(cmdsql);//sql语句执行影响行数
                if (rsObj <= 0)
                    result = "0";
                else
                    result = rsObj.ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region 获取后台用户权限列表
        [WebMethod(EnableSession = true, Description = "获取后台用户权限列表")]
        public string GetAfterUserRightList(string where)
        {
            string sql = @"SELECT right_id,right_code,right_name,right_remark,right_isdelete,CASE WHEN right_isdelete IS NULL or right_isdelete=1 THEN '无效' ELSE '有效'  END AS deletename,right_imagecode  FROM dbo.tb_Right ";
            string cmdsql = sql + where;
            DataTable dt = DBHelper.GetDataTable(cmdsql);
            var jsonData = Unite.ToJson(dt);
            //jsonData = "{ \"page\":\" 1\", \"total\":\" 10\", \"records\": \"10\", \"rows\": " + jsonData + "}";
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region  保存后台权限信息
        [WebMethod(EnableSession = true, Description = "保存后台权限信息")]
        public string SaveAfterRight(string context, string dbcmd, string id)
        {
            string result = "0";//最终执行结果
            try
            {
                string sql = "";//最终执行sql语句
                var jsonstrtemp = HttpUtility.UrlDecode(context);//将明细数据进行解码
                var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//转json格式
                //var detialdatalist = formatjsonfordetial.ToArray();//将json转换成数组

                //var formatjsonformain = DataConvert.FormatClinetContextForData(context);//格式化主档数据
                //var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(formatjsonformain) as JContainer;//将格式化后的字符串转换为json格式
                if (dbcmd == "add")
                {
                    sql = @"INSERT INTO dbo.tb_Right( right_code ,right_name ,right_remark,right_isdelete,right_imagecode)
VALUES  ( N'{0}' ,N'{1}' ,N'{2}' ,{3},'{4}')";
                }
                else
                {
                    sql = @"UPDATE dbo.tb_Right SET right_code='{0}',right_name='{1}',right_remark='{2}',right_isdelete={3},right_imagecode='{4}' WHERE right_id=" + id;
                }
                string cmdsql = string.Format(sql,
            jsondataformain[0].SelectToken("righrcode").ToString(),
            jsondataformain[0].SelectToken("rightname").ToString(),
            jsondataformain[0].SelectToken("remark").ToString(),
            jsondataformain[0].SelectToken("isdelete").ToString(),
            jsondataformain[0].SelectToken("right_imagecode").ToString()
                     );
                var rsObj = DBHelper.ExecuteSql(cmdsql);//sql语句执行影响行数
                if (rsObj <= 0)
                    result = "0";
                else
                    result = rsObj.ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region 获取后台模块列表
        [WebMethod(EnableSession = true, Description = "获取后台模块列表")]
        public string GetAfterModuleList(string where)
        {
            string sql = @"SELECT M_ID,M_Name,M_ModuleKey,M_Level_No,M_Remark,M_ShowState,CASE WHEN M_ShowState=0 or M_ShowState is null THEN '否' ELSE '是' end  AS showstatename,M_EditUrl,parement_id,isparement,
                           CASE WHEN isparement=0 or isparement is null THEN '否' ELSE '是' end  AS isparementname,imagecode,reorder FROM dbo.tb_Module ";
            string cmdsql = sql + where;
            DataTable dt = DBHelper.GetDataTable(cmdsql);
            var jsonData = Unite.ToJson(dt);
            //jsonData = "{ \"page\":\" 1\", \"total\":\" 10\", \"records\": \"10\", \"rows\": " + jsonData + "}";
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region 获取后台模块权限列表
        [WebMethod(EnableSession = true, Description = "获取后台模块权限列表")]
        public string GetAfterModuleRightList(string where)
        {
            string sql = @"SELECT tb_Right.right_id, right_code ,right_name ,right_remark ,right_isdelete   FROM dbo.tb_Right ";

            string cmdsql = sql + where;
            DataTable dt = DBHelper.GetDataTable(cmdsql);
            var jsonData = Unite.ToJson(dt);
            //jsonData = "{ \"page\":\" 1\", \"total\":\" 10\", \"records\": \"10\", \"rows\": " + jsonData + "}";
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region 获取后台模块权限列表
        [WebMethod(EnableSession = true, Description = "获取后台模块权限列表")]
        public string GetAfterModuleRight(string where)
        {
            string sql = @"SELECT * FROM dbo.tb_ModuleRight ";
            string cmdsql = sql + where;
            DataTable dt = DBHelper.GetDataTable(cmdsql);
            var jsonData = Unite.ToJson(dt);
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region 获取后台角色模块权限列表
        [WebMethod(EnableSession = true, Description = "获取后台模块权限列表")]
        public string GetAfterRoleModuleRight(string where)
        {
            string sql = @"SELECT tb_RoleModuleRight.*,r.right_name,r.right_imagecode,r.right_code FROM dbo.tb_RoleModuleRight
                            JOIN dbo.tb_Right r ON r.right_id=tb_RoleModuleRight.right_id  AND r.right_isdelete=0 
                            JOIN dbo.tb_ModuleRight mr ON mr.right_id=r.right_id AND mr.module_id=tb_RoleModuleRight.module_id";
            string cmdsql = sql + where;
            DataTable dt = DBHelper.GetDataTable(cmdsql);
            var jsonData = Unite.ToJson(dt);
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region 保存后台模块权限
        [WebMethod(EnableSession = true, Description = "保存后台模块权限")]
        public string SaveModuleRight(string rightid, string moduleid, string type)
        {
            string result = "0";//最终执行结果
            string sql = "";
            try
            {
                if (type == "0")
                {
                    sql = string.Format(@"INSERT  INTO dbo.tb_ModuleRight(module_id, right_id) VALUES({0},{1})", moduleid, rightid);//最终执行sql语句
                }
                else
                {
                    sql = string.Format(@"delete from  dbo.tb_ModuleRight where module_id={0} and right_id={1}", moduleid, rightid);//最终执行sql语句
                }
                var rsObj = DBHelper.ExecuteSql(sql);//sql语句执行影响行数
                if (rsObj <= 0)
                    result = "0";
                else
                    result = rsObj.ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region 保存后台角色模块权限
        [WebMethod(EnableSession = true, Description = "保存后台模块权限")]
        public string SaveRoleModuleRight(string rightid, string moduleid, string type,string role_id)
        {
            string result = "0";//最终执行结果
            string sql = "";
            try
            {
                if (type == "0")
                {
                    sql = string.Format(@"INSERT  INTO dbo.tb_RoleModuleRight(module_id, right_id,role_id) VALUES({0},{1},{2})", moduleid, rightid, role_id);//最终执行sql语句
                }
                else
                {
                    sql = string.Format(@"delete from  dbo.tb_RoleModuleRight where module_id={0} and right_id={1} and role_id={2}", moduleid, rightid, role_id);//最终执行sql语句
                }
                var rsObj = DBHelper.ExecuteSql(sql);//sql语句执行影响行数
                if (rsObj <= 0)
                    result = "0";
                else
                    result = rsObj.ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region 获取后台角色模块列表
        [WebMethod(EnableSession = true, Description = "获取后台角色模块列表")]
        public string GetAfterRoleModuleList(string where)
        {
            string sql = @"SELECT M_ID,M_Name,M_ModuleKey,M_Level_No,M_Remark,M_ShowState,CASE WHEN M_ShowState=0 or M_ShowState is null THEN '否' ELSE '是' end  AS showstatename,M_EditUrl,parement_id,isparement,
                           CASE WHEN isparement=0 or isparement is null THEN '否' ELSE '是' end  AS isparementname,rm.role_id FROM dbo.tb_Module LEFT JOIN dbo.tb_RoleModule rm ON rm.module_id=tb_Module.M_ID";
            string cmdsql = sql + where;
            DataTable dt = DBHelper.GetDataTable(cmdsql);
            var jsonData = Unite.ToJson(dt);
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region 保存后台角色模块
        [WebMethod(EnableSession = true, Description = "保存后台角色模块")]
        public string SaveRoleModule(string roleid, string moduleid,string type)
        {
            string result = "0";//最终执行结果
            string sql = "";
            try
            {
                if (type == "0")
                {
                    sql = string.Format(@"INSERT  INTO dbo.tb_RoleModule(role_id, module_id) VALUES({0},{1})", roleid, moduleid);//最终执行sql语句
                }
                else {
                    sql = string.Format(@"delete from  dbo.tb_RoleModule where role_id={0} and module_id={1}", roleid, moduleid);//最终执行sql语句
                }
                var rsObj = DBHelper.ExecuteSql(sql);//sql语句执行影响行数
                if (rsObj <= 0)
                    result = "0";
                else
                    result = rsObj.ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region  保存后台模块信息
        [WebMethod(EnableSession = true, Description = "保存后台模块信息")]
        public string SaveAfterModule(string context, string dbcmd, string id)
        {
            string result = "0";//最终执行结果
            try
            {
                string sql = "";//最终执行sql语句
                var jsonstrtemp = HttpUtility.UrlDecode(context);//将明细数据进行解码
                var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//转json格式
                //var detialdatalist = formatjsonfordetial.ToArray();//将json转换成数组

                //var formatjsonformain = DataConvert.FormatClinetContextForData(context);//格式化主档数据
                //var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(formatjsonformain) as JContainer;//将格式化后的字符串转换为json格式
                if (dbcmd == "add")
                {
                    sql = @"INSERT INTO dbo.tb_Module( M_Name ,M_ModuleKey ,M_Level_No ,M_Remark ,M_ShowState,M_EditUrl,parement_id,isparement,imagecode,reorder)
VALUES  ( N'{0}' ,N'{1}' ,N'{2}' ,N'{3}' ,{4},'{5}',{6},{7},'{8}',{9})";
                }
                else
                {
                    sql = @"UPDATE dbo.tb_Module SET M_Name='{0}',M_ModuleKey='{1}',M_Level_No='{2}',M_Remark='{3}',M_ShowState={4},M_EditUrl='{5}',parement_id={6},isparement={7},imagecode='{8}',reorder={9} WHERE M_ID=" + id;
                }
                string cmdsql = string.Format(sql,
            jsondataformain[0].SelectToken("module_name").ToString(),
            jsondataformain[0].SelectToken("module_key").ToString(),
            jsondataformain[0].SelectToken("module_code").ToString(),
            jsondataformain[0].SelectToken("module_remark").ToString(),
            jsondataformain[0].SelectToken("showstate").ToString(),
            jsondataformain[0].SelectToken("editurl").ToString(),
            jsondataformain[0].SelectToken("parement_id").ToString()==""?"0": jsondataformain[0].SelectToken("parement_id").ToString(),
            jsondataformain[0].SelectToken("isparemnt").ToString(),
            jsondataformain[0].SelectToken("imagecode").ToString(),
            jsondataformain[0].SelectToken("reorder").ToString()
                     );
                var rsObj = DBHelper.ExecuteSql(cmdsql);//sql语句执行影响行数
                if (rsObj <= 0)
                    result = "0";
                else
                    result = rsObj.ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region 将本地area表中的数据读取出来以json格式生成txt文件保存到本地
        [WebMethod(EnableSession = true, Description = "生成区域txt文本文件")]
        public string SetAreaToTxt()
        {
            string result = "0";//最终执行结果
            string jsonstr = "";
            try
            {
                string sql = "SELECT * FROM dbo.Area WHERE Level=1";//将area表中省份数据查询出来

                DataTable dt = DBHelper.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        jsonstr = jsonstr + "{\"name\":\"" + dt.Rows[i]["name"].ToString() + "\"";
                        string getsql = "SELECT *FROM dbo.Area WHERE ParentID=" + dt.Rows[i]["AreaID"];
                        DataTable dt2 = DBHelper.GetDataTable(getsql);
                        if (dt2.Rows.Count > 0)
                        {
                            jsonstr = jsonstr + ",\"sub\":[";
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                jsonstr = jsonstr + "{\"name\":\"" + dt2.Rows[j]["name"].ToString() + "\"";
                                string getsql2 = "SELECT *FROM dbo.Area WHERE ParentID=" + dt2.Rows[j]["AreaID"];
                                DataTable dt3 = DBHelper.GetDataTable(getsql2);
                                if (dt3.Rows.Count > 0)
                                {
                                    jsonstr = jsonstr + ",\"sub\":[";
                                    for (int h = 0; h < dt3.Rows.Count; h++)
                                    {
                                        if (h == dt3.Rows.Count - 1)
                                        {
                                            if (j == dt2.Rows.Count - 1)
                                            {
                                                jsonstr = jsonstr + "{\"name\":\"" + dt3.Rows[h]["name"].ToString() + "\"}]}]},";
                                            }
                                            else
                                            {
                                                jsonstr = jsonstr + "{\"name\":\"" + dt3.Rows[h]["name"].ToString() + "\"}]},";
                                            }
                                            
                                        }
                                        else
                                        {
                                            jsonstr = jsonstr + "{\"name\":\"" + dt3.Rows[h]["name"].ToString() + "\"},";
                                            //if (j == dt2.Rows.Count - 1)
                                            //{
                                            //    jsonstr = jsonstr + "{\"name\":\"" + dt3.Rows[h]["name"].ToString() + "\"},";
                                            //}
                                            //else
                                            //{
                                            //    jsonstr = jsonstr + "{\"name\":\"" + dt3.Rows[h]["name"].ToString() + "\"}]}],";
                                            //}
                                        }
                                    }
                                }
                                else
                                {
                                    if (j == dt2.Rows.Count - 1)
                                    {
                                        jsonstr = jsonstr + "}";
                                    }
                                    else
                                    {
                                        jsonstr = jsonstr + "},";
                                    }
                                }
                            }
                        }
                        else {
                            if (i == dt.Rows.Count - 1)
                            {
                                jsonstr = jsonstr + "}";
                            }
                            else
                            {
                                jsonstr = jsonstr + "},";
                            }
                        }
                        
                    }
                }
                jsonstr = "[" + jsonstr + "]";
                FileStream fs1= new FileStream("F:\\TestTxt.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.Write(jsonstr);
                sw.Flush();
                sw.Close();
                result = "1";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region apicloud数据操作
        #region  将本地数据库area表的数据移到云数据库上的area表中
        [WebMethod(EnableSession = true, Description = "将本地数据库area表的数据移到云数据库上的area表中")]
        public string MoveAreaInfo()
        {
            string result = "0";//最终执行结果
            try
            {
                string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
                string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
                var resouce = new Resource(appid, appkey);
                var model = resouce.Factory("Area");

                string sql = "SELECT * FROM dbo.Area";//将area表的所有信息查询出来
                DataTable dt = DBHelper.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var ret = model.Create(new
                        {
                            AreaID = dt.Rows[i]["AreaID"].ToString(),
                            Name = dt.Rows[i]["Name"].ToString(),
                            ParentID = dt.Rows[i]["ParentID"].ToString(),
                            AreaCode = dt.Rows[i]["AreaCode"].ToString(),
                            IsUse = dt.Rows[i]["IsUse"].ToString(),
                            Level = dt.Rows[i]["Level"].ToString()
                        });
                    }
                    result = "1";
                }

            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion
        #region  设置产品期数
        [WebMethod(EnableSession = true, Description = "设置产品期数")]
        public string SetProductIssue(string id,string productprice, int number,string productname, string productsort,string type)
        {
            string result = "0";//最终执行结果
            try
            {
                string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
                string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
                var resouce = new Resource(appid, appkey);
                var model = resouce.Factory("tb_Issue");//要查询的云数据表

                string filter1 = "{\"where\":{ \"P_ID\":\"" + id + "\",\"I_State\":\"进行中\"}}";
                var ret1 = model.Query(filter1);//进行中期数查询结果
                var jsonstrtemp1 = HttpUtility.UrlDecode(ret1);
                var jsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp1) as JContainer;//查询的结果转json格式
                if (jsondataformain1.Count <= 0)
                {
                    //新增商品期数之前先查询出期数最大的编号
                    string filter = "{\"where\":{ \"P_ID\":\"" + id + "\"},\"order\":\"createdAT DESC\",\"limit\":1}";
                    //var temp1 = HttpUtility.UrlDecode(filter);//将条件进行编码
                    //var tempfilter = Newtonsoft.Json.JsonConvert.DeserializeObject(temp1) as JContainer;//转条件json格式

                    var ret = model.Query(filter);//然会查询结果
                    var jsonstrtemp = HttpUtility.UrlDecode(ret);
                    var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//查询的结果转json格式
                    int maxcode = 0;
                    int newmaxcode = 0;
                    if (jsondataformain.Count > 0)
                    {
                        maxcode = Convert.ToInt32(jsondataformain[0].SelectToken("I_IssueNumber").ToString());//获取最大的商品期数编号
                    }
                    if (maxcode > 0)
                    {
                        newmaxcode = maxcode;
                    }
                    int tempsurplusnumber = Convert.ToInt32(productprice) / Convert.ToInt32(type);
                    for (int i = 0; i < number; i++)
                    {
                        var rets = model.Create(new
                        {
                            P_ID = id,
                            I_IssueNumber = newmaxcode + (i + 1),
                            I_State = "进行中",
                            sendmessage = "否",
                            I_AnnouncedTime = "",
                            AlreadyNumber = 0,
                            SurplusNumber = tempsurplusnumber,
                            productname = productname.Trim(),
                            productprice = Convert.ToInt32(productprice),
                            productsort = productsort.Trim(),
                            P_Type= type
                        });
                    }
                    result = "1";
                }
                else
                {
                    result = "2";//该产品已有进行中期数
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region  根据产品id删除状态为未开始的产品期数
        [WebMethod(EnableSession = true, Description = "根据产品id删除状态为未开始的产品期数")]
        public string DeleteIssueByProduct(string productid)
        {
            string result = "0";//最终执行结果 
            try
            {
                string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
                string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
                var resouce = new Resource(appid, appkey);
                var model = resouce.Factory("tb_Issue");//要查询的云数据表

                string filter1 = "{\"where\":{ \"P_ID\":\"" + productid.Trim() + "\",\"I_State\":\"未开始\"}}";
                var ret1 = model.Query(filter1);//未开始期数查询结果
                var jsonstrtemp1 = HttpUtility.UrlDecode(ret1);
                var jsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp1) as JContainer;//查询的结果转json格式

                if (jsondataformain1.Count > 0)
                {
                    for (int i = 0; i < jsondataformain1.Count; i++)
                    {
                        var ret = model.Delete(jsondataformain1[0].SelectToken("id").ToString());
                    }
                    result = "1";
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region  根据产品id获取产品期数
        [WebMethod(EnableSession = true, Description = "根据产品id获取产品期数")]
        public string getissuebyproductid(string productid)
        {
            string result = "0";//最终执行结果 
            try
            {
                string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
                string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
                var resouce = new Resource(appid, appkey);
                var model = resouce.Factory("tb_Issue");//要查询的云数据表

                string filter1 = "{\"where\":{ \"P_ID\":\"" + productid.Trim() + "\",\"I_State\":\"进行中\"}}";
                var ret1 = model.Query(filter1);//未开始期数查询结果
                var jsonstrtemp1 = HttpUtility.UrlDecode(ret1);
                var jsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp1) as JContainer;//查询的结果转json格式

                string filter2 = "{\"where\":{ \"P_ID\":\"" + productid.Trim() + "\",\"I_State\":\"即将揭晓\"}}";
                var ret2 = model.Query(filter2);//未开始期数查询结果
                var jsonstrtemp2 = HttpUtility.UrlDecode(ret2);
                var jsondataformain2 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp2) as JContainer;//查询的结果转json格式

                string filter3 = "{\"where\":{ \"P_ID\":\"" + productid.Trim() + "\",\"I_State\":\"已揭晓\"}}";
                var ret3 = model.Query(filter3);//未开始期数查询结果
                var jsonstrtemp3 = HttpUtility.UrlDecode(ret3);
                var jsondataformain3 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp3) as JContainer;//查询的结果转json格式


                if (jsondataformain1.Count > 0|| jsondataformain2.Count>0|| jsondataformain3.Count>0)
                {
                    result = "1";
                }
                else {
                    result = "0";
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion
        #region  根据产品id修改产品上架状态
        [WebMethod(EnableSession = true, Description = "根据产品id修改产品上架状态")]
        public string updatestatebyid(string productid,string state)
        {
            string result = "0";//最终执行结果 
            try
            {
                string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
                string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
                var resouce = new Resource(appid, appkey);
                var model = resouce.Factory("tb_Product");//要查询的云数据表

                var resultstr=model.Edit(productid.Trim(),new { p_state= state });

                var jsonstrtemp = HttpUtility.UrlDecode("["+resultstr+"]");
                var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//查询的结果转json格式
                string resultid = jsondataformain[0].SelectToken("id").ToString();
                if (resultid!=""&& resultid!=null)
                {
                    result = "1";
                }
                else
                {
                    result = "0";
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion


        #region 获取产品信息列表
        [WebMethod(EnableSession = true, Description = "获取产品信息列表")]
        public string GetProductList(string where)
        {
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
            var resouce = new Resource(appid, appkey);
            var model = resouce.Factory("tb_Product");//要查询的云数据表

            //string filter1 = "{\"where\":{ \"P_ID\":\"" + productid.Trim() + "\",\"I_State\":\"进行中\"}}";
            string filter1 = "{\"limit\":5000,\"order\": \"createdAt DESC\"}";
            var ret1 = model.Query(filter1);//未开始期数查询结果
            var jsonData = ret1;// HttpUtility.UrlDecode(ret1);
            //var jsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp1) as JContainer;//查询的结果转json格式
            //jsonData = "{ \"page\":\" 1\", \"total\":\" 10\", \"records\": \"10\", \"rows\": " + jsonData + "}";
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region 获取客户列表
        [WebMethod(EnableSession = true, Description = "获取客户列表")]
        public string GetUserList(string where)
        {
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
            var resouce = new Resource(appid, appkey);
            var model = resouce.Factory("user");//要查询的云数据表

            string filter1 = "{\"where\":{ \"issystem\":\"是\"},\"limit\":500,\"order\": \"createdAt DESC\"}";
            var ret1 = model.Query(filter1);//未开始期数查询结果
            var jsonData = HttpUtility.UrlDecode(ret1);
            //var jsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp1) as JContainer;//查询的结果转json格式
            //jsonData = "{ \"page\":\" 1\", \"total\":\" 10\", \"records\": \"10\", \"rows\": " + jsonData + "}";
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion

        #region 获取二维码分享列表
        [WebMethod(EnableSession = true, Description = "获取二维码分享列表")]
        public string GetQRCodeList(string where)
        {
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
            var resouce = new Resource(appid, appkey);
            var model = resouce.Factory("tb_QRCode");//要查询的云数据表

            //string filter1 = "{\"where\":{ \"P_ID\":\"" + productid.Trim() + "\",\"I_State\":\"进行中\"}}";
            var ret1 = model.Query();//未开始期数查询结果
            var jsonData = HttpUtility.UrlDecode(ret1);
            //var jsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp1) as JContainer;//查询的结果转json格式
            //jsonData = "{ \"page\":\" 1\", \"total\":\" 10\", \"records\": \"10\", \"rows\": " + jsonData + "}";
            jsonData = "{\"rows\": " + jsonData + "}";
            return jsonData;
        }
        #endregion



        #region 随机分配产品期数中奖号码
        [WebMethod(EnableSession = true, Description = "随机分配产品期数中奖号码")]
        private string SetIssueLuckyCode(string i_id,int countnumber,string productname,string issuenumber,string introduceuserid,string oldrewardmoney,int productprice)
        {
            string result = "0"; //"[{ \"result\":\" 0\"}]";//最终执行结果 
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
            var resouce = new Resource(appid, appkey);
            var model = resouce.Factory("tb_OrderDetail");//要查询的云数据表 tb_OrderDetail
            var model1 = resouce.Factory("tb_Issue");
            var model2= resouce.Factory("user");
            var rewarddetailmodel = resouce.Factory("tb_rewarddetail");

            string filterissue = "{\"where\":{ \"id\":\"" + i_id + "\"}}";
            var issueret = model1.Query(filterissue);//已购买完的产品期数数据
            var issuejsonData = HttpUtility.UrlDecode(issueret);
            var issuejsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(issuejsonData) as JContainer;//查询的结果转json格式

            //如果介绍人id不为空
            if (!string.IsNullOrWhiteSpace(introduceuserid))
            {
                int orderrewardmoney = 0;
                if (productprice > 500)
                {
                    orderrewardmoney = 100;
                }
                else if (productprice < 500 && productprice > 300)
                {
                    orderrewardmoney = 50;
                }
                else
                {
                    orderrewardmoney = 10;
                }
                decimal tempwardmoney = Convert.ToDecimal(oldrewardmoney) + orderrewardmoney;
                //var result1 = model2.Edit(introduceuserid.Trim(), new { rewardmoney = tempwardmoney });//
                string jsdatastr = "{ \"$inc\": { \"rewardmoney\":" + tempwardmoney + "}}";
                var result1 = model2.Edit(introduceuserid.Trim(), jsdatastr);//
                var rets = rewarddetailmodel.Create(new
                {
                    U_ID = introduceuserid.Trim(),
                    rewardmoney = orderrewardmoney,
                    isforward = "否"
                });
            }


            string luckuid = issuejsondataformain[0].SelectToken("LuckU_ID").ToString();
            //如果获奖者id为空则计算获奖者
            if (string.IsNullOrWhiteSpace(luckuid))
            {
                string filter1 = "{\"where\":{ \"I_ID\":\"" + i_id + "\"}}";
                var ret1 = model.Query(filter1);//已购买完的产品期数数据
                var jsonData = HttpUtility.UrlDecode(ret1);
                var jsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData) as JContainer;//查询的结果转json格式
                                                                                                             //Random r = new Random((int)DateTime.Now.Ticks);
                                                                                                             //long tick = DateTime.Now.Ticks;
                                                                                                             //Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
                string tempcodestr = "";
                DataTable dt = new DataTable();
                dt.Columns.Add("i_id", typeof(string));
                dt.Columns.Add("code", typeof(string));
                dt.Columns.Add("u_id", typeof(string));
                dt.Columns.Add("buynumber", typeof(int));

                if (jsondataformain1.Count > 0)
                {
                    for (int i = 0; i < jsondataformain1.Count; i++)
                    {
                        if (i == jsondataformain1.Count - 1)
                        {
                            tempcodestr = tempcodestr + jsondataformain1[i].SelectToken("code").ToString();
                        }
                        else
                        {
                            tempcodestr = tempcodestr + jsondataformain1[i].SelectToken("code").ToString() + ",";
                        }
                        DataRow row = dt.NewRow();
                        row["i_id"] = jsondataformain1[i].SelectToken("I_ID").ToString();
                        row["code"] = jsondataformain1[i].SelectToken("code").ToString();
                        row["buynumber"] = jsondataformain1[i].SelectToken("buynumber").ToString();
                        row["u_id"] = jsondataformain1[i].SelectToken("U_ID").ToString();
                        dt.Rows.Add(row);
                    }
                }
                Random r = new Random(Guid.NewGuid().GetHashCode());
                //int minnumber = 10000001;
                //int maxnumber = 10000000 + countnumber;
                string[] liststr = tempcodestr.Split(',');
                int luckynumber = r.Next(0, liststr.Length);
                string luckycode = liststr[luckynumber].ToString();
                //int luckycode = r.Next(minnumber, maxnumber);
                //dt = Newtonsoft.Json.JsonConvert.DeserializeObject(jsondataformain1, typeof(DataTable)) as DataTable;
                DataRow[] getrow = dt.Select("code like '%" + luckycode.ToString() + "%'");
                DataRow[] numberrow = dt.Select("i_id ='" + i_id.ToString() + "' and u_id='" + getrow[0]["u_id"].ToString() + "'");
                int resultbuynumber = 0;
                if (numberrow.Length > 0)
                {
                    for (int i = 0; i < numberrow.Length; i++)
                    {
                        resultbuynumber = resultbuynumber + Convert.ToInt32(numberrow[i]["buynumber"].ToString());
                    }
                }
                if (getrow.Length > 0)
                {
                    string luckyuid = getrow[0]["u_id"].ToString();
                    string luckycodestr = luckycode.ToString();
                    string announcedtime = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
                    string filter2 = "{\"where\":{ \"id\":\"" + luckyuid + "\"}}";
                    var ret2 = model2.Query(filter2);//中奖用户的昵称
                    var jsonData2 = HttpUtility.UrlDecode(ret2);
                    var jsondataformain2 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData2) as JContainer;//查询的结果转json格式
                    string usname = jsondataformain2[0].SelectToken("nickname").ToString();
                    var resultstr = model1.Edit(i_id.Trim(), new { I_AnnouncedTime = announcedtime, LuckCode = luckycodestr, luckybuynumber = resultbuynumber, LuckU_ID = luckyuid, I_State = "已揭晓", ShippingState = "待确认信息" });//

                    
                    string str=SendMessageToUser("中奖", luckyuid, usname, productname, issuenumber);//给中奖用户发送消息
                    var jsonstrtemp = HttpUtility.UrlDecode("[" + resultstr + "]");
                    var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//查询的结果转json格式
                    string resultid = jsondataformain[0].SelectToken("id").ToString();
                    if (resultid != "" && resultid != null)
                    {
                        result = "1";//"[{ \"result\":\" 1\"}]";
                    }
                    else
                    {
                        result = "0";// "[{ \"result\":\" 0\"}]";
                    }
                }
            }
            else {
                string announcedtime = DateTime.Now.AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss");
                string filter12 = "{\"where\":{ \"I_ID\":\"" + i_id + "\",\"U_ID\":\"" + luckuid + "\"}}";
                var ret12 = model.Query(filter12);//已购买完的产品期数数据
                var jsonData12 = HttpUtility.UrlDecode(ret12);
                var jsondataformain12 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData12) as JContainer;//查询的结果转json格式
                DataTable dt = new DataTable();
                dt.Columns.Add("i_id", typeof(string));
                dt.Columns.Add("code", typeof(string));
                dt.Columns.Add("u_id", typeof(string));
                dt.Columns.Add("buynumber", typeof(int));

                string codestr = "";
                string luckycode = "";
                if (jsondataformain12.Count > 0)
                {
                    for (int i = 0; i < jsondataformain12.Count; i++)
                    {
                        if (i == jsondataformain12.Count - 1)
                        {
                            codestr = codestr + jsondataformain12[i].SelectToken("code").ToString();
                        }
                        else
                        {
                            codestr = codestr + jsondataformain12[i].SelectToken("code").ToString() + ",";
                        }
                        DataRow row = dt.NewRow();
                        row["i_id"] = jsondataformain12[i].SelectToken("I_ID").ToString();
                        row["code"] = jsondataformain12[i].SelectToken("code").ToString();
                        row["buynumber"] = jsondataformain12[i].SelectToken("buynumber").ToString();
                        row["u_id"] = jsondataformain12[i].SelectToken("U_ID").ToString();
                        dt.Rows.Add(row);
                    }
                    Random r = new Random(Guid.NewGuid().GetHashCode());
                    string[] liststr = codestr.Split(',');
                    int luckynumber = r.Next(0, liststr.Length);
                    luckycode = liststr[luckynumber].ToString();
                }
                //dt = Newtonsoft.Json.JsonConvert.DeserializeObject(jsondataformain1, typeof(DataTable)) as DataTable;
                //DataRow[] getrow = dt.Select("code like '%" + luckycode.ToString() + "%'");
                DataRow[] numberrow = dt.Select("i_id ='" + i_id.ToString() + "' and u_id='" + luckuid + "'");
                int resultbuynumber = 0;
                if (numberrow.Length > 0)
                {
                    for (int i = 0; i < numberrow.Length; i++)
                    {
                        resultbuynumber = resultbuynumber + Convert.ToInt32(numberrow[i]["buynumber"].ToString());
                    }
                }
                var resultstr = model1.Edit(i_id.Trim(), new { I_AnnouncedTime = announcedtime, LuckCode = luckycode, luckybuynumber = resultbuynumber, LuckU_ID = luckuid, I_State = "已揭晓", ShippingState = "待确认信息" });
                string filter2 = "{\"where\":{ \"id\":\"" + luckuid + "\"}}";
                var ret2 = model2.Query(filter2);//中奖用户的昵称
                var jsonData2 = HttpUtility.UrlDecode(ret2);
                var jsondataformain2 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData2) as JContainer;//查询的结果转json格式
                string usname = jsondataformain2[0].SelectToken("nickname").ToString();
                string str = SendMessageToUser("中奖", luckuid, usname, productname, issuenumber);//给中奖用户发送消息
                result = "1";// "[{ \"result\":\" 1\"}]";
            }
            //var jsonresult = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as JContainer;//查询的结果转json格式
            //Context.Response.Charset = "UTF-8"; //设置字符集类型  
            //Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            //Context.Response.Write(jsonresult);
            //Context.Response.End();
            return result;
        }
        #endregion

        //给用户发送消息
        private string SendMessageToUser(string type, string uid,string usname,string productname,string qishu)
        {
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
            var resouce = new Resource(appid, appkey);
            string mi_title = "";
            string mi_content = "";
            var model1 = resouce.Factory("tb_MyInformation");
            if (type.Equals("中奖"))
            {
                mi_title = "中奖通知！";
                mi_content = "亲爱的：'"+ usname+"',恭喜您已成为'"+productname+"'第'"+ qishu + "期'的幸运儿！请到'个人中心'下的'获得商品'菜单下确认配送信息！以便我们尽快为您安排商品配送！";
            }
            var rets = model1.Create(new
            {
                MI_Title = mi_title,
                MI_IsRead = "否",
                MI_Content = mi_content,
                U_ID = uid
            });
            return "";
        }
        #region 修改产品期数数据旧版
        [WebMethod(EnableSession = true, Description = "修改产品期数数据")]
        public void UpdateIssueState(string i_id, string p_id,int alreadynumber, int surplusnumber,int issuenumber,int countnumber,string productname, string introduceuserid,string oldrewardmoney)
        {
            string result = "[{ \"result\":\" 0\"}]";//最终执行结果 
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
            var resouce = new Resource(appid, appkey);
            var model = resouce.Factory("tb_Product");//要查询的云数据表 tb_OrderDetail
            var model1 = resouce.Factory("tb_Issue");

            string productfilter1 = "{\"where\":{ \"id\":\"" + p_id + "\"}}";
            var productret1 = model.Query(productfilter1);//已购买完的产品数据
            var productjsonData = productret1;// HttpUtility.UrlDecode(productret1);
            var productjsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(productjsonData) as JContainer;//查询的结果转json格式
            int price = 0;
            if (productjsondataformain1.Count > 0)
            {
                if (productjsondataformain1[0].SelectToken("p_state").ToString().Equals("上架"))
                {
                    string type = productjsondataformain1[0].SelectToken("P_Type").ToString();
                    price = Convert.ToInt32(productjsondataformain1[0].SelectToken("P_Price").ToString());
                    int tempsurplusnumber =  price / Convert.ToInt32(type);
                    var rets = model1.Create(new
                    {
                        P_ID = p_id,
                        I_IssueNumber = issuenumber + 1,
                        I_State = "进行中",
                        sendmessage = "否",
                        I_AnnouncedTime = "",
                        AlreadyNumber = 0,
                        SurplusNumber = tempsurplusnumber,
                        productname = productjsondataformain1[0].SelectToken("P_Name").ToString(),
                        productprice = price,
                        productsort = productjsondataformain1[0].SelectToken("PS_ID").ToString(),
                        P_Type = type
                    });
                }
            }
            string eexpecttime = DateTime.Now.AddMinutes(2).ToString();//EexpectTime= eexpecttime
            var resultstr = model1.Edit(i_id.Trim(), new { AlreadyNumber= countnumber, SurplusNumber= 0, I_State = "即将揭晓", EexpectTime = eexpecttime });
            var jsonstrtemp = HttpUtility.UrlDecode("[" + resultstr + "]");
            var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//查询的结果转json格式
            string resultid = jsondataformain[0].SelectToken("id").ToString();
            if (resultid != "" && resultid != null)
            {
                //将即将揭晓的产品期数id通过极光推送推送到app上并更新即将揭晓的列表数据
                //string pushstr = "{\"i_id\":\""+ i_id + "\"}";
                //var tempstr = Newtonsoft.Json.JsonConvert.DeserializeObject(pushstr) as JContainer;
                //string s = tempstr.ToString();
                jpush jsp = new jpush();
                jsp.ExecutePushExample("亲 又有商品开奖了，赶快去看看是谁中奖吧！", i_id);
                string luckstr= SetIssueLuckyCode(i_id, countnumber, productname, issuenumber.ToString(), introduceuserid, oldrewardmoney, price);
                result = "[{ \"result\":\" 1\"}]";
            }
            else
            {
                result = "[{ \"result\":\" 0\"}]";
            }
            var jsonresult = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as JContainer;//查询的结果转json格式
            Context.Response.Charset = "UTF-8"; //设置字符集类型  
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Context.Response.Write(jsonresult);
            Context.Response.End();
        }
        #endregion


        #region 修改产品期数数据
        [WebMethod(EnableSession = true, Description = "修改产品期数数据")]
        private string UpdateIssueStatereturn(string i_id, string p_id, int alreadynumber, int surplusnumber, int issuenumber, int countnumber, string productname, string introduceuserid, string oldrewardmoney)
        {
            //string result = "[{ \"result\":\" 0\"}]";//最终执行结果 
            string result = "0";
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
            var resouce = new Resource(appid, appkey);
            var model = resouce.Factory("tb_Product");//要查询的云数据表 tb_OrderDetail
            var model1 = resouce.Factory("tb_Issue");

            string productfilter1 = "{\"where\":{ \"id\":\"" + p_id + "\"}}";
            var productret1 = model.Query(productfilter1);//已购买完的产品数据
            var productjsonData = productret1;// HttpUtility.UrlDecode(productret1);
            var productjsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(productjsonData) as JContainer;//查询的结果转json格式
            int price = 0;
            if (productjsondataformain1.Count > 0)
            {
                if (productjsondataformain1[0].SelectToken("p_state").ToString().Equals("上架"))
                {
                    string type = productjsondataformain1[0].SelectToken("P_Type").ToString();
                    price = Convert.ToInt32(productjsondataformain1[0].SelectToken("P_Price").ToString());
                    int tempsurplusnumber = price / Convert.ToInt32(type);
                    var rets = model1.Create(new
                    {
                        P_ID = p_id,
                        I_IssueNumber = issuenumber + 1,
                        I_State = "进行中",
                        sendmessage = "否",
                        I_AnnouncedTime = "",
                        AlreadyNumber = 0,
                        SurplusNumber = tempsurplusnumber,
                        productname = productjsondataformain1[0].SelectToken("P_Name").ToString(),
                        productprice = price,
                        productsort = productjsondataformain1[0].SelectToken("PS_ID").ToString(),
                        P_Type = type
                    });
                }
            }
            string eexpecttime = DateTime.Now.AddMinutes(2).ToString();//EexpectTime= eexpecttime
            var resultstr = model1.Edit(i_id.Trim(), new { AlreadyNumber = countnumber, SurplusNumber = 0, I_State = "即将揭晓", EexpectTime = eexpecttime });
            var jsonstrtemp = HttpUtility.UrlDecode("[" + resultstr + "]");
            var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//查询的结果转json格式
            string resultid = jsondataformain[0].SelectToken("id").ToString();
            if (resultid != "" && resultid != null)
            {
                //将即将揭晓的产品期数id通过极光推送推送到app上并更新即将揭晓的列表数据
                //string pushstr = "{\"i_id\":\""+ i_id + "\"}";
                //var tempstr = Newtonsoft.Json.JsonConvert.DeserializeObject(pushstr) as JContainer;
                //string s = tempstr.ToString();
                jpush jsp = new jpush();
                jsp.ExecutePushExample("亲 又有商品开奖了，赶快去看看是谁中奖吧！", i_id);
                SetIssueLuckyCode(i_id, countnumber, productname, issuenumber.ToString(), introduceuserid, oldrewardmoney, price);
                //result = "[{ \"result\":\" 1\"}]";
                result = "1";
            }
            else
            {
                //result = "[{ \"result\":\" 0\"}]";
                result = "0";
            }
            //var jsonresult = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as JContainer;//查询的结果转json格式
            //Context.Response.Charset = "UTF-8"; //设置字符集类型  
            //Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            //Context.Response.Write(jsonresult);
            //Context.Response.End();
            return result;
        }
        #endregion

        #region 保存订单详情信息
        [WebMethod(EnableSession = true, Description = "保存订单详情信息")]
        public void saveorderdetail(string pstr,string paytype, string i_id,string uid, string pid, int br,int by, int issuenumber, int countnumber, int onlynumber, int AlreadyNumber,int SurplusNumber, string productname, string introduceuserid, string oldrewardmoney)
        {
            string result = "[{ \"result\":\"0\",\"oid\":\"0\"}]";//最终执行结果 
            string m5str = "逸趣rp" + paytype+ i_id.ToString() + countnumber.ToString() + uid + pid + br + by + issuenumber.ToString() + onlynumber.ToString() + AlreadyNumber.ToString() + SurplusNumber.ToString() + productname + introduceuserid + oldrewardmoney;
            //string m5str = "逸趣rp";
            var md5pass = Unite.ToMD5(m5str);
            if (md5pass == pstr)
            {
                string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
                string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
                var resouce = new Resource(appid, appkey);
                var model = resouce.Factory("tb_OrderDetail");//要查询的云数据表
                var m2 = resouce.Factory("tb_Issue");

                string resultstr = "";
                for (var i = 1; i <= br; i++)
                {
                    string lastcode = "100" + onlynumber + (AlreadyNumber + i).ToString();
                    if (resultstr == "")
                    {
                        resultstr = resultstr + lastcode;
                    }
                    else
                    {
                        resultstr = resultstr + "," + lastcode;
                    }
                }
                var rets = model.Create(new
                {
                    I_ID = i_id,
                    U_ID = uid,
                    P_ID = pid,
                    buynumber = br,
                    code = resultstr,
                    ispay = true,
                    paytype= paytype,
                    ordertime = DateTime.Now.ToString()
                });

                var jsonstrtemp = HttpUtility.UrlDecode("[" + rets + "]");
                var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//查询的结果转json格式
                string resultid = jsondataformain[0].SelectToken("id").ToString();
                if (resultid != "" && resultid != null)
                {
                    var mbuycount = resouce.Factory("tb_buycount");
                    string buycountstr = "{ \"$inc\": { \"buycount\":" + br + "}}";
                    var buystr = mbuycount.Edit("5b1fc57822e6674007790e72", buycountstr);
                    if (SurplusNumber == br)
                    {
                        string resultstr1 = UpdateIssueStatereturn(i_id, pid, AlreadyNumber, SurplusNumber, issuenumber, countnumber, productname, introduceuserid, oldrewardmoney);
                        if (resultstr1 == "1")
                        {
                            setuserinfo(uid, by, introduceuserid);
                            result = "[{ \"result\":\"1\"}]";
                        }
                        else
                        {
                            result = "[{ \"result\":\"0\"}]";
                        }
                    }
                    else
                    {
                        string jsdatastr = "{ \"$inc\": { \"AlreadyNumber\":" + br + ",\"SurplusNumber\":-" + br + "}}";
                        var sstr = m2.Edit(i_id.Trim(), jsdatastr);
                        var jsonstrtemp1 = HttpUtility.UrlDecode("[" + sstr + "]");
                        var jsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp1) as JContainer;//查询的结果转json格式
                        string resultid1 = jsondataformain1[0].SelectToken("id").ToString();
                        if (resultid1 != "" && resultid1 != null)
                        {
                            setuserinfo(uid, by, introduceuserid);
                            result = "[{ \"result\":\"1\"}]";
                        }
                        else
                        {
                            result = "[{ \"result\":\"0\"}]";
                        }
                    }
                }
            }
            else
            {
                result = "[{ \"result\":\"3\"}]";
            }
            var jsonresult = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as JContainer;//查询的结果转json格式
            Context.Response.Charset = "UTF-8"; //设置字符集类型  
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Context.Response.Write(jsonresult);
            Context.Response.End();
        }
        #endregion

        #region 保存充值信息
        [WebMethod(EnableSession = true, Description = "保存充值信息")]
        public void saverecharge(string pstr, string paytype, string uid,int ry,string firstcharge,string paytypeid)
        {
            string result = "[{ \"result\":\"0\"}]";//最终执行结果 
            string m5str = "逸趣rp充值" + paytype + uid + ry + firstcharge.ToString()+ paytypeid.ToString();
            var md5pass = Unite.ToMD5(m5str);
            if (md5pass == pstr)
            {
                string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
                string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
                var resouce = new Resource(appid, appkey);
                var model = resouce.Factory("tb_Recharge");//要查询的云数据表
                var m2= resouce.Factory("user");//要查询的云数据表

                int chargemoney = 0;
                int give = 0;
                if (firstcharge == "否")
                {
                    chargemoney = ry * 2;
                    give = ry;
                }
                else {
                    chargemoney = ry;
                    give = 0;
                }
                var rets = model.Create(new
                {
                    PA_ID = paytypeid,
                    U_ID = uid,
                    R_Money = ry,
                    paytype= paytype,
                    give=give
                });

                var jsonstrtemp = HttpUtility.UrlDecode("[" + rets + "]");
                var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//查询的结果转json格式
                string resultid = jsondataformain[0].SelectToken("id").ToString();
                if (resultid != "" && resultid != null)
                {
                        string jsdatastr = "{ \"$inc\": { \"integral\":1,\"balance\":" + chargemoney + "},\"firstcharge\":\"是\"}";
                        var sstr = m2.Edit(uid.Trim(), jsdatastr);
                        var jsonstrtemp1 = HttpUtility.UrlDecode("[" + sstr + "]");
                        var jsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp1) as JContainer;//查询的结果转json格式
                        string resultid1 = jsondataformain1[0].SelectToken("id").ToString();
                        if (resultid1 != "" && resultid1 != null)
                        {
                            result = "[{ \"result\":\"1\"}]";
                        }
                        else
                        {
                            result = "[{ \"result\":\"0\"}]";
                        }
                }
            }
            else
            {
                result = "[{ \"result\":\"3\"}]";
            }
            var jsonresult = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as JContainer;//查询的结果转json格式
            Context.Response.Charset = "UTF-8"; //设置字符集类型  
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Context.Response.Write(jsonresult);
            Context.Response.End();
        }
        #endregion
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="by"></param>
        /// <param name="introduceuserid"></param>
        private void setuserinfo(string uid, int by, string introduceuserid) {
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
            var resouce = new Resource(appid, appkey);
            var m3 = resouce.Factory("user");
            int tempintegral = 0;
            if (by > 10)
            {
                tempintegral= (by / 10);
            }
            else {
                tempintegral = 1;
            }
            string jsdatastr2 = "{ \"$inc\": { \"integral\":" + tempintegral + "}}";
            var ss3 = m3.Edit(uid.Trim(), jsdatastr2);
            if (!string.IsNullOrWhiteSpace(introduceuserid))
            {
                //decimal tempmy = Convert.ToDecimal((by / 10));
                string jsdatastr3 = "{ \"$inc\": { \"rewardmoney\":" + tempintegral + "}}";
                var ss4 = m3.Edit(introduceuserid.Trim(), jsdatastr2);
                var jsonstrtemp2 = HttpUtility.UrlDecode("[" + ss4 + "]");
                var jsondataformain2 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp2) as JContainer;//查询的结果转json格式
                string resultid2 = jsondataformain2[0].SelectToken("id").ToString();
                if (resultid2 != "" && resultid2 != null)
                {
                    var rets2 = m3.Create(new
                    {
                        U_ID = introduceuserid,
                        rewardmoney = tempintegral,
                        isforward = "否",
                    });
                }
            }
        }
        #endregion

        #region 处理订单数据
        [WebMethod(EnableSession = true, Description = "处理订单数据")]
        public void ManageOrder(string i_id, int countnumber)
        {
            string result = "[{ \"result\":\" 0\"}]";//最终执行结果 
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];
            var resouce = new Resource(appid, appkey);
            var model = resouce.Factory("tb_OrderDetail");//要查询的云数据表 tb_OrderDetail
            var model1 = resouce.Factory("tb_Issue");

            string filter1 = "{\"where\":{ \"I_ID\":\"" + i_id + "\"}}";
            var ret1 = model.Query(filter1);//已购买完的产品期数数据
            var jsonData = HttpUtility.UrlDecode(ret1);
            var jsondataformain1 = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData) as JContainer;//查询的结果转json格式
            //Random r = new Random((int)DateTime.Now.Ticks);
            Random r = new Random(Guid.NewGuid().GetHashCode());
            int minnumber = 10000001;
            int maxnumber = 10000000 + countnumber;
            int luckycode = r.Next(minnumber, maxnumber);
            DataTable dt = new DataTable();
            dt.Columns.Add("i_id", typeof(string));
            dt.Columns.Add("code", typeof(string));
            dt.Columns.Add("u_id", typeof(string));
            dt.Columns.Add("buynumber", typeof(int));

            if (jsondataformain1.Count > 0)
            {
                for (int i = 0; i < jsondataformain1.Count; i++)
                {
                    DataRow row = dt.NewRow();
                    row["i_id"] = jsondataformain1[i].SelectToken("I_ID").ToString();
                    row["code"] = jsondataformain1[i].SelectToken("code").ToString();
                    row["buynumber"] = jsondataformain1[i].SelectToken("buynumber").ToString();
                    row["u_id"] = jsondataformain1[i].SelectToken("U_ID").ToString();
                    dt.Rows.Add(row);
                }
            }
            //dt = Newtonsoft.Json.JsonConvert.DeserializeObject(jsondataformain1, typeof(DataTable)) as DataTable;
            DataRow[] getrow = dt.Select("code like '%" + luckycode.ToString() + "%'");
            DataRow[] numberrow = dt.Select("i_id ='" + i_id.ToString() + "' and u_id='" + getrow[0]["u_id"].ToString() + "'");
            int resultbuynumber = 0;
            if (numberrow.Length > 0)
            {
                for (int i = 0; i < numberrow.Length; i++)
                {
                    resultbuynumber = resultbuynumber + Convert.ToInt32(numberrow[i]["buynumber"].ToString());
                }
            }
            if (getrow.Length > 0)
            {
                string luckyuid = getrow[0]["u_id"].ToString();
                string luckycodestr = luckycode.ToString();
                string announcedtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var resultstr = model1.Edit(i_id.Trim(), new { I_AnnouncedTime = announcedtime, LuckCode = luckycodestr, luckybuynumber = resultbuynumber, LuckU_ID = luckyuid, I_State = "已揭晓" });
                var jsonstrtemp = HttpUtility.UrlDecode("[" + resultstr + "]");
                var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//查询的结果转json格式
                string resultid = jsondataformain[0].SelectToken("id").ToString();
                if (resultid != "" && resultid != null)
                {
                    result = "[{ \"result\":\" 1\"}]";
                }
                else
                {
                    result = "[{ \"result\":\" 0\"}]";
                }
            }
            var jsonresult = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as JContainer;//查询的结果转json格式
            Context.Response.Charset = "UTF-8"; //设置字符集类型  
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Context.Response.Write(jsonresult);
            Context.Response.End();
        }
        #endregion


        #region 极光消息推送
        [WebMethod(EnableSession = true, Description = "极光消息推送")]
        public string JGPushInfo(string content)
        {
            jpush jsp = new jpush();
            jsp.ExecutePushExample(content,"");
            return "";
        }
        #endregion

        #region 微信支付统一下单
        [WebMethod(EnableSession = true, Description = "微信支付统一下单")]
        public void UnifiedOrder(string body,int totalmoney)
        {
            WxPayData parmdata = new WxPayData();
            parmdata.SetValue("body", body);//商品描述
            parmdata.SetValue("attach", "逸趣网络科技有限公司");//附加数据
            parmdata.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());//随机字符串
            parmdata.SetValue("total_fee", totalmoney);//总金额
            parmdata.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            parmdata.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            parmdata.SetValue("goods_tag", "");//商品标记
            parmdata.SetValue("trade_type", "APP");//交易类型
            //parmdata.SetValue("product_id", productid);//商品ID

            WxPayData resultdata=WxPayApi.UnifiedOrder(parmdata);
            string resultcode = resultdata.GetValue("return_code").ToString();
            string prepay_id= resultdata.GetValue("prepay_id").ToString();
            string resultstr = "";
            string noncestr = GenerateNonceStr();
            if (resultcode.Equals("SUCCESS"))
            {
                resultstr= "[{ \"result\":\""+ resultcode + "\",\"prepay_id\":\"" + prepay_id+ "\",\"noncestr\":\""+ noncestr + "\"}]";
            }
            else
            {
                resultstr = "[{ \"result\":\" " + resultcode + "\",\"prepay_id\":\"0\",\"noncestr\":\"" + noncestr + "\"}]";
            }
            var jsonresult = Newtonsoft.Json.JsonConvert.DeserializeObject(resultstr) as JContainer;//查询的结果转json格式
            Context.Response.Charset = "UTF-8"; //设置字符集类型  
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Context.Response.Write(jsonresult);
            Context.Response.End();
            //return resultstr;
        }
        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        #endregion


        #region 支付宝支付生成签名字符串
        [WebMethod(EnableSession = true, Description = "微信支付统一下单")]
        public void CreateSignStr(string body, string totalmoney,string subject)
        {
            string appid = "2018041602567852";//appid
            string app_private_key = "MIIEpQIBAAKCAQEAyUyCGfwC0XFYOMjAvYntfPuaYvCEldZ/5QKJMFaYKj1dSZU8vuam0I6WMntTcR+xT64k8zrq/Tlw5gI+251PXUbabB9QWjZSMeupFjDc2FN6vBYWZvtyieJz7HE6o2Y0sCu02xi6kqhHwq3ySMxoYe+VfQNh35d7rKQzBen9a/0uXrwSbg1M/aQcnBeWOWGnBto+HDyGGEnoJKsxekE26UVls85cDi7wI7kZbOJuVhpYj9odEdZBvn+Jg5SoPc/Z/s0fqWtNpgHNQG5KdwrFu0QQjjIZVDMslMWvYqr2QOj1j5+pD3dJMyKGkkgc3CPzQ6n1tPQQvnSNQt1AzbfSNwIDAQABAoIBAQCN2XIcqW+682o9qYnYhqdp2UrzyZVEmUDKujy+aWcU7OUeAyIpTBPlB3Vj4W/tWW3zPj4fgDczdhTOoGp3C6Vvj4w/gNl4mKrXLr+aOZiGgF0OyWnD7BDMhV03EptFpbIfKs1pT0W6LwdSco03K4Oq78+hpo6DpxWplJO36SmBvEloDgJzZiJLlAkiVFhQxTipDWLl3thEcfdquiUDia5PAszEcoDlnGQdFOY5p2wWbHtbWqVnXBFNnUQTQYsZ1XNN9lL0t2n5bPLcejY5SyS7KaFkh/gdELUtZV8LG9yo3veXsUhxY61OrPpeZLis9JtuRL9PNKMfYkwpuDBZVe1xAoGBAO6vcgcR5B1pKqD500kLwuNBbU1AQkRiiRr7KM4je7A+qd/7Ar4McftrVoUBlryBcC4CI2AKaZ6UTwqxFpA0A6YcenjCGAKYQOZrOGSEIeUP6Nfil/apdy+J9ynwYjt/oKCWfQwb0mgqM5IJYk1I1ZUEkO+K64PUVmAgOPww8k3rAoGBANfmxQk2ZZN7j6nJBGrF7heg5SFx27wN4QUOicMVlvxHr94QVseMK0H8SOKLCdpaXZet+ckoXZEpdzUPdbA0dKr6sXWQBxSMKFDNLYvRWqixCq95Hg0K2bAiVFKZyqeBMXJZVBpQoF/AoA6xA5KGAZdtGE5IWClKXRWCfoSSzZ3lAoGBAKtRwsbQUKvLkI16w+zqRDhZ/do1BVuQXli/bcqILX+TetsJkC5ZQHb11GQjf85OGfbsEfgdgTIRwaoq8ccPjo7sYfvLVPCH2A2LaC69qJaBlN9gBTNG8AVvQbkYkWmjcefSHG9UiPG7WMi5c5WFcchEPsOxMtqszlKwzjY167WBAoGANxnoc590cR153uU0wWNejp07nTuHzwjjwvyg4C8kZ6KMGeqlmywE5kRS/a5qh1XEyS9XrqUkrCWfDOWzLZNVq0VsAQsPI4lZyLV0yFhYAPGePoZ0yvNX94Hrb2FcvT9VtU9jDYxCQe3Ra651sPGOem0XZPNFvNQDybeSPpeQ7pkCgYEAo4JKQ+SWGfrJSOv9BVZ3QWsXv8del0UupM8XJKrtb6gt47HgrTx2FKQDNhn8QqFAVkWHzaeOQODQ8OAeOhwewv/wPxhyxK9D2DsUpYDY6d/azYbYZE+gznekuyuP1BPt0lerJfW52tEeGEOdC9YSWY0fbq9qw7cRFP4k+7ZnLM8=";//私钥
            string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyUyCGfwC0XFYOMjAvYntfPuaYvCEldZ/5QKJMFaYKj1dSZU8vuam0I6WMntTcR+xT64k8zrq/Tlw5gI+251PXUbabB9QWjZSMeupFjDc2FN6vBYWZvtyieJz7HE6o2Y0sCu02xi6kqhHwq3ySMxoYe+VfQNh35d7rKQzBen9a/0uXrwSbg1M/aQcnBeWOWGnBto+HDyGGEnoJKsxekE26UVls85cDi7wI7kZbOJuVhpYj9odEdZBvn+Jg5SoPc/Z/s0fqWtNpgHNQG5KdwrFu0QQjjIZVDMslMWvYqr2QOj1j5+pD3dJMyKGkkgc3CPzQ6n1tPQQvnSNQt1AzbfSNwIDAQAB";//公钥
            string charset = "utf-8";
            string outtradeno = WxPayApi.GenerateOutTradeNo();
            string notifyurl = "https://www.rpshoping.com/AliNotifyUrl.aspx";
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", appid, app_private_key.Trim(), "json", "1.0", "RSA2", alipay_public_key.Trim(), charset, false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.Body = body;
            model.Subject = subject;
            model.TotalAmount = totalmoney;
            model.ProductCode = "QUICK_MSECURITY_PAY";
            model.OutTradeNo = outtradeno;
            model.TimeoutExpress = "30m";
            request.SetBizModel(model);
            request.SetNotifyUrl(notifyurl);
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
            //Response.Write(HttpUtility.HtmlEncode(response.Body));
            string resultcode = HttpUtility.HtmlEncode(response.Body);
            string tempresult = resultcode.Replace("amp;","");
            //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。

            //string resultcode = resultdata.GetValue("return_code").ToString();
            //string prepay_id = resultdata.GetValue("prepay_id").ToString();
            string resultstr = "[{ \"result\":\"" + tempresult + "\"}]";
            //string noncestr = GenerateNonceStr();
            //if (resultcode.Equals("SUCCESS"))
            //{
            //    resultstr = "[{ \"result\":\"" + resultcode + "\",\"prepay_id\":\"" + prepay_id + "\",\"noncestr\":\"" + noncestr + "\"}]";
            //}
            //else
            //{
            //    resultstr = "[{ \"result\":\" " + resultcode + "\",\"prepay_id\":\"0\",\"noncestr\":\"" + noncestr + "\"}]"; ;
            //}
            var jsonresult = Newtonsoft.Json.JsonConvert.DeserializeObject(resultstr) as JContainer;//查询的结果转json格式
            Context.Response.Charset = "UTF-8"; //设置字符集类型  
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Context.Response.Write(jsonresult);
            Context.Response.End();
            //return resultstr;
        }
        #endregion
    }
}
