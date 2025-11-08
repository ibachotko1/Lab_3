using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CycleMicroscope.Core.Models
{
    /// <summary>
    /// Модель данных для работы с массивом и его параметрами
    /// </summary>
    public class ArrayModel : INotifyPropertyChanged
    {
        private int[] _array = new int[0];
        private int _size = 10;
        private int _minValue = 0;
        private int _maxValue = 100;
        private int _threshold = 50;

        /// <summary>
        /// Массив целых чисел для обработки
        /// </summary>
        public int[] Array
        {
            get => _array;
            set
            {
                if (_array != value)
                {
                    _array = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Размер массива
        /// </summary>
        public int Size
        {
            get => _size;
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Минимальное значение элементов массива
        /// </summary>
        public int MinValue
        {
            get => _minValue;
            set
            {
                if (_minValue != value)
                {
                    _minValue = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Максимальное значение элементов массива
        /// </summary>
        public int MaxValue
        {
            get => _maxValue;
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Пороговое значение для алгоритма подсчета
        /// </summary>
        public int Threshold
        {
            get => _threshold;
            set
            {
                if (_threshold != value)
                {
                    _threshold = value;
                    OnPropertyChanged();
                }
            }
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