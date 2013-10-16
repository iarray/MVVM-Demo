using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVM.Attributes
{
    public class StaticResourceAttribute:Attribute
    {
        public string Key { get; set; }
        public StaticResourceAttribute(string key)
        {
            this.Key = key;
        }
    }
}
