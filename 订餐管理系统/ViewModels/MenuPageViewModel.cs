﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MVVM;
using 订餐管理系统.Attributes;
using 订餐管理系统.Model;

namespace 订餐管理系统.ViewModules
{
    [StaticResource("MenuPageViewModel")]
    class MenuPageViewModel:NotificationObject
    {
        private DataTable _menuData;

        public DataTable MenuData
        {
            get
            {
                return this._menuData;
            }
        }

        public MenuPageViewModel()
        {
            using (SqlCRUD sqlCRUD = new SqlCRUD(SqlHelper.MyConnectionString))
            {
                _menuData = sqlCRUD.SelectData("Menu", new string[] { "*" });
            }
        }
    }
}
