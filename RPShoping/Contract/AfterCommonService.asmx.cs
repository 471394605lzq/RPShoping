using DAL;
using Microsoft.Ajax.Utilities;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PublicLibrary;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.RS;
using Qiniu.RS.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;



namespace RPShoping.Contract
{
    /// <summary>
    /// AfterCommonService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]

    public class AfterCommonService : System.Web.Services.WebService
    {

        #region 后台公用删除方法
        [WebMethod(EnableSession = true, Description = "后台公用删除方法")]
        public string CommonDelete(string tablename,string id,string idvalue)
        {
            string result = "";
            try
            {
                //idvalue可为一段id拼好的字符串字段(适用于批量删除)
                string sql = string.Format(@"DELETE FROM {0} WHERE {1} in({2})", tablename, id, idvalue);
                int resultorw = DBHelper.ExecuteSql(sql);
                if (resultorw > 0)
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
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="tablename">要删除数据的表名</param>
        /// <param name="id">表数据id</param>
        /// <param name="idvalue">id的value值</param>
        /// <returns>返回sql语句执行影响的行数</returns>
        [WebMethod(EnableSession = true, Description = "后台公用物理删除方法")]
        public string CommonIsDelete(string tablename, string id, string idvalue,int state,string setcolum)
        {
            string result = "";
            try
            {
                //idvalue可为一段id拼好的字符串字段(适用于批量删除)
                string sql = string.Format(@"update {0} set {4}={3} WHERE {1} in({2})", tablename, id, idvalue, state, setcolum);
                int resultorw = DBHelper.ExecuteSql(sql);
                if (resultorw > 0)
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


        #region 生成七牛云上传使用的token
        [WebMethod(EnableSession = true, Description = "生成七牛云上传使用的token")]
        public void SetQiNiuToken()
        {
            string upToken = "";
            QiniuToken token = new QiniuToken();
            try
            {
                PutPolicy putPolicy = new PutPolicy();
                putPolicy.Scope = "test";// + ":" + "myfile";
                putPolicy.SetExpires(3600);
                //putPolicy.DeleteAfterDays = 1;
                Mac mac = new Mac("AwGglhX2wy5BX36zbHL_5YfC--EiQFWPdE44oblq", "X7nOg3cLkpVff_ZSor2zTUTmbYeMaJWaujsXX_Yd");
                Auth auth = new Auth(mac);
                upToken=auth.CreateUploadToken(putPolicy.ToJsonString());
                token.uptoken = upToken;
            }
            catch (Exception ex)
            {
                upToken = ex.ToString();
            }
            //Context.Response.Write();
            string s = JsonHelper.ObjectToJSON(token);
            Context.Response.Write(JsonHelper.ObjectToJSON(token));
            Context.Response.End();

            //Response.ContentType = "application/json";
            //context.Response.Write(JsonConvert.SerializeObject(token));
            //return upToken;
        }
        #endregion

        #region 生成七牛云上传使用的token
        [WebMethod(EnableSession = true, Description = "生成七牛云上传使用的token")]
        public string SetQiNiuToken2()
        {
            string upToken = "";
            //QiniuToken token = new QiniuToken();
            try
            {
                PutPolicy putPolicy = new PutPolicy();
                putPolicy.Scope = "test";// + ":" + "myfile";
                putPolicy.SetExpires(3600);
                //putPolicy.DeleteAfterDays = 1;
                Mac mac = new Mac("AwGglhX2wy5BX36zbHL_5YfC--EiQFWPdE44oblq", "X7nOg3cLkpVff_ZSor2zTUTmbYeMaJWaujsXX_Yd");
                Auth auth = new Auth(mac);
                upToken = auth.CreateUploadToken(putPolicy.ToJsonString());
                //token.uptoken = upToken;
            }
            catch (Exception ex)
            {
                upToken = ex.ToString();
            }
            //string s = JsonHelper.ObjectToJSON(token);
            //Context.Response.Write(JsonHelper.ObjectToJSON(token));
            //Context.Response.End();
            return upToken;
        }
        #endregion


        #region 七牛云上传方法
        /// <summary>
        /// 简单上传-上传小文件 HttpPostedFileBase img
        /// </summary>
        //[WebMethod(EnableSession = true, Description = "七牛云上传方法")]
        //public void UploadFile()
        //{
        //    // 生成(上传)凭证时需要使用此Mac
        //    // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
        //    // 实际应用中，请自行设置您的AccessKey和SecretKey
        //    Mac mac = new Mac("AwGglhX2wy5BX36zbHL_5YfC--EiQFWPdE44oblq", "X7nOg3cLkpVff_ZSor2zTUTmbYeMaJWaujsXX_Yd");
        //    string bucket = "test";
        //    string saveKey = "1.png";
        //    string localFile = "D:\\QFL\\1.png";
        //    // 上传策略，参见 
        //    // https://developer.qiniu.com/kodo/manual/put-policy
        //    PutPolicy putPolicy = new PutPolicy();
        //    // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
        //    // putPolicy.Scope = bucket + ":" + saveKey;
        //    putPolicy.Scope = bucket;
        //    // 上传策略有效期(对应于生成的凭证的有效期)          
        //    putPolicy.SetExpires(3600);
        //    // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
        //    //putPolicy.DeleteAfterDays = 1;
        //    // 生成上传凭证，参见
        //    // https://developer.qiniu.com/kodo/manual/upload-token            
        //    string jstr = putPolicy.ToJsonString();
        //    string token = Auth.CreateUploadToken(mac, jstr);
        //    UploadManager um = new UploadManager();
        //    HttpResult result = um.UploadFile(localFile, saveKey, token);
        //    Console.WriteLine(result);
        //}
        #endregion

        #region 删除七牛云上的图片
        [WebMethod(EnableSession = true, Description = "删除七牛云上的图片")]
        public  string DeleteQiNiuImage(string key)
        {
            string resultstr = "";
            string tempkey = key.Trim();

            try
            {
                Mac mac = new Mac("AwGglhX2wy5BX36zbHL_5YfC--EiQFWPdE44oblq", "X7nOg3cLkpVff_ZSor2zTUTmbYeMaJWaujsXX_Yd");
                string bucket = "test";
                BucketManager bm = new BucketManager(mac);
                var result = bm.Delete(bucket, tempkey);
                resultstr = result.Code.ToString();

            }
            catch (Exception ex)
            {
                resultstr = ex.ToString();
            }
            //string s = JsonHelper.ObjectToJSON(token);
            //Context.Response.Write(JsonHelper.ObjectToJSON(token));
            //Context.Response.End();
            return resultstr;
        }
        #endregion
        #region 删除七牛云上的图片
        [WebMethod(EnableSession = true, Description = "删除七牛云上的图片")]
        public void DeleteQiNiuImage2(string key)
        {
            string[] liststr = key.Split('/');
            string resultstr = "";
            string tempkey = liststr[3].Trim();

            try
            {
                Mac mac = new Mac("AwGglhX2wy5BX36zbHL_5YfC--EiQFWPdE44oblq", "X7nOg3cLkpVff_ZSor2zTUTmbYeMaJWaujsXX_Yd");
                string bucket = "test";
                BucketManager bm = new BucketManager(mac);
                var result = bm.Delete(bucket, tempkey);
                resultstr = result.Code.ToString();

            }
            catch (Exception ex)
            {
                resultstr = ex.ToString();
            }
            string resultstrt = "[{ \"result\":\"" + resultstr + "\"}]";
            //string s = JsonHelper.ObjectToJSON(token);
            //Context.Response.Write(JsonHelper.ObjectToJSON(token));
            //Context.Response.End();
            var jsonresult = Newtonsoft.Json.JsonConvert.DeserializeObject(resultstr) as JContainer;//查询的结果转json格式
            Context.Response.Charset = "UTF-8"; //设置字符集类型  
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Context.Response.Write(jsonresult);
            Context.Response.End();
        }
        #endregion

        #region 获取省份/城市/镇区信息
        [WebMethod(EnableSession = true, Description = "获取省份/城市/镇区信息")]
        public string getareainfo(string id)
        {
            string result = "";
            try
            {
                string sql = string.Format(@"SELECT * FROM Area WHERE ParentID={0}", id);
                DataTable dt = DBHelper.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    result = DataConvert.ConvertDsToJson(dt);
                }
                else
                {
                    result = "0";
                }
            }
            catch (Exception ex)
            {
                result = "0";
            }
            return result;
        }
        #endregion
    }
}
