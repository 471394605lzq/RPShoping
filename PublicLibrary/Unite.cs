using DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace PublicLibrary
{
   public static class Unite
    {
        public const int MAXFreeSpace = 500;//小于500M,删除以前的备份,保留最后一次备份
        public const int MaxFileCount = 10;//备份文件超过10个时,删除以前的备份,保留最近10个备份

        /// <summary>
        /// 返回数据库备份目录
        /// </summary>
        /// <returns></returns>
        public static string DBBackupPath()
        {
            return Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "DbBack");
        }
        /// <summary>
        /// 将datatable转换成json
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        public static string ToJson(this DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }

            return javaScriptSerializer.Serialize(arrayList);  //返回一个json字符串
        }
        /// <summary>
        /// 获取数据库备份名
        /// </summary>
        /// <returns></returns>
        public static string NewDBBackupName()
        {
            var i = 1;
            var extName = ".bak";
            var Path = DBBackupPath();
            Path = Path.EndsWith("\\") ? Path : (Path + "\\");

            string dbName = GetCurrDbName();
            string dbNameSuffix = string.Format("_{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var dbBackupName = dbName + dbNameSuffix + extName;
            var dbBackupPath = Path + dbBackupName;
            while (File.Exists(dbBackupPath))
            {
                i++;
                dbBackupName = dbName + dbNameSuffix + "_" + i + extName;
                dbBackupPath = Path + dbBackupName;
            }
            return dbBackupName;
        }

        /// <summary>
        /// 获取当前数据库的名称
        /// </summary>
        /// <returns></returns>
        public static string GetCurrDbName()
        {
            string dbname = "";
            var pattern = "database=(?<dbname>[^;]*);";
            var connStr = DBHelper.connectionString;
            var reg = new Regex(pattern, RegexOptions.IgnoreCase);
            if (reg.IsMatch(connStr))
            {
                dbname = reg.Match(connStr).Result("${dbname}");
            }
            return dbname;
        }

        /// <summary>
        /// 备份数据库到备份目录下
        /// </summary>
        public static void BackupDatabase()
        {
            var dbname = GetCurrDbName();
            if (dbname == string.Empty)
            {
                return;
            }
            var dbBackName = NewDBBackupName();

            //获取数据库备份目标,并检查空间是否够用,不够则删除以前的备份
            var dbBackDir = Unite.DBBackupPath();
            CheckDriveFreeSpace(dbBackDir);

            if (!dbBackDir.EndsWith("\\")) dbBackDir += "\\";
            var filename = string.Format("{0}{1}", dbBackDir, dbBackName);
            var sql = string.Format("BACKUP DATABASE [{0}] TO  DISK = '{1}' WITH NOFORMAT, NOINIT,  NAME = N'{0}-完整 数据库 备份', SKIP, NOREWIND, NOUNLOAD, COMPRESSION, STATS = 10", dbname, filename);

            DBHelper.ExcuteSql(sql);
        }

        /// <summary>
        /// 恢复数据库
        /// </summary>
        public static void RestoreDatabase(string backupName)
        {
            string sql = "";
            string dbName = GetCurrDbName();
            string dbBackPath = DBBackupPath();
            dbBackPath = dbBackPath.EndsWith("\\") ? dbBackPath : (dbBackPath + "\\");
            string dbBackFullName = dbBackPath + backupName;
            if (!File.Exists(dbBackFullName))
                throw new FileNotFoundException("指定的备份文件不存在");

            sql += "declare @sql nvarchar(4000);" +
                    "set @sql='';" +
                    "select @sql='exec(''kill '+convert(nvarchar(50),spid)+''');'+@sql from sysprocesses where dbid=db_id('" + dbName + "');" +
                    "exec sp_executesql @sql;";

            sql += "ALTER DATABASE " + dbName + " SET OFFLINE WITH ROLLBACK IMMEDIATE;";

            sql += string.Format("RESTORE DATABASE [{0}] FROM  DISK = N'{1}' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10", dbName, dbBackFullName);

            //修改连接字符串,指向master数据库
            var connStr = DBHelper.connectionString;
            connStr = connStr.Replace("database=" + dbName, "database=master");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                command.CommandTimeout = 60;

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        /// <summary>
        /// 删除数据库备份文件
        /// </summary>
        /// <param name="files"></param>
        public static void DeleteBackupFile(string files)
        {
            var dbBackPath = DBBackupPath();
            dbBackPath = dbBackPath.EndsWith("\\") ? dbBackPath : (dbBackPath + "\\");
            var fileInfos = files.Split(',').Select(p => dbBackPath + p).Select(p => new FileInfo(p));
            if (fileInfos.Any(p => !p.Exists))
                throw new FileNotFoundException("部分文件未找到,无法删除");

            fileInfos.ToList().ForEach(p => p.Delete());
        }

        /// <summary>
        /// 检查空闲空间与文件数
        /// </summary>
        /// <param name="dir"></param>
        private static void CheckDriveFreeSpace(string dir)
        {
            List<FileInfo> needDeletedFiles = new List<FileInfo>();
            var driverNo = Path.GetPathRoot(dir);
            DriveInfo di = new DriveInfo(driverNo);
            var freeSpace = di.AvailableFreeSpace / 1024 / 1024;
            var files = Directory.GetFiles(dir, "*.bak", SearchOption.TopDirectoryOnly);
            if (files.Length == 0) { return; }

            if (freeSpace < MAXFreeSpace)//小于500M,删除以前的备份,保留最后一次备份
            {
                needDeletedFiles = files.Select(p => new FileInfo(p)).OrderByDescending(p => p.CreationTime).Skip(1).ToList();
            }
            else if (files.Length > MaxFileCount)
            {
                needDeletedFiles = files.Select(p => new FileInfo(p)).OrderByDescending(p => p.CreationTime).Skip(MaxFileCount).ToList();
            }

            //执行删除
            needDeletedFiles.ForEach(p =>
            {
                p.Delete();
            });
        }

        /// <summary>
        /// 获取随机数种子
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            //使用加密服务提供程序 (CSP)提供的实现来实现加密随机数生成器 (RNG)。无法继承此类。
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);//用经过加密的强随机值序列填充字节数组。
            return BitConverter.ToInt32(bytes, 0);
        }

        //转换为md5的字符串
        public static string ToMD5(string str)
        {
            var md5 = MD5.Create();
            byte[] source = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sBuilder.Append(source[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        //转换为md5的字符串
        public static string ToMD5New(string source)
        {
            char[] hexDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

            var md5 = MD5.Create();
            //此处一定要用Default编码方式编码
            byte[] md = md5.ComputeHash(Encoding.Default.GetBytes(source));

            int j = md.Length;
            char[] str = new char[j * 2];
            int k = 0;
            for (int i = 0; i < j; i++)
            {
                byte byte0 = md[i];
                str[k++] = hexDigits[byte0 >> 4 & 0xf];
                str[k++] = hexDigits[byte0 & 0xf];
            }
            return string.Join("", str);
        }
        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="strUserPassWord">待加密的密码字符串</param>
        /// <param name="bIsUpper">是否返回大写加密后字符串</param>
        /// <returns>返回32位加密后的密码字符串</returns>
        public static string Md532(string source, bool bIsUpper)
        {
            //加密后字符串
            //string strPassWord = "";
            //MD5 md5 = MD5.Create();//实例化一个md5对像
            //// 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            //byte[] byteStr = md5.ComputeHash(Encoding.UTF8.GetBytes(strUserPassWord));
            //// 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            //for (int i = 0; i < byteStr.Length; i++)
            //{
            //    // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            //    if (bIsUpper)
            //        strPassWord = strPassWord + byteStr[i].ToString("X");
            //    else
            //        strPassWord = strPassWord + byteStr[i].ToString();

            //}
            //return strPassWord;

            char[] hexDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

            var md5 = MD5.Create();
            //此处一定要用Default编码方式编码
            byte[] md = md5.ComputeHash(Encoding.UTF8.GetBytes(source));

            int j = md.Length;
            char[] str = new char[j * 2];
            int k = 0;
            for (int i = 0; i < j; i++)
            {
                byte byte0 = md[i];
                str[k++] = hexDigits[byte0 >> 4 & 0xf];
                str[k++] = hexDigits[byte0 & 0xf];
            }
            return string.Join("", str);
        }

        ///<summary>
        ///得到客户端ip
        ///</summary>
        ///<returns>ip</returns>
        public static string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

        /// <summary>
        /// 根据IP地址获取归属地
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetIPArea(string ip)
        {
            try
            {
                var url = "https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?query=" + ip + "&co=&resource_id=6006&t=1434011367448&ie=utf8&oe=gbk&cb=op_aladdin_callback&format=json&tn=baidu&cb=jQuery1102009208318265154958_1433987923394&_=" + DateTime.Now.Ticks;
                WebClient client = new WebClient();
                string source = client.DownloadString(url);
                var sp = source.IndexOf("(");
                var ep = source.IndexOf(")");
                source = source.Substring(sp + 1, ep - sp - 1);
                var obj = JsonConvert.DeserializeObject(source) as JToken;
                var data = obj.Last;
                return data.First.First.First.First.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
