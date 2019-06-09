using System;
using System.Windows.Input;

namespace CsharpHelpers.WindowServices
{

    public sealed class DelegateCommand : ICommand
    {

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


        private readonly Predicate<object> _canExecute;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke(parameter);
        }


        private readonly Action<object> _execute;

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }

    }


    public sealed class DelegateCommand<T> : ICommand
    {

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


        private readonly Predicate<T> _canExecute;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke((T)parameter);
        }


        private readonly Action<T> _execute;

        public void Execute(object parameter)
        {
            _execute.Invoke((T)parameter);
        }

    }

}
