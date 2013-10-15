using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using MVVM;
using 订餐管理系统.Commands;

namespace 订餐管理系统.Model
{
    class OrderForm<T>where T: Order
    {
        //ObservableCollection
        public ObservableCollection<T> Orders { get; set; }

        public double TotalMoney { get; set; }

        public double GetTotalMoney()
        {
            if (Orders == null || Orders.Count == 0)
            {
                return 0;
            }
            double totalmoney=0;
            foreach(Order ord in Orders)
            {
                if(ord.IsSelected==true)
                    totalmoney += ord.Count * ord.Price;
            }
            return  totalmoney;
        }

        public OrderForm()
        {
            Orders = new ObservableCollection<T>();
        }

        public string GetOrderInfoString()
        {
            List<Order> ods = this.GetIsSelectedOrder();
            StringBuilder strOrder = new StringBuilder(50);
            strOrder.Append("订单信息:\n");
            foreach (Order od in ods)
            {
                strOrder.Append(od.Name);
                strOrder.Append("\t数量:");
                strOrder.Append(od.Count);
                strOrder.Append("\n");
            }
            this.TotalMoney=this.GetTotalMoney();
            strOrder.Append("总金额:" + TotalMoney.ToString() + " 元");
            return strOrder.ToString();
        }

        public List<Order> GetIsSelectedOrder()
        {
            List<Order> ods =
                   (from od in this.Orders
                    where od.IsSelected == true
                    select od).ToList<Order>();
            return ods;
        }
    }
}
