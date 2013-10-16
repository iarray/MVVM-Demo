using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MVVM;
using 订餐管理系统.Model;

namespace 订餐管理系统.ViewModels
{
    class OrdersInfoPageViewModel:NotificationObject
    {
        /// <summary>
        /// 订单数据表
        /// </summary>
        private DataTable _ordersData;

        public DataTable OrdersData
        {
            get
            {
                return this._ordersData;
            }
        }

        /// <summary>
        /// 选中订单的索引
        /// </summary>
        private int _orderSelectIndex;

        public int OrderSelectIndex
        {
            get
            {
                return this._orderSelectIndex;
            }
            set
            {
                this._orderSelectIndex = value;
                this.OnPropertyChanged("OrderSelectIndex");
                this.SelectOrderInfo = this.GetSelectOrderInfo(this._orderSelectIndex);
            }
        }
        /// <summary>
        /// 选中订单的详细信息
        /// </summary>
        private string _selectOrderInfo;
        public string SelectOrderInfo
        {
            get
            {
                return this._selectOrderInfo;
            }
            set
            {
                this._selectOrderInfo = value;
                this.OnPropertyChanged("SelectOrderInfo");
            }
        }

        public OrdersInfoPageViewModel()
        {
            using (SqlCRUD sqlCRUD = new SqlCRUD(SqlHelper.MyConnectionString))
            {
                _ordersData = sqlCRUD.SelectData("Orders", new string[] { "ID", "Money", "Time" });
            }
            OrderSelectIndex = 0;
        }

        public string GetSelectOrderInfo(int index)
        {
            if (index >= _ordersData.Rows.Count)
            {
                return string.Empty;
            }
            else
            {
                using (SqlHelper sqlH = new SqlHelper(SqlHelper.MyConnectionString))
                {
                    SqlDataReader sdr = null;
                    string id = _ordersData.Rows[index]["ID"].ToString();
                    sqlH.ExcuteSql("Select Info from Orders where ID=" + id, out sdr);
                    if (sdr != null)
                    {
                        StringBuilder strbInfo = new StringBuilder();
                        while (sdr.Read())
                        {
                            strbInfo.Append(sdr.GetValue(0).ToString());
                        }
                        return strbInfo.ToString();
                    }
                    else
                    {
                        return "没有找到相关订单的信息。";
                    }
                }
            }
        }

    }
}
