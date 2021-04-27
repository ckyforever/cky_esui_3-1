using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using MySql.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Text;

namespace Webjdatademo
{
    public class datamysql
    {
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.		
        public static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];//PubConstant.ConnectionString;     		
        public datamysql()
        {
        }


        #region 公用方法
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
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
        public static bool Exists(string strSql, params MySql.Data.MySqlClient.MySqlParameter[] cmdParms)
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
        public static void ExecuteSql(string SQLString)
        {  
            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        //cmd.CommandText = "SELECT @@IDENTITY";
                        //int rows = (int)cmd.ExecuteScalar();

                        //return rows;
                    }
                    catch (System.Data.OleDb.OleDbException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
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
            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SQLString, connection);
                MySql.Data.MySqlClient.MySqlParameter myParameter = new MySql.Data.MySqlClient.MySqlParameter("@content", MySql.Data.MySqlClient.MySqlDbType.VarChar);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT @@IDENTITY";
                    int rows = (int)cmd.ExecuteScalar();
                    return rows;
                }
                catch (System.Data.OleDb.OleDbException E)
                {
                    throw new Exception(E.Message);
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
            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, connection);
                MySql.Data.MySqlClient.MySqlParameter myParameter = new MySql.Data.MySqlClient.MySqlParameter("@fs", MySql.Data.MySqlClient.MySqlDbType.Binary);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT @@IDENTITY";
                    int rows = (int)cmd.ExecuteScalar();
                    return rows;
                }
                catch (System.Data.OleDb.OleDbException E)
                {
                    throw new Exception(E.Message);
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
            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SQLString, connection))
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
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回MySql.Data.MySqlClient.MySqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>MySql.Data.MySqlClient.MySqlDataReader</returns>
        public static MySql.Data.MySqlClient.MySqlDataReader ExecuteReader(string strSQL)
        {
            MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, connection);
            try
            { 
                connection.Open();
                MySql.Data.MySqlClient.MySqlDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                { 
                    connection.Open();
                    MySql.Data.MySqlClient.MySqlDataAdapter command = new MySql.Data.MySqlClient.MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.OleDb.OleDbException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        public static DataTable getdatatable(string SQLString)
        {
            DataSet ds = new DataSet(); 
            ds = Query(SQLString);
            return ds.Tables[0];
        }

        public static string getvalue(string SQLString)
        {
            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                string str = "";
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySql.Data.MySqlClient.MySqlDataAdapter command = new MySql.Data.MySqlClient.MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.OleDb.OleDbException ex)
                {
                    throw new Exception(ex.Message);
                }
                if (ds.Tables[0].Rows.Count > 0)
                    str = ds.Tables[0].Rows[0][0].ToString();
                return str;
            }
        }
        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params MySql.Data.MySqlClient.MySqlParameter[] cmdParms)
        {
            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT @@IDENTITY";
                        int rows = (int)cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.OleDb.OleDbException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的MySql.Data.MySqlClient.MySqlParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySql.Data.MySqlClient.MySqlTransaction trans = conn.BeginTransaction())
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            MySql.Data.MySqlClient.MySqlParameter[] cmdParms = (MySql.Data.MySqlClient.MySqlParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                        }
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
        public static object GetSingle(string SQLString, params MySql.Data.MySqlClient.MySqlParameter[] cmdParms)
        {
            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
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
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回MySql.Data.MySqlClient.MySqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>MySql.Data.MySqlClient.MySqlDataReader</returns>
        public static MySql.Data.MySqlClient.MySqlDataReader ExecuteReader(string SQLString, params MySql.Data.MySqlClient.MySqlParameter[] cmdParms)
        {
            MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                MySql.Data.MySqlClient.MySqlDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params MySql.Data.MySqlClient.MySqlParameter[] cmdParms)
        {
            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.OleDb.OleDbException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(MySql.Data.MySqlClient.MySqlCommand cmd, MySql.Data.MySqlClient.MySqlConnection conn, MySql.Data.MySqlClient.MySqlTransaction trans, string cmdText, MySql.Data.MySqlClient.MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (MySql.Data.MySqlClient.MySqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }


        #endregion


        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="strsql">读的sql语句</param>
        /// <param name="pagenum">第几页</param>
        /// <param name="pagerecord">每页的记录数</param>
        /// <returns>返回分页的sql字符串</returns>
        public static string createsql(string strsql, int pagenum, int pagerecord)
        {
            pagenum = pagenum - 1;
            if (pagenum < 0)
                pagenum = 0;
            string s1 = "", s2 = "", s3 = "", s4 = "", s5 = "", orderstr = "";//,orderstr
            int n1, n2, n3, n4, tempnum;
            string str1, str2, str3;
            strsql = strsql.ToLower();
            strsql = strsql.Replace("order  by", "order by").Replace("order  by", "order by");

            if ((strsql.IndexOf("order by") < 0))
                strsql = strsql + " order by id desc ";
            else
            {
                s1 = strsql.Substring(strsql.IndexOf("order by"));
                if (s1.IndexOf("id") < 0)
                    strsql = strsql + ",id DESC";
            }
            //给条件加个括号。
            if ((strsql.IndexOf("where") > 0) && (strsql.IndexOf("order") > 0))
            {
                str1 = strsql.Substring(0, strsql.IndexOf("where") + 6);
                str2 = strsql.Substring(strsql.IndexOf("where") + 6);
                str2 = str2.Substring(0, str2.IndexOf("order"));
                str3 = strsql.Substring(strsql.IndexOf("order"));
                strsql = str1 + " (" + str2 + ") " + str3;
            }
            else if (strsql.IndexOf("where") > 0)
            {
                str1 = strsql.Substring(0, strsql.IndexOf("where") + 6);//where 之前的，包括where
                str2 = strsql.Substring(strsql.IndexOf("where") + 6);  //where 之后的，不包括where
                strsql = str1 + " (" + str2 + ")";
            }


            strsql = strsql.TrimStart();
            tempnum = pagenum * pagerecord;
            n1 = strsql.IndexOf("select") + 6;//n1 读出select后的开始位置
            n2 = strsql.IndexOf("where");//n2 读出where前开始的位置
            n3 = strsql.IndexOf("from");//n3 读出from前开始的位置
            n4 = strsql.IndexOf("order"); //n4 读出order前开始的位置

            if (pagenum == 0)//如果是第一页，则只须读出前面的pagerecord条记录。
            {

                n2 = strsql.Length;
                s1 = strsql.Substring(6);

                s4 = "select  top  " + pagerecord.ToString() + " " + s1;
            }
            else if ((n2 == -1) && (n4 == -1))//没有where、 order
            {
                n2 = strsql.Length;
                s1 = strsql.Substring(0, n1);
                s2 = " " + strsql.Substring(n1, n2 - n1) + " ";
                s5 = " " + strsql.Substring(n3, n2 - n3) + " ";//from dt
                orderstr = "order by id";//读出order及其后的字符串
                //				s4 = "select distinct top  "+pagerecord.ToString()+s2 +" WHERE (ID NOT IN (SELECT TOP "+tempnum.ToString()+" id "+s5+" "+orderstr+")  as mmbb ) " ;
                s4 = "select    top  " + pagerecord.ToString() + s2 + " WHERE (id NOT IN (select id from (SELECT TOP " + tempnum.ToString() + " id " + s5 + ") as mmbb) ) ";

            }
            else if ((n2 == -1) && (n4 != -1))//没有where 只有 order,如： select a,b from dt order by a
            {
                n2 = strsql.IndexOf("order"); //
                s1 = strsql.Substring(0, n1);//读出 select
                s2 = " " + strsql.Substring(n1, n2 - n1) + " ";//读出: a,b from dt
                s5 = " " + strsql.Substring(n3, n2 - n3) + " ";//读出:from dt
                orderstr = " " + strsql.Substring(strsql.IndexOf("order"));//读出order及其后的字符串:order by a
                // select top 10 a,b from dt where(id not in(select id from (select top 2*10 a,b from dt order by a) ) ) ;
                //				s4 = "select  top  "+pagerecord.ToString()+s2 +" WHERE (ID NOT IN (select id from (SELECT TOP "+tempnum.ToString()+s2+" "+orderstr+") as mmbb )  "+") "+orderstr ;		
                s4 = "select    top  " + pagerecord.ToString() + s2 + " WHERE (id NOT IN (select id from (SELECT TOP " + tempnum.ToString() + " id " + s5 + " " + orderstr + ") as mmbb) ) " + orderstr;

            }
            else if ((n2 != -1) && (n4 == -1))//只有where 没有 order 如： select a,b from dt where a>5
            {
                n4 = strsql.Length;
                s1 = strsql.Substring(0, n1);//读出select 如：select
                s2 = " " + strsql.Substring(n1, n2 - n1) + " ";//读出select和 where这间的字符串，如：　a,b from dt 
                s3 = " " + strsql.Substring(n2, n4 - n2) + " ";//读出where包括where 和 order这间的字符串,如：　where a>5
                s5 = " " + strsql.Substring(n3, n2 - n3) + " ";//读出from 表名 ,如： from dt
                orderstr = "order by id";//读出order及其后的字符串
                // select  top 10 a,b from dt where (id not in( select top 2*10 id from dt where a>5 order by a) and  a>5 order by a)
                //				s4 = "select distinct top  "+pagerecord.ToString()+s2 +" WHERE (ID NOT IN (SELECT TOP "+tempnum.ToString()+" id "+s5+s3+" "+orderstr+")  as mmbb) and "+s3.Substring(6)+") "+orderstr  ;
                s4 = "select  top  " + pagerecord.ToString() + s2 + " WHERE (id NOT IN (select id from (SELECT TOP " + tempnum.ToString() + " id " + s5 + s3 + " " + orderstr + ") as mmbb) and " + s3.Substring(6) + ") " + orderstr;

            }
            else//都有where order 如： select a,b from dt where a>5 order by a
            {
                s1 = strsql.Substring(0, n1);//读出select 如：select
                s2 = " " + strsql.Substring(n1, n2 - n1) + " ";//读出select和 where这间的字符串 a,b from dt 
                s3 = " " + strsql.Substring(n2, n4 - n2) + " ";//读出where包括where 和 order这间的字符串 where a>5
                s5 = " " + strsql.Substring(n3, n2 - n3) + " ";//读出from 表名:                       from dt
                orderstr = " " + strsql.Substring(strsql.IndexOf("order"));//读出order及其后的字符串 order by a

                // select  top 10 a,b from dt where (id not in(select id from (select top 20 a,b from dt where a>5 order by a) as mmbb) and a>5) order by a
                s4 = "select  top  " + pagerecord.ToString() + s2 + " WHERE (id NOT IN (select id from (SELECT TOP " + tempnum.ToString() + " id " + s5 + s3 + " " + orderstr + ") as mmbb) and " + s3.Substring(6) + ") " + orderstr;

            }
            return s4;

        }

        #region  读取appsetting
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            string CacheKey = "AppSettings-" + key;
            object objModel = GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = ConfigurationManager.AppSettings[key];
                    if (objModel != null)
                    {
                        SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                    }
                }
                catch
                { }
            }
            return objModel.ToString();
        }

        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            bool result = false;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = bool.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }
            return result;
        }
        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = decimal.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }
        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            int result = 0;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = int.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }

        #endregion

        #region 按分页读取数据库，并转换为json
        public static string GetListByPageColumns(string columslist, string strWhere, string orderby, int PageSize, int PageIndex, string datatablename)
        {
            if (orderby.Trim() == "")
                orderby = " order by id desc";
            else
                orderby = " order by " + orderby;
            string strwhere1 = "";
            if (strWhere != "")
            {
                strwhere1 = " where " + strWhere;
                strWhere = " and " + strWhere;
            }
            string strsql = "";
            int topnum = (PageIndex - 1) * PageSize;
            string mylimit = " LIMIT " + topnum + "," + PageSize;
            //if (topnum <= 0)
            strsql = "select   " + columslist + " from " + datatablename + " " + strwhere1 + orderby + mylimit;

            
            return strsql;

        }
 
        public static string GetListByPageColumn_tojson(string columslist, string strWhere, string orderby, string datatablename)
        {
            if (orderby.Trim() == "")
                orderby = " order by id desc";
            else
                orderby = " order by " + orderby;
            string strwhere1 = "";
            if (strWhere != "")
            {
                strwhere1 = " where " + strWhere;
                strWhere = " and " + strWhere;
            }
            string strsql = "";


           strsql = "select  " + columslist + " from " + datatablename + " " + strwhere1 + orderby;
            //return strsql;
            DataSet ds = new DataSet();
            ds = Query(strsql);

            string str = ToJson_combox(ds);//?
            return str;
        }

        public static string GetListByPageColumn_tojson(HttpContext thiscontext, string columslist, string strWhere, string orderstr, string datatablename)
        {
            int PageIndex = 1;
            int PageSize = 20;
            if (thiscontext != null)
            {
                PageIndex = thiscontext.Request.Form["page"] != "" ? Convert.ToInt32(thiscontext.Request.Form["page"]) : 0;
                PageSize = thiscontext.Request.Form["rows"] != "" ? Convert.ToInt32(thiscontext.Request.Form["rows"]) : 0;
                string sort = thiscontext.Request.Form["sort"] != "" ? thiscontext.Request.Form["sort"] : "";
                string order = thiscontext.Request.Form["order"] != "" ? thiscontext.Request.Form["order"] : "";
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 20;

            }

            string str = "(1 = 1)";


            string searchType = thiscontext.Request.Form["search_type"] != "" ? thiscontext.Request.Form["search_type"] : string.Empty;
            string searchValue = thiscontext.Request.Form["search_value"] != "" ? thiscontext.Request.Form["search_value"] : string.Empty;

            if (searchType != null)
                searchType = searchType.Replace(" ", "").Replace("'", "");
            if (searchValue != null)
                searchValue = searchValue.Replace(" ", "").Replace("'", "");

            if (searchType != null && searchValue != null)
            {
                str = searchType + " like '%" + searchValue + "%'";
            }
            if (strWhere != "")
                strWhere = strWhere  +" and "+ str;
            else
                strWhere = str;
            return GetListByPageColumns_tojson(columslist, strWhere, orderstr, PageSize, PageIndex, datatablename);
        }


        public static string GetListByPageColumns_tojson(string columslist, string strWhere, string orderby, int PageSize, int PageIndex, string datatablename)
        {
            //string mylimit = " LIMIT 0," + PageSize;
            if (orderby.Trim() == "")
                orderby = " order by Id desc";
            else
                orderby = " order by " + orderby;
            string strwhere1 = "";
            if (strWhere != "")
            {
                strwhere1 = " where " + strWhere;
                strWhere = " and " + strWhere;
            }
            string strsql = "";
            int topnum = (PageIndex - 1) * PageSize;
            string mylimit = " LIMIT " + topnum + "," + PageSize;
            //if (topnum <= 0)
            strsql = "select   " + columslist + " from " + datatablename + " " + strwhere1 + orderby + mylimit;
            //else
            //strsql = "select  " + columslist + " from  " + datatablename + "  where id >= (select   id from  " + datatablename + "  " + strwhere1 + orderby + " limit " + topnum + ",1) " + strWhere + orderby + " limit " + PageSize;
            //return strsql;
            DataSet ds = new DataSet();

         
            ds = Query(strsql);

            string allcount = "0";
            
            allcount = getvalue("select count(*) from  " + datatablename + strwhere1);

            if (allcount == "")
                allcount = "0";
            string str = ToJson(ds, allcount);//?
            return str;
        }


        public static string ToJson(DataSet ds, string allcount)
        {
            DataTable dt = ds.Tables[0];
            StringBuilder jsonString = new StringBuilder();
            //
            //TODO:total表示记录的总数
            //
            jsonString.Append("{\"total\":" + allcount + ",\"rows\":[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = String.Format(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append("\"" + strValue + "\",");
                    }
                    else
                    {
                        jsonString.Append("\"" + strValue + "\"");
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]}");
            return jsonString.ToString();
        
        }

        public static string ToJson_combox(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            StringBuilder jsonString = new StringBuilder();
            //
            //TODO:total表示记录的总数
            //
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = String.Format(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append("\"" + strValue + "\",");
                    }
                    else
                    {
                        jsonString.Append("\"" + strValue + "\"");
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>
        /// 组合搜索条件
        /// </summary>
        /// <returns></returns>
        public static string GetWhere(HttpContext context)
        {

            string str = "(1=1)";
            string searchType = context.Request.Form["search_type"] != "" ? context.Request.Form["search_type"] : string.Empty;
            string searchValue = context.Request.Form["search_value"] != "" ? context.Request.Form["search_value"] : string.Empty;

            if (searchType != null && searchValue != null)
            {
                str = searchType + " like '%" + searchValue + "%'";

            }
            return str;
        }
        #endregion
    }
}