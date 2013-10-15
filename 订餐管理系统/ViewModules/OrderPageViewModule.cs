using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using MVVM;
using 订餐管理系统.Commands;
using 订餐管理系统.Model;

namespace 订餐管理系统.ViewModules
{
    class OrderPageViewModule:NotificationObject
    {
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

        public OrderForm<Order> OrderList { get; set; }

        public OrderPageViewModule()
        {
            OrderList = new OrderForm<Order>();
            using (SqlHelper sqlH = new SqlHelper("server=localhost;uid=root;pwd=123456;database=AppDataBase;"))
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
            SelectMenuCommand = new DelegateCommand();
            SelectMenuCommand.ExcuteAction = new Action<object>(SelectMenuProc);
        }

        public DelegateCommand SelectMenuCommand { get; set; }

        public void SelectMenuProc(object obj)
        {
            this.ToTalMoney = this.OrderList.GetTotalMoney();
//            MessageBox.Show("a");
        }
    }
}
