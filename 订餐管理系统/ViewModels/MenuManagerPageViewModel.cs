using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using FirstFloor.ModernUI.Windows.Controls;
using MVVM;
using 订餐管理系统.Attributes;
using 订餐管理系统.Commands;
using 订餐管理系统.Manager;
using 订餐管理系统.Model;
using 订餐管理系统.ViewModules;

namespace 订餐管理系统.ViewModels
{
    [StaticResource("MenuManagerPageViewModel")]
    class MenuManagerPageViewModel:NotificationObject
    {
        private DataTable _menuTable;
        public DataTable MenuTable
        {
            get
            {
                return this._menuTable;

            }
            private set
            {
                this._menuTable = value;
                this.OnPropertyChanged("MenuTable");
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                this._name = value;
                this.OnPropertyChanged("Name");
            }
        }

        private double _price;
        public double Price
        {
            get
            {
                return this._price;
            }
            set
            {
                this._price = value;
                this.OnPropertyChanged("Price");
            }
        }

        private string _type;
        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value; 
                this.OnPropertyChanged("Type");
            }

        }

        private string _review;
        public string Review
        {
            get
            {
                return this._review;
            }
            set
            {
                this._review = value;
                this.OnPropertyChanged("Review");
            }
        }

        private int _score;
        public int Score
        {
            get
            {
                return this._score;
            }
            set
            {
                this._score = value;
                this.OnPropertyChanged("Score");
            }
        }

        /// <summary>
        /// 绑定菜单表中选中项索引
        /// </summary>
        public int SelectIndex { get; set; }

        /// <summary>
        /// 添加数据命令
        /// </summary>
        public DelegateCommand InsertCommand { get; set; }

        public DelegateCommand DeleteCommand { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MenuManagerPageViewModel()
        {
            UpDateTable();
            MenuTableAddEvent();
            InsertCommand=new DelegateCommand(new Action<object>(InsertDataToTable));
            DeleteCommand = new DelegateCommand(new Action<object>(DeleteRow));
        }

        /// <summary>
        /// 更新MenuTable表中数据
        /// </summary>
        public void UpDateTable()
        {
            using (SqlCRUD sqlCRUD = new SqlCRUD(SqlHelper.MyConnectionString))
            {
                MenuTable = sqlCRUD.SelectData("Menu", new string[] { "*" });
            }
        }

        /// <summary>
        /// 更新订餐菜单表
        /// </summary>
        public void UpDateOrderPageTable()
        {
            OrderPageViewModel OPVM = ViewModelsManager.GetViewModelFromResources<OrderPageViewModel>();
            if (OPVM != null)
                OPVM.LoadMenuData();
        }

        /// <summary>
        /// 移除MenuTable事件委托
        /// </summary>
        public void MenuTableRemoveEvent()
        {
            if (MenuTable != null)
                _menuTable.RowChanged -= _menuTable_RowChanged;
        }

        /// <summary>
        /// 添加MenuTable事件委托
        /// </summary>
        public void MenuTableAddEvent()
        {
            if (MenuTable != null)
                _menuTable.RowChanged +=_menuTable_RowChanged;
        }

        /// <summary>
        /// MenuTable行数据改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _menuTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Row != null)
            {
                string id = e.Row["ID"].ToString();
                if (id != null)
                {
                    using (SqlCRUD sqlCRUD = new SqlCRUD(SqlHelper.MyConnectionString))
                    {
                        sqlCRUD.UpdateData("Menu", new string[] { "Name", "Price", "Review", "Score" },
                            new string[] { e.Row["Name"].ToString(), e.Row["Price"].ToString(), e.Row["Review"].ToString(), e.Row["Score"].ToString() },"ID="+id);
                    }
                }
            }
        }

        /// <summary>
        /// 添加数据命令函数
        /// </summary>
        /// <param name="obj"></param>
        public void InsertDataToTable(object obj)
        {
            if (!string.IsNullOrEmpty(this.Name) && this.Price>=0)
            {
                SqlCRUD sqlCRUD = new SqlCRUD(SqlHelper.MyConnectionString);
                try
                {
                    sqlCRUD.InsertData("Menu", new string[] { "Name", "Price", "Type", "Review", "Score" },
                        new string[] { Name, Price.ToString(), Type, Review, Score.ToString() });
                    ModernDialog.ShowMessage("添加一项数据成功", "提示", System.Windows.MessageBoxButton.OK);
                    UpDateTable();
                    SetDefauteValue();
                    UpDateOrderPageTable();
                }
                catch(System.Exception e)
                {
                    ModernDialog.ShowMessage(e.Message,"错误",System.Windows.MessageBoxButton.OKCancel);
                }
                finally
                {
                    sqlCRUD.Dispose();
                }
            }
        }

        /// <summary>
        /// 删除一行数据
        /// </summary>
        /// <param name="index"></param>
        public void DeleteRow(object index)
        {
            int i = this.SelectIndex;
            if (i >= 0&&i<MenuTable.Rows.Count)
            {
                string id = MenuTable.Rows[i]["ID"].ToString();
                using (SqlCRUD sqlCRUD = new SqlCRUD(SqlHelper.MyConnectionString))
                {
                    sqlCRUD.DeleteData("Menu", "ID=" + id);
                }
                UpDateTable();
                UpDateOrderPageTable();
            }
        }

        /// <summary>
        /// 重置添加数据所需值
        /// </summary>
        public void SetDefauteValue()
        {
            this.Name = string.Empty;
            this.Price = 0;
            this.Review = string.Empty;
            this.Type = string.Empty;
            this.Score = 0;
        }
    }
}
