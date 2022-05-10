using System;
using System.Windows.Input;

namespace Restaurant.Admin.Command
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action<object> execute) : this(null, execute)
        {

        }
        public DelegateCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            if (this.canExecute == null)
            {
                this.canExecute = (o) => true;
            }
            else
            {
                this.canExecute = canExecute;
            }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if(!canExecute(parameter))
            {
                throw new InvalidOperationException("Command execution is disabled.");
            }
            execute(parameter);
        }
    }
}
