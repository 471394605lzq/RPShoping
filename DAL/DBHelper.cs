using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace DAL
{
    public enum EffentNextType
    {
        /// <summary>   
        /// 对其他语句无任何影响    
        /// </summary>   
        None,
        /// <summary>   
        /// 当前语句必须为"SELECT COUNT(1) FROM .."格式，如果存在则继续执行，不存在回滚事务   
        /// </summary>   
        WhenHaveContine,
        /// <summary>   
        /// 当前语句必须为"SELECT COUNT(1) FROM .."格式，如果不存在则继续执行，存在回滚事务   
        /// </summary>   
        WhenNoHaveContine,
        /// <summary>   
        /// 当前语句影响到的行数必须大于0，否则回滚事务   
        /// </summary>   
        ExcuteEffectRows,
        /// <summary>   
        /// 引发事件-当前语句必须为"SELECT COUNT(1) FROM .."格式，如果不存在则继续执行，存在回滚事务   
        /// </summary>   
        SolicitationEvent
    }
    public class CommandInfo
    {
        public object ShareObject = null;
        public object OriginalData = null;
        event EventHandler _solicitationEvent;
        public event EventHandler SolicitationEvent
        {
            add
            {
                _solicitationEvent += value;
            }
            remove
            {
                _solicitationEvent -= value;
            }
        }
        public void OnSolicitationEvent()
        {
            if (_solicitationEvent != null)
            {
                _solicitationEvent(this, new EventArgs());
            }
        }
        public string CommandText;
        public CommandType ComType;
        public System.Data.Common.DbParameter[] Parameters;
        public EffentNextType EffentNextType = EffentNextType.None;
        public CommandInfo()
        {
            ComType = CommandType.Text;
        }
        public CommandInfo(string sqlText, SqlParameter[] para)
            : this()
        {
            this.CommandText = sqlText;
            this.Parameters = para;
        }
        public CommandInfo(string sqlText, SqlParameter[] para, EffentNextType type)
            : this()
        {
            this.CommandText = sqlText;
            this.Parameters = para;
            this.EffentNextType = type;
        }
        public CommandInfo(string sqlText, SqlParameter[] para, EffentNextType type, CommandType comType)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
            this.EffentNextType = type;
            this.ComType = comType;
        }
    }
    /// <summary>   
    /// 数据访问抽象基础类   
    /// Copyright (C) 2004-2008 By LiTianPing    
    /// </summary>
    public partial class DBHelper
    {

        public DBHelper()
        {

        }

        const string cacheKey = "CRMConnectionstring";
        const int maxValidMinute = 30;
        //static EncryptManager em = new EncryptManager();
        static object syncObj = new object();
        public const string TimeCacheKey = "CRMTimeValidCacheKey";

        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.         
        public static string connectionString
        {
            get { return ReadConnectionString(); }
        }
        public static bool EncryIsPass()
        {
            return connectionString != string.Empty;
        }
        private static string ReadConnectionString()
        {
            var connectionstring = "";

            //查询cache中是否存在连接字符串
            if (HttpRuntime.Cache[cacheKey] != null)
            {
                connectionstring = HttpRuntime.Cache[cacheKey] as string;
            }

            //如果没有则查询配置
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                lock (syncObj)
                {
                    if (string.IsNullOrWhiteSpace(connectionstring))
                    {
                        //if (em.QueryEncryptHardwareState())
                        //{
                            //返回连接字符串，并緩存
                            connectionstring = ConfigurationManager.ConnectionStrings["AFaneTiConectStr"].ConnectionString;
                            HttpRuntime.Cache.Insert(cacheKey, connectionstring, null, DateTime.Now.AddMinutes(maxValidMinute), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                        //}
                        //else
                        //{
                        //    throw new InvalidOperationException("获取连接字符串时，验证失败！");
                        //}
                    }
                }
            }

            return connectionstring;
        }
        public static void ClearConnectionCache()
        {
            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }

        #region SQL连接 AppSettings

        public static SqlConnection Connection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();

            }
            else
            {
                connection.Close();
                connection.Open();

            }

            return connection;
        }
        #endregion
        #region SQL语句
        /// <summary>
        /// 执行sql语句，返回影响的记录数[增、删、改]
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static int GetExecuteNonQuery(string sqlString)
        {
            using (SqlConnection connection = Connection())
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static DataSet GetQuery(string sqlString)
        {
            using (SqlConnection connection = Connection())
            {
                DataSet ds = new DataSet();
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(sqlString, connection);
                    da.Fill(ds, "ds");
                    connection.Close();
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
                return ds;
            }
        }
        /// <summary>
        /// 创建数据集，填充DataTable:Ajax自动补全功能用到
        /// </summary>
        /// <param name="safeSql"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string safeSql)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = Connection())
            {
                using (SqlCommand cmd = new SqlCommand(safeSql, connection))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    connection.Close();
                    return ds.Tables[0];
                }
            }
        }
        /// <summary>
        /// 执行返回首行首列的方法
        /// </summary>
        /// <param name="safeSql"></param>
        /// <returns></returns>
        public static int GetScalar(string safeSql)
        {
            using (SqlConnection connection = Connection())
            {
                using (SqlCommand cmd = new SqlCommand(safeSql, connection))
                {
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    connection.Close();
                    return result;
                }
            }
        }

        /// <summary>
        /// 执行返回首行首列的方法
        /// </summary>
        /// <param name="safeSql"></param>
        /// <returns></returns>
        public static object GetScalarobj(string safeSql)
        {
            using (SqlConnection connection = Connection())
            {
                using (SqlCommand cmd = new SqlCommand(safeSql, connection))
                {
                    object result = cmd.ExecuteScalar();
                    connection.Close();
                    return result;
                }
            }
        }


        /// <summary>
        /// 执行查询语句，返回OracleDataReader
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        public static SqlDataReader GetExecuteReader(string sql)
        {
            SqlConnection connection = Connection();
            SqlCommand cmd = new SqlCommand(sql, connection);
            try
            {
                cmd.CommandTimeout = 20 * 60;
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException E)
            {
                throw new Exception(E.Message);
            }
        }
        #endregion
        #region 存储过来执行
        /// <summary>
        ///  执行存储过程 返回查询结果集
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static SqlDataReader GetPROSqlDataReader(string procedure, params SqlParameter[] where)
        {
            SqlDataReader myReader = null;
            SqlConnection connection = Connection();
            SqlCommand cmd = new SqlCommand(procedure, connection);
            try
            {
                cmd.CommandText = procedure;
                cmd.Parameters.AddRange(where);
                cmd.CommandType = CommandType.StoredProcedure;//设置cmd的类型为存储过程
                myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException E)
            {

                throw new Exception(E.Message);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 返回DataSet集合
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataSet GetPRODataSet(string procedure, params SqlParameter[] where)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = Connection())
            {
                using (SqlCommand cmd = new SqlCommand(procedure, connection))
                {
                    cmd.CommandText = procedure;
                    cmd.Parameters.AddRange(where);
                    cmd.CommandType = CommandType.StoredProcedure;//设置cmd的类型为存储过程
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    connection.Close();
                    return ds;
                }
            }
        }
        /// <summary>
        ///  增删改 返回影响行数
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int GetPROExecuteNonQuery(string procedure, params SqlParameter[] where)
        {
            using (SqlConnection connection = Connection())
            {
                using (SqlCommand cmd = new SqlCommand(procedure, connection))
                {
                    try
                    {
                        cmd.CommandText = procedure;
                        cmd.Parameters.AddRange(where);
                        cmd.CommandType = CommandType.StoredProcedure;//设置cmd的类型为存储过程
                        int count = cmd.ExecuteNonQuery();
                        return count;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        connection.Close();
                        return 0;
                    }
                }
            }
        }
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static object GetPROExecuteScalar(string procedure, SqlParameter[] where)
        {
            using (SqlConnection connection = Connection())
            {
                using (SqlCommand cmd = new SqlCommand(procedure, connection))
                {
                    try
                    {
                        cmd.CommandText = procedure;
                        cmd.Parameters.AddRange(where);
                        cmd.CommandType = CommandType.StoredProcedure;//设置cmd的类型为存储过程
                        object count = cmd.ExecuteScalar();
                        return count;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        connection.Close();
                        return 0;
                    }
                }
            }
        }
        #endregion

        #region 事务
        /// <summary>
        /// 多条数据插入事务
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool GetTransaction(List<string> list)
        {
            SqlConnection cnn = Connection();
            SqlCommand cm = new SqlCommand();
            cm.Connection = cnn;
            SqlTransaction trans = cnn.BeginTransaction();
            try
            {
                foreach (string str in list)
                {
                    cm.Transaction = trans;
                    cm.CommandText = str;
                    cm.ExecuteNonQuery();
                }
                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
            finally
            {
                cnn.Close();
                trans.Dispose();
                cnn.Dispose();
            }
        }
        #endregion


        public static void DDdd(string ddd)
        {

            //"User=SYSDBA;Password=masterkey;Database=test.fdb;
            string contionstr = "User=sysdba;Password=masterkey;Database=test.fdb;DataSource=localhost;Port=3050;Dialect=3; Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=4096;ServerType=0; ";
            OleDbConnection contion = new OleDbConnection(contionstr);
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = contion;
            cmd.CommandText = "";

        }

        /// <summary>
        /// 获取安全的sql值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetSafeSqlValue(string value)
        {
            if (value == null) return string.Empty;
            return value.Replace("'", "''");
        }

        public static void TableValuedToDB(DataTable dt)
        {
            SqlConnection sqlConn = Connection();
            const string TSqlStatement =
             "insert into BulkTestTable (Id,UserName,Pwd)" +
             " SELECT nc.Id, nc.UserName,nc.Pwd" +
             " FROM @NewBulkTestTvp AS nc";
            SqlCommand cmd = new SqlCommand(TSqlStatement, sqlConn);
            SqlParameter catParam = cmd.Parameters.AddWithValue("@NewBulkTestTvp", dt);
            catParam.SqlDbType = SqlDbType.Structured;
            //表值参数的名字叫BulkUdt，在上面的建立测试环境的SQL中有。  
            catParam.TypeName = "dbo.BulkUdt";
            try
            {
                if (dt != null && dt.Rows.Count != 0)
                    cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }
        }



        public static SqlDataReader GetDRSql(string sql)
        {
            SqlConnection con = Connection();
            SqlCommand cmd = new SqlCommand(sql, con);

            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        //根据SQL语句获取记录集
        public static DataSet GetDsBySql(string sql)
        {
            DataSet ds = new DataSet();
            SqlConnection con = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                con = Connection();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.Connection = con;
                da.SelectCommand = cmd;

                da.Fill(ds);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (con != null)
                {
                    con.Dispose();
                }
                cmd.Dispose();
                da.Dispose();
            }
            return ds;
        }
        //根据SQL语句获取记录集
        public static bool ExcuteSql(string sql)
        {
            Boolean isSuccess = false;
            SqlConnection con = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                con = Connection();
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                isSuccess = true;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Dispose();
                cmd.Dispose();
                //    da.Dispose();
            }
            return isSuccess;
        }
        //数据分析 获取数据表需要的数据
        private static void Get_TableField()
        { }

        public static DataSet GetDataset(string connectionstring, string cmd)
        {
            try
            {
                using (OleDbConnection con = new OleDbConnection(connectionstring))
                {
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = con;
                    command.CommandText = cmd;
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return ds;
                }
            }
            catch
            {
                return new DataSet();
            }
        }



        #region 公用方法
        /// <summary>   
        /// 判断是否存在某表的某个字段   
        /// </summary>   
        /// <param name="tableName">表名称</param>   
        /// <param name="columnName">列名称</param>   
        /// <returns>是否存在</returns>   
        public static bool ColumnExists(string tableName, string columnName)
        {
            string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
            object res = GetSingle(sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>   
        /// 表是否存在   
        /// </summary>   
        /// <param name="TableName"></param>   
        /// <returns></returns>   
        public static bool TabExists(string TableName)
        {
            string strsql = "select count(*) from SYSOBJECTS where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            //string strsql = "SELECT count(*) FROM SYS.OBJECTS WHERE object_id = OBJECT_ID(N"[dbo].[" + TableName + "]") AND type in (N"U")";   
            object obj = GetSingle(strsql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
        #region  执行简单SQL语句
        /// <summary>   
        /// 执行SQL语句，返回影响的记录数   
        /// </summary>   
        /// <param name="SQLString">SQL语句</param>   
        /// <returns>影响的记录数</returns>   
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = 5 * 60;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        public static int ExecuteSqlByTime(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        /// <summary>   
        /// 执行Sql和Oracle的混合事务   
        /// </summary>   
        /// <param name="list">SQL命令行列表</param>   
        /// <param name="oracleCmdSqlList">Oracle命令行列表</param>   
        /// <returns>执行结果 0-由于SQL造成事务失败 -1 由于Oracle造成事务失败 1-整体事务执行成功</returns>   
        public static int ExecuteSqlTran(List<CommandInfo> list, List<CommandInfo> oracleCmdSqlList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (CommandInfo myDE in list)
                    {
                        string cmdText = myDE.CommandText;
                        SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
                        PrepareCommand(cmd, conn, tx, cmdText, cmdParms, CommandType.Text);
                        if (myDE.EffentNextType == EffentNextType.SolicitationEvent)
                        {
                            if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                            {
                                tx.Rollback();
                                throw new Exception("违背要求" + myDE.CommandText + "必须符合select count(..的格式");
                                //return 0;   
                            }
                            object obj = cmd.ExecuteScalar();
                            bool isHave = false;
                            if (obj == null && obj == DBNull.Value)
                            {
                                isHave = false;
                            }
                            isHave = Convert.ToInt32(obj) > 0;
                            if (isHave)
                            {
                                //引发事件   
                                myDE.OnSolicitationEvent();
                            }
                        }
                        if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
                        {
                            if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "必须符合select count(..的格式");
                                //return 0;   
                            }
                            object obj = cmd.ExecuteScalar();
                            bool isHave = false;
                            if (obj == null && obj == DBNull.Value)
                            {
                                isHave = false;
                            }
                            isHave = Convert.ToInt32(obj) > 0;
                            if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "返回值必须大于0");
                                //return 0;   
                            }
                            if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "返回值必须等于0");
                                //return 0;   
                            }
                            continue;
                        }
                        int val = cmd.ExecuteNonQuery();
                        if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                        {
                            tx.Rollback();
                            throw new Exception("SQL:违背要求" + myDE.CommandText + "必须有影响行");
                            //return 0;   
                        }
                        cmd.Parameters.Clear();
                    }
                    //string oraConnectionString = PubConstant.GetConnectionString("ConnectionStringPPC");   
                    //bool res = OracleHelper.ExecuteSqlTran(oraConnectionString, oracleCmdSqlList);   
                    //if (!res)   
                    //{   
                    //    tx.Rollback();   
                    //    throw new Exception("Oracle执行失败");   
                    // return -1;   
                    //}   
                    tx.Commit();
                    return 1;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    tx.Rollback();
                    throw e;
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    throw e;
                }
            }
        }
        /// <summary>   
        /// 执行多条SQL语句，实现数据库事务。   
        /// </summary>   
        /// <param name="SQLStringList">多条SQL语句</param>        
        public static int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>   
        /// 执行带一个存储过程参数的的SQL语句。   
        /// </summary>   
        /// <param name="SQLString">SQL语句</param>   
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>   
        /// <returns>影响的记录数</returns>   
        public static int ExecuteSql(string SQLString, string content)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>   
        /// 执行带一个存储过程参数的的SQL语句。   
        /// </summary>   
        /// <param name="SQLString">SQL语句</param>   
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>   
        /// <returns>影响的记录数</returns>   
        public static object ExecuteSqlGet(string SQLString, string content)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>   
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)   
        /// </summary>   
        /// <param name="strSQL">SQL语句</param>   
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>   
        /// <returns>影响的记录数</returns>   
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>   
        /// 执行一条计算查询结果语句，返回查询结果（object）。   
        /// </summary>   
        /// <param name="SQLString">计算查询结果语句</param>   
        /// <returns>查询结果（object）</returns>   
        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        public static object GetSingle(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        /// <summary>   
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )   
        /// </summary>   
        /// <param name="strSQL">查询语句</param>   
        /// <returns>SqlDataReader</returns>   
        public static SqlDataReader ExecuteReader(string strSQL)
        {
            return ExecuteReader(strSQL, CommandType.Text, null);
        }
        public static SqlDataReader ExecuteReader(string strSQL, CommandType cmdType, params SqlParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(strSQL, connection);
            cmd.CommandType = cmdType;
            if (parameters != null && parameters.Length > 0)
            {
                foreach (SqlParameter meter in parameters)
                {
                    if (meter.Value == null)
                    {
                        meter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(meter);
                }
            }
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }
        }
        /// <summary>   
        /// 执行查询语句，返回DataSet   
        /// </summary>   
        /// <param name="SQLString">查询语句</param>   
        /// <returns>DataSet</returns>   
        public static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        /// <summary>   
        /// 查询并得到数据集DataSet   
        /// </summary>   
        /// <param name="SQLString"></param>   
        /// <param name="Times"></param>   
        /// <returns></returns>   
        public static DataSet Query(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        #endregion
        #region 执行带参数的SQL语句
        /// <summary>   
        /// 执行SQL语句，返回影响的记录数   
        /// </summary>   
        /// <param name="SQLString">SQL语句</param>   
        /// <returns>影响的记录数</returns>   
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms, CommandType.Text);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }
        /// <summary>   
        /// 执行多条SQL语句，实现数据库事务。   
        /// </summary>   
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>   
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环   
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms, CommandType.Text);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>   
        /// 执行多条SQL语句，实现数据库事务。   
        /// </summary>   
        /// <param name="SQLStringList">SQL语句的List表</param>   
        /// <param name="cmdType">Command类型</param>
        public static int ExecuteSqlTran(List<CommandInfo> cmdList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int count = 0;
                        //循环   
                        foreach (CommandInfo myDE in cmdList)
                        {
                            string cmdText = myDE.CommandText;
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms, myDE.ComType);
                            if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
                            {
                                if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                object obj = cmd.ExecuteScalar();
                                bool isHave = false;
                                if (obj == null && obj == DBNull.Value)
                                {
                                    isHave = false;
                                }
                                isHave = Convert.ToInt32(obj) > 0;
                                if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                continue;
                            }
                            int val = cmd.ExecuteNonQuery();
                            count += val;
                            if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                            {
                                trans.Rollback();
                                return 0;
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return count;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>   
        /// 执行多条SQL语句，实现数据库事务。   
        /// </summary>   
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>   
        public static void ExecuteSqlTranWithIndentity(List<CommandInfo> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环   
                        foreach (CommandInfo myDE in SQLStringList)
                        {
                            string cmdText = myDE.CommandText;
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms, CommandType.Text);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>   
        /// 执行多条SQL语句，实现数据库事务。   
        /// </summary>   
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>   
        public static void ExecuteSqlTranWithIndentity(Hashtable SQLStringList)
        {
            using (SqlConnection conn = Connection())
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环   
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>   
        /// 执行一条计算查询结果语句，返回查询结果（object）。   
        /// </summary>   
        /// <param name="SQLString">计算查询结果语句</param>   
        /// <returns>查询结果（object）</returns>   
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }
        /// <summary>   
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )   
        /// </summary>   
        /// <param name="strSQL">查询语句</param>   
        /// <returns>SqlDataReader</returns>   
        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            return ExecuteReader(SQLString, CommandType.Text, cmdParms);
        }
        /// <summary>   
        /// 执行查询语句，返回DataSet   
        /// </summary>   
        /// <param name="SQLString">查询语句</param>   
        /// <returns>DataSet</returns>   
        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet   
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="start">起始记录</param>
        /// <param name="size">最大的记录条数</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, int start, int size, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, start, size, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms, CommandType cmdType)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            PrepareCommand(cmd, conn, trans, cmdText, cmdParms, CommandType.Text);
        }
        #endregion

        #region 存储过程操作

        /// <summary>
        /// 执行无参数无返回的存储过程
        /// </summary>
        /// <param name="storedProcName"></param>
        public static void RunProcNoParametersAndNoReturn(string storedProcName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(storedProcName, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>   
        /// 执行存储过程，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )   
        /// </summary>   
        /// <param name="storedProcName">存储过程名</param>   
        /// <param name="parameters">存储过程参数</param>   
        /// <returns>SqlDataReader</returns>   
        public static SqlDataReader RunProcedure(string storedProcName, params IDataParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataReader returnReader;
            connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
        }
        /// <summary>   
        /// 执行存储过程   
        /// </summary>   
        /// <param name="storedProcName">存储过程名</param>   
        /// <param name="parameters">存储过程参数</param>   
        /// <param name="tableName">DataSet结果中的表名</param>   
        /// <returns>DataSet</returns>   
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }
        /// <summary>   
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)   
        /// </summary>   
        /// <param name="connection">数据库连接</param>   
        /// <param name="storedProcName">存储过程名</param>   
        /// <param name="parameters">存储过程参数</param>   
        /// <returns>SqlCommand</returns>   
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 60 * 30;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.   
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }
        /// <summary>   
        /// 执行存储过程，返回影响的行数       
        /// </summary>   
        /// <param name="storedProcName">存储过程名</param>   
        /// <param name="parameters">存储过程参数</param>   
        /// <param name="rowsAffected">影响的行数</param>   
        /// <returns></returns>   
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int result;
                connection.Open();
                SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();   
                return result;
            }
        }
        /// <summary>   
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)    
        /// </summary>   
        /// <param name="storedProcName">存储过程名</param>   
        /// <param name="parameters">存储过程参数</param>   
        /// <returns>SqlCommand 对象实例</returns>   
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion


        //操作的数据
        //account_Rec      公司帐号标识
        //sub_User_Rec     操作用户标识
        //pro_Name        存储过程名称
        //rightKey         功能权限
        //contect          传入存储过程的数据
        //ds               调用存储过程返回的数据集
        //result           调用存储过程返回的字符串
        //public bool opt_BaseProData(int account_Rec, int sub_User_Rec, string pro_Name, string ip, string rightKey, string context, DataSet ds, out string result)
        //{
        //    bool isSuccess = true;
        //    result = "";
        //    if (pro_Name.Trim().Length == 0)
        //    {
        //        //LogInfor.attenText("存储过程名称为空", "Data_Exchange.opt_Data", "调用通用的存储过程!");
        //        return false;
        //    }

        //    SqlConnection con = null;
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    try
        //    {
        //        //优化条件
        //        var condition = DataConvert.getvaluebyname("qcondition", context);
        //        if (!string.IsNullOrWhiteSpace(condition))
        //        {
        //            condition = DBHelper.OptimizeQueryCondition(condition);
        //            DataConvert.setvaluebyname("qcondition", condition, context);
        //        }

        //        da.SelectCommand = cmd;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = pro_Name;

        //        cmd.Parameters.Add("@Account_Rec", SqlDbType.Int);
        //        cmd.Parameters.Add("@Sub_User_Rec", SqlDbType.Int);
        //        cmd.Parameters.Add("@RightKey", SqlDbType.VarChar);
        //        cmd.Parameters.Add("@IP", SqlDbType.VarChar);

        //        cmd.Parameters.Add("@Context", SqlDbType.VarChar);

        //        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.VarChar, 500, ParameterDirection.Output, false, 8, 2, "", DataRowVersion.Current, ""));

        //        cmd.Parameters["@Account_Rec"].Value = account_Rec;
        //        cmd.Parameters["@Sub_User_Rec"].Value = sub_User_Rec;
        //        cmd.Parameters["@RightKey"].Value = rightKey;
        //        cmd.Parameters["@IP"].Value = ip;
        //        cmd.Parameters["@context"].Value = context;
        //        cmd.CommandTimeout = 60 * 60;

        //        con = Connection();
        //        con.Open();
        //        cmd.Connection = con;
        //        da.Fill(ds);
        //        result = cmd.Parameters["@result"].Value.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = string.Format("执行\"exec {0} '{1}','{2}','{3}','{4}','{5}','{6}'\"失败，\r\n详情:{7}", pro_Name, account_Rec, sub_User_Rec, rightKey, ip, context.Replace("'", "''"), result, ex.Message);
        //        IoCContainer.Current.GetInstance<ILogger>().WriteException(LogLevel.Error, new Exception(message, ex));

        //        isSuccess = false;
        //        result = "op_Result:0***errorInfo:" + ex.Message;
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Dispose();
        //            cmd.Dispose();
        //            da.Dispose();
        //        }
        //    }
        //    //op_Result
        //    if (result.Trim().Length > 0)
        //    {
        //        if (DataConvert.getvaluebyname("op_Result", result) == "1")
        //        {
        //            isSuccess = true;
        //            if (DataConvert.getvaluebyname("total_records", result) == "0")
        //            {
        //                if (ds.Tables.Count > 0)
        //                {
        //                    result = DataConvert.setvaluebyname("total_records", ds.Tables[0].Rows.Count.ToString(), result);
        //                }
        //            }
        //        }
        //        else if (DataConvert.getvaluebyname("op_Result", result) == "0")
        //        {
        //            isSuccess = false;
        //        }
        //        result = DataConvert.delItemByName("op_Result", result);
        //    }

        //    return isSuccess;
        //}
    }
}
