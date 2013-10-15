using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace 订餐管理系统.Model
{
    class SqlHelper:IDisposable
    {
        private SqlConnection sqlconc;

        private string _strConnec;
        public string ConnectionString
        {
            get
            {
                return this._strConnec;
            }
        }
        /// <summary>
        /// 构造函数初始化连接参数
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlHelper(string connectionString)
        {
            this._strConnec = connectionString;
            sqlconc = new SqlConnection(this._strConnec);
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        public void Connect()
        {
            if (sqlconc.State == ConnectionState.Broken)
            {
                sqlconc.Close();
                sqlconc.Open();
            }
            else if (sqlconc.State == ConnectionState.Closed)
            {
                sqlconc.Open();
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 断开数据库连接
        /// </summary>
        public void DisConnect()
        {
            if (sqlconc.State == ConnectionState.Open)
            {
                sqlconc.Close();
            }
            else
            {
                return;
            }
        }

        private SqlCommand CreatSqlCommand(string sql)
        {
            SqlCommand sqlcmd = new SqlCommand(sql, this.sqlconc);
            return sqlcmd;
        }
        /// <summary>
        /// 执行指定Sql语句,不带数据返回。一般用于非Select查询语句
        /// </summary>
        /// <param name="sql">需要执行的sql语句</param>
        public void ExcuteSql(string sql)
        {
            this.Connect();
            using (SqlCommand sqlcmd = CreatSqlCommand(sql))
            {
                sqlcmd.ExecuteNonQuery();
            }
            this.DisConnect();
        }

        /// <summary>
        /// 执行指定Sql语句，并返回一个SqlDataReader流，一般用于获取Select查询语句结果
        /// </summary>
        /// <param name="sql">需要执行的sql语句</param>
        /// <param name="data">传入一个SqlDataReader</param>
        public void ExcuteSql(string sql, out SqlDataReader data)
        {
            data = null;
            this.Connect();
            using (SqlCommand sqlcmd = CreatSqlCommand(sql))
            {
                data = sqlcmd.ExecuteReader();
            }
            //this.DisConnect();
        }

        /// <summary>
        /// 执行指定Sql语句，并将结果存放于DataTable中。一般用于获取Select查询语句结果
        /// </summary>
        /// <param name="sql">需要执行的sql语句</param>
        /// <param name="dt">传入一个DataTable用于获取返回结果</param>
        public void ExcuteSql(string sql, out DataTable dt)
        {
            try
            {
                DataSet ds = new DataSet();
                dt = new DataTable();
                this.Connect();
                using (SqlCommand sqlcmd = CreatSqlCommand(sql))
                {
                    SqlDataAdapter sqlddp = new SqlDataAdapter(sqlcmd);
                    sqlddp.Fill(ds);
                    dt = ds.Tables[0];
                }
            }
            catch
            {
                dt = null;
            }
            finally
            {
                this.DisConnect();
            }
        }

        ~SqlHelper()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (sqlconc != null)
            {
                this.DisConnect();
                sqlconc.Dispose();
            }
        }
    }
}
