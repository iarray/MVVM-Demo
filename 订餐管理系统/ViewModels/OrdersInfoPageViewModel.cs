using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MVVM;
using 订餐管理系统.Attributes;
using 订餐管理系统.Model;

namespace 订餐管理系统.ViewModels
{
    [StaticResource("OrdersInfoPageViewModel")]
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
            set
            {
                this._ordersData = value;
                this.OnPropertyChanged("OrdersData");
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
                if (this._orderSelectIndex != value)
                {
                    this._orderSelectIndex = value;
                    this.OnPropertyChanged("OrderSelectIndex");
                    this.SelectOrderInfo = this.GetSelectOrderInfo(this._orderSelectIndex);
                }
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

        /// <summary>
        /// 数据过滤指定天数
        /// </summary>
        private int _filterDay;
        public int FilterDay
        {
            get
            {
                return this._filterDay;
            }
            set
            {
                if (this._filterDay != value)
                {
                    this._filterDay = value;
                    this.OnPropertyChanged("FilterDay");
                    this.InitializeData();
                }
            }
        }


        public OrdersInfoPageViewModel()
        {
            FilterDay = 1;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitializeData()
        {
            using (SqlCRUD sqlCRUD = new SqlCRUD(SqlHelper.MyConnectionString))
            {
                OrdersData = sqlCRUD.SelectData("Orders", new string[] { "ID", "Money", "Time" },"DATEDIFF(day, Time, '"+DateTime.Now.ToShortDateString()+"')<="+this.FilterDay.ToString());
            }
            OrderSelectIndex = 0;
        }

        /// <summary>
        /// 获取选中项详细订单信息
        /// </summary>
        /// <param name="index">选中项索引</param>
        /// <returns>返回订单信息</returns>
        public string GetSelectOrderInfo(int index)
        {
            if (index >= _ordersData.Rows.Count||index<0)
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
