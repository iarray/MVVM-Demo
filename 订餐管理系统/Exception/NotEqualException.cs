using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 订餐管理系统.Exception
{
    class NotEqualException:System.Exception
    {
        public NotEqualException(string msg):base(msg){}
        public NotEqualException() { }
    }
}
