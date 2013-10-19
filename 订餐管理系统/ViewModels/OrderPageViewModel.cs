using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using MVVM;
using 订餐管理系统.Attributes;
using 订餐管理系统.Commands;
using 订餐管理系统.Manager;
using 订餐管理系统.Model;
using 订餐管理系统.ViewModels;

namespace 订餐管理系统.ViewModules
{
    [StaticResource("OrderPageViewModel")]
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
        /// 订单备注信息
        /// </summary>
        private string _remark;
        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
                this.OnPropertyChanged("Remark");
            }
        }
        /// <summary>
        /// 构造函数，从数据库读取订单信息
        /// </summary>
        public OrderPageViewModel()
        {
            OrderList = new OrderForm<Order>();
            LoadMenuData();
            SelectMenuCommand = new DelegateCommand(new Action<object>(SelectMenuProc));
            SumbitCommand=new DelegateCommand(new Action<object>(PostOrdersProc));
        }

        public DelegateCommand SelectMenuCommand { get; set; }
        public DelegateCommand SumbitCommand { get; set; }

        public void LoadMenuData()
        {
            if (OrderList != null)
            {
                OrderList.Orders.Clear();
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
            }
        }

        /// <summary>
        /// CheckBox相关Command
        /// </summary>
        /// <param name="obj"></param>
        public void SelectMenuProc(object obj)
        {
            this.ToTalMoney = this.OrderList.GetTotalMoney();
        }

        /// <summary>
        /// Button(提交按钮)相关Command
        /// </summary>
        /// <param name="obj"></param>
        public void PostOrdersProc(object obj)
        {
            string info = string.Empty;
            if (String.IsNullOrEmpty(this.Remark))
            {
                info = OrderList.GetOrderInfoString();
            }
            else
            {
                info = OrderList.GetOrderInfoString() + "\n\n备注信息:\n" + this.Remark;
            }
            if (ModernDialog.ShowMessage(info, "订单信息", MessageBoxButton.OKCancel) ==
                MessageBoxResult.OK)
            {
                SqlHelper sqlH=null;
                try
                {
                    using (sqlH = new SqlHelper(SqlHelper.MyConnectionString))
                    {
                        SqlCRUD sqlCRUD = new SqlCRUD(SqlHelper.MyConnectionString);
                        sqlCRUD.InsertData("Orders", new string[] { "Info", "Money", "Time" },
                            new string[] { info, OrderList.TotalMoney.ToString(), DateTime.Now.ToString() });
                    }
                    this.Remark = string.Empty;
                    OrdersInfoPageViewModel OrdersInfoVM = ViewModelsManager.GetViewModelFromResources<OrdersInfoPageViewModel>();
                    OrdersInfoVM.InitializeData();
                }
                finally
                {
                    if (sqlH != null)
                    {
                        sqlH.Dispose();
                    }
                }
            }   
        }
    }
}
