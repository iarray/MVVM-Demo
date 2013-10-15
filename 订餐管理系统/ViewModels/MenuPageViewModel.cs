using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MVVM;
using 订餐管理系统.Model;

namespace 订餐管理系统.ViewModules
{
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
            using (SqlHelper sqlH = new SqlHelper(SqlHelper.MyConnectionString))
            {
                sqlH.ExcuteSql("Select * from dbo.Menu", out _menuData);
            }
        }
    }
}
