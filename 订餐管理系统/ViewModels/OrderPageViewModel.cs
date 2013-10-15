using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using MVVM;
using 订餐管理系统.Commands;
using 订餐管理系统.Model;

namespace 订餐管理系统.ViewModules
{
    class OrderPageViewModel:NotificationObject
    {
        /// <summary>
        /// 订单总金额
        /// </summary>
        private double _totalMoney;
        public double ToTalMoney
        {
            get
            {
                return this._totalMoney;
            }
            set
            {
                this._totalMoney = value;
                this.OnPropertyChanged("ToTalMoney");
            }
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        public OrderForm<Order> OrderList { get; set; }

        /// <summary>
        /// 构造函数，从数据库读取订单信息
        /// </summary>
        public OrderPageViewModel()
        {
            OrderList = new OrderForm<Order>();
            using (SqlHelper sqlH = new SqlHelper(SqlHelper.MyConnectionString))
            {
                SqlDataReader sdr;
                sqlH.ExcuteSql("Select Name,Price from dbo.Menu", out sdr);
                if (sdr != null)
                {
                    while (sdr.Read())
                    {
                        OrderList.Orders.Add(new Order(sdr.GetString(0), sdr.GetSqlMoney(1).ToDouble()));
                    }
                    sdr.Dispose();
                }
            }

            SelectMenuCommand = new DelegateCommand(new Action<object>(SelectMenuProc));
            SumbitCommand=new DelegateCommand(new Action<object>(PostOrdersProc));
        }

        public DelegateCommand SelectMenuCommand { get; set; }
        public DelegateCommand SumbitCommand { get; set; }

        public void SelectMenuProc(object obj)
        {
            this.ToTalMoney = this.OrderList.GetTotalMoney();
        }

        public void PostOrdersProc(object obj)
        {
            string info=OrderList.GetOrderInfoString();
            if(ModernDialog.ShowMessage(info,"订单信息",MessageBoxButton.OKCancel)==
                MessageBoxResult.OK)
            {
                using (SqlHelper sqlH = new SqlHelper(SqlHelper.MyConnectionString))
                {
                    sqlH.ExcuteSql("Insert into Orders(Info,Money,Time)values('" 
                                            + info + "','" 
                                            + OrderList.TotalMoney.ToString() + "','" 
                                            + DateTime.Now.ToString() + "')");
                }
            }
        }
    }
}
