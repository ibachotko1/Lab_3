using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CycleMicroscope.App.ViewModels
{
    public class CycleState : INotifyPropertyChanged
    {
        private int _j;
        private int[] _res = new int[0];
        private int _variantFunction;
        private bool _isInvariantHeldBefore = true;
        private bool _isInvariantHeldAfter = true;
        private bool _isCompleted;

        public int J
        {
            get => _j;
            set
            {
                _j = value;
                OnPropertyChanged();
            }
        }

        public int[] Res
        {
            get => _res;
            set
            {
                _res = value;
                OnPropertyChanged();
            }
        }

        public int VariantFunction
        {
            get => _variantFunction;
            set
            {
                _variantFunction = value;
                OnPropertyChanged();
            }
        }

        public bool IsInvariantHeldBefore
        {
            get => _isInvariantHeldBefore;
            set
            {
                _isInvariantHeldBefore = value;
                OnPropertyChanged();
            }
        }

        public bool IsInvariantHeldAfter
        {
            get => _isInvariantHeldAfter;
            set
            {
                _isInvariantHeldAfter = value;
                OnPropertyChanged();
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
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