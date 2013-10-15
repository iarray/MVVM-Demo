using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM;

namespace 订餐管理系统.Model
{
    public class Order:NotificationObject
    {
        private string _name;
        public string Name
        {
            get
            {
                return this._name;
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

        private int _count;
        public int Count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
                this.OnPropertyChanged("Count");
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this._isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public Order(string name, double price, int count)
        {
            this.Name = name;
            this.Price = price;
            this.Count = count;
            this.IsSelected = false;
        }

        public Order(string name, double price)
            : this(name, price, 1)
        {
        }
    }
}
