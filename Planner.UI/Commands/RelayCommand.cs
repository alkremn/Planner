using System;
using System.Windows.Input;

namespace Planner.UI
{
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        readonly Action<T> _execute = null;
        readonly Func<T, bool> _canExecute = null;

        #endregion

        #region Constructors

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute ?? (_ => true);
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute((T)parameter);

        public void Execute(object parameter) => _execute((T)parameter);

        #endregion
    }
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute)
            : base(_ => execute())
        {
        }
        public RelayCommand(Action execute, Func<bool> canExecute)
            : base(_ => execute(), _ => canExecute())
        {
        }
    }

}
