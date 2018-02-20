using System;

namespace StockScreener
{
    public class CommandHandler : System.Windows.Input.ICommand
    {
        private Action<object> _actionWithParam;
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public CommandHandler(Action action)
        {
            _action = action;
            _canExecute = true;
        }

        public CommandHandler(Action<object> action, bool canExecute)
        {
            _actionWithParam = action;
            _canExecute = canExecute;
        }
        public CommandHandler(Action<object> action)
        {
            _actionWithParam = action;
            _canExecute = true;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
                _action();
            else
                _actionWithParam(parameter);
        }


    }
}
