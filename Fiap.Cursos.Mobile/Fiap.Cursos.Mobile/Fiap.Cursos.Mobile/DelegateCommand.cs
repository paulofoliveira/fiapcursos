using System;
using System.Windows.Input;

namespace Fiap.Cursos.Mobile
{
    public class DelegateCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public DelegateCommand(Action<object> executeCommand, Func<object, bool> canExecuteCommand)
        {
            execute = executeCommand;
            canExecute = canExecuteCommand;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
