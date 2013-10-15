using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace 订餐管理系统.Commands
{
    public class DelegateCommand : ICommand
    {

        public bool CanExecute(object parameter)
        {
            if (CanExcuteFunc == null)
            {
                return true;
            }
            else
            {
                return CanExcuteFunc(parameter);
            }

        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (ExcuteAction != null)
            {
                if (CanExecute(parameter))
                {
                    ExcuteAction(parameter);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        public Action<object> ExcuteAction { get; set; }
        public Func<object,bool> CanExcuteFunc { get; set; }
    }
}
