using System;
using System.Windows.Input;

namespace CycleMicroscope.Core.Common
{
    /// <summary>
    /// Реализация команды для MVVM с поддержкой проверки возможности выполнения
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="execute">Действие для выполнения</param>
        /// <param name="canExecute">Функция проверки возможности выполнения</param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Событие изменения возможности выполнения команды
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Проверка возможности выполнения команды
        /// </summary>
        /// <param name="parameter">Параметр команды</param>
        /// <returns>true - команда может быть выполнена, false - команда не может быть выполнена</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="parameter">Параметр команды</param>
        public void Execute(object parameter)
        {
            _execute();
        }
    }
}