using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CycleMicroscope.Core.Models
{
    public class ArrayModel : INotifyPropertyChanged
    {
        private int[] _array = System.Array.Empty<int>();
        private int _size = 10;
        private int _minValue = 0;
        private int _maxValue = 100;
        private int _threshold = 50;

        public int[] Array
        {
            get => _array;
            set => SetField(ref _array, value);
        }

        public int Size
        {
            get => _size;
            set => SetField(ref _size, value);
        }

        public int MinValue
        {
            get => _minValue;
            set => SetField(ref _minValue, value);
        }

        public int MaxValue
        {
            get => _maxValue;
            set => SetField(ref _maxValue, value);
        }

        public int Threshold
        {
            get => _threshold;
            set => SetField(ref _threshold, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}