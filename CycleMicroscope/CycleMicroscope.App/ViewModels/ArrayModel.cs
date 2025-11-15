using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CycleMicroscope.App.ViewModels
{
    public class ArrayModel : INotifyPropertyChanged
    {
        private int _size = 5;
        private int _minValue = 1;
        private int _maxValue = 10;
        private int _threshold = 5;
        private int[] _array = new int[5];

        public int Size
        {
            get => _size;
            set
            {
                if (value != _size)
                {
                    _size = value;
                    OnPropertyChanged();
                    // Автоматически пересоздаем массив при изменении размера
                    Array = new int[_size];
                }
            }
        }

        public int MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                OnPropertyChanged();
            }
        }

        public int MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                OnPropertyChanged();
            }
        }

        public int Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                OnPropertyChanged();
            }
        }

        public int[] Array
        {
            get => _array;
            set
            {
                _array = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}