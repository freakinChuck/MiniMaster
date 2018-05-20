using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiniMaster._Helper
{
    public class BindingCommand : ICommand
    {
        private readonly Action<object> myAction;
        public BindingCommand(Action<object> myAction)
        {
            this.myAction = myAction;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            myAction.Invoke(parameter);
        }
    }
}
