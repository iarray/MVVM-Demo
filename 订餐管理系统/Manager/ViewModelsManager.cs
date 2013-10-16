using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using 订餐管理系统.Attributes;

namespace 订餐管理系统.Manager
{
    public class ViewModelsManager
    {
        private static Application app = App.Current;

        /// <summary>
        /// 将标有StaticResourceAttribute特性的类添加为静态资源
        /// </summary>
        public static void InitializeViewModelsToStaticResouce()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    if (attribute is StaticResourceAttribute)
                    {
                        var staticResource = Activator.CreateInstance(type);
                        if (!app.Resources.Contains(staticResource))
                        {
                            app.Resources.Add(((StaticResourceAttribute)attribute).Key, staticResource);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 从静态资源中获取ViewModel
        /// </summary>
        /// <typeparam name="T">ViewModel类型</typeparam>
        /// <returns>返回静态资源中ViewModel实例</returns>
        public static T GetViewModelFromResources<T>()
        {

            var key = typeof(T).Name;

            if (app.Resources.Contains(key))

                return (T)app.Resources[key];

            else

                return default(T);

        }
    }
}
