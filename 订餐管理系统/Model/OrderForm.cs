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
            SelectMenuCommand = new DelegateCommand();
            SelectMenuCommand.ExcuteAction = new Action<object>(SelectMenuProc);
        }

        public DelegateCommand SelectMenuCommand { get; set; }

        public void SelectMenuProc(object obj)
        {
            this.TotalMoney = this.GetTotalMoney();
        }
    }
}
