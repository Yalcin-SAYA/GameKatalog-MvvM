using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Windows.Input;

namespace GameKatalog_MvvM.Commands
{
    public class ButtonBefehl : ICommand
    {
        private readonly Action execute;

        public ButtonBefehl(Action execute)
        {
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}