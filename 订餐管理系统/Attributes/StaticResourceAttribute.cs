using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 订餐管理系统.Attributes
{
    [AttributeUsage(AttributeTargets.Class,Inherited=false)]
    public class StaticResourceAttribute:Attribute
    {
        public string Key { get; set; }
        public StaticResourceAttribute(string key)
        {
            this.Key = key;
        }
    }
}
