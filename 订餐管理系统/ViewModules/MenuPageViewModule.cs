using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MVVM;
using 订餐管理系统.Model;

namespace 订餐管理系统.ViewModules
{
    class MenuPageViewModule:NotificationObject
    {
        private DataTable _menuData;

        public DataTable MenuData
        {
            get
            {
                return this._menuData;
            }
        }

        public MenuPageViewModule()
        {
            SqlHelper sqlH = new SqlHelper("server=localhost;uid=root;pwd=123456;database=AppDataBase;");
            sqlH.ExcuteSql("Select * from dbo.Menu", out _menuData);
        }
    }
}
