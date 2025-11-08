using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CycleMicroscope.Core.Models
{
    /// <summary>
    /// Состояние выполнения цикла с отслеживанием инвариантов и варианта-функции
    /// </summary>
    public class CycleState : INotifyPropertyChanged
    {
        private int _j;
        private int _res;
        private int _variantFunction;
        private bool _isInvariantHeldBefore;
        private bool _isInvariantHeldAfter;
        private bool _isCompleted;

        /// <summary>
        /// Текущий индекс в массиве (счетчик цикла)
        /// </summary>
        public int J
        {
            get => _j;
            set
            {
                if (_j != value)
                {
                    _j = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Результат вычисления (сумма, счетчик или максимум)
        /// </summary>
        public int Res
        {
            get => _res;
            set
            {
                if (_res != value)
                {
                    _res = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Значение варианта-функции (монотонно убывает)
        /// </summary>
        public int VariantFunction
        {
            get => _variantFunction;
            set
            {
                if (_variantFunction != value)
                {
                    _variantFunction = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Флаг выполнения инварианта до шага цикла
        /// </summary>
        public bool IsInvariantHeldBefore
        {
            get => _isInvariantHeldBefore;
            set
            {
                if (_isInvariantHeldBefore != value)
                {
                    _isInvariantHeldBefore = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Флаг выполнения инварианта после шага цикла
        /// </summary>
        public bool IsInvariantHeldAfter
        {
            get => _isInvariantHeldAfter;
            set
            {
                if (_isInvariantHeldAfter != value)
                {
                    _isInvariantHeldAfter = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Флаг завершения цикла
        /// </summary>
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Сброс состояния к начальным значениям
        /// </summary>
        public void Reset()
        {
            J = 0;
            Res = 0;
            VariantFunction = 0;
            IsInvariantHeldBefore = false;
            IsInvariantHeldAfter = false;
            IsCompleted = false;
        }

        /// <summary>
        /// Событие изменения свойства
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод вызова события изменения свойства
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
