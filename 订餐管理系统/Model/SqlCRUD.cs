using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using 订餐管理系统.Exception;

namespace 订餐管理系统.Model
{
    class SqlCRUD:SqlHelper
    {
        public SqlCRUD(string connectionStr):base(connectionStr)
        {
        }

        /// <summary>
        /// 向指定数据库表插入数据
        /// </summary>
        /// <param name="table">数据表名</param>
        /// <param name="parameters">需要插入的参数名列表</param>
        /// <param name="values">插入数据值列表</param>
        public void InsertData(string table,string[] parameters,string[] values)
        {
            if (string.IsNullOrEmpty(table) || parameters == null || values == null)
            {
                throw new ArgumentNullException("参数不能为null");
            }
            else if (parameters.Length != values.Length)
            {
                throw new NotEqualException("参数对和值对数量不相同");
            }
            else
            {
                string paramas = parameters.SplitArrayToString(",");
                string _values = values.SplitArrayToString("'",",");
                string sql = "Insert into " + table+"("+paramas+")values("+_values+")";
                try
                {
                    this.ExcuteSql(sql);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    this.Dispose();
                }
            }
        }

        /// <summary>
        /// 更新指定数据表指定数据
        /// </summary>
        /// <param name="table">数据表名</param>
        /// <param name="parameters">参数名列表</param>
        /// <param name="values">数据值列表</param>
        /// <param name="filter">筛选语句,例如Name='Mike'</param>
        public void UpdateData(string table, string[] parameters, string[] values, string filter)
        {
            //update table1 set field1=value1 where 范围
            if (string.IsNullOrEmpty(table) || parameters == null || values == null || string.IsNullOrEmpty(filter))
            {
                throw new ArgumentNullException("参数不能为null");
            }
            else if (parameters.Length != values.Length)
            {
                throw new NotEqualException("参数对和值对数量不相同");
            }
            else
            {
                StringBuilder UpdateSql = new StringBuilder();
                UpdateSql.Append("update " + table + " set ");
                #region SQL语句拼接
                for (int i = 0; i < parameters.Length - 1; i++)
                {
                    UpdateSql.Append(parameters[i]);
                    UpdateSql.Append("=");
                    UpdateSql.Append("'");
                    UpdateSql.Append(values[i]);
                    UpdateSql.Append("'");
                    UpdateSql.Append(",");
                }
                UpdateSql.Append(parameters[parameters.Length - 1]);
                UpdateSql.Append("=");
                UpdateSql.Append("'");
                UpdateSql.Append(values[parameters.Length - 1]);
                UpdateSql.Append("'  where ");
                UpdateSql.Append(filter);
                #endregion
                try
                {
                    this.ExcuteSql(UpdateSql.ToString());
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 删除表中所有行
        /// </summary>
        /// <param name="table">数据表名</param>
        public void DeleteData(string table)
        {
            if (string.IsNullOrEmpty(table))
            {
                throw new ArgumentNullException("数据表明不能为null或空字符串");
            }
            else
            {
                string deleteSql = "DELETE * FROM " + table;
                try
                {
                    this.ExcuteSql(deleteSql);
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 按制定规则删除表中相关行
        /// </summary>
        /// <param name="table">数据表名</param>
        /// <param name="filter">指定规则,例如:Name='John'</param>
        public void DeleteData(string table, string filter)
        {
            if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(filter))
            {
                throw new ArgumentNullException("参数不能为null或空字符串");
            }
            else
            {
                string deleteSql = "DELETE * FROM " + table+" WHERE "+filter;
            }
        }

        /// <summary>
        /// 获取数据库指定表中指定参数列表的值
        /// </summary>
        /// <param name="table">数据表名</param>
        /// <param name="parameters">参数名列表</param>
        /// <returns>返回查询结果</returns>
        public DataTable SelectData(string table, string[] parameters)
        {
            return this.SelectData(table, parameters, null);
        }
        /// <summary>
        /// 根据查找筛选规则,获取数据库指定表中指定参数列表的值
        /// </summary>
        /// <param name="table">数据表名</param>
        /// <param name="parameters">参数名列表</param>
        /// <param name="filter">指定查找筛选规则,例如:Name='John'</param>
        /// <returns>返回查询结果</returns>
        public DataTable SelectData(string table, string[] parameters, string filter)
        {
            if (string.IsNullOrEmpty(table) || parameters == null)
            {
                throw new ArgumentNullException("参数不能为null");
            }
            else
            {
                string paramas = parameters.SplitArrayToString(",");
                string selectSql = string.Empty;
                if (string.IsNullOrEmpty(filter))
                {
                    selectSql = "select " + paramas + " from " + table;
                }
                else
                {
                    selectSql = "select " + paramas + " from " + table+" where "+filter;
                }
                try
                {
                    DataTable dt = null;
                    this.ExcuteSql(selectSql, out dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
