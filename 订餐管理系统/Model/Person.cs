using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 订餐管理系统.Model
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
