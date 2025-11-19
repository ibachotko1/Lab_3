using CycleMicroscope.Core.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CycleMicroscope.App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _selectedMode = "PrefixMax";
        private string _wpResult = "Результат будет отображен здесь";
        private ArrayModel _arrayModel = new ArrayModel();
        private CycleState _cycleState = new CycleState();

        public MainViewModel()
        {
            GenerateArrayCommand = new RelayCommand(GenerateArray);
            InitializeCommand = new RelayCommand(InitializeAlgorithm);
            StepCommand = new RelayCommand(ExecuteStep);
            RunAllCommand = new RelayCommand(RunAllSteps);

            // Инициализация по умолчанию
            GenerateArray();
        }

        public ArrayModel ArrayModel
        {
            get => _arrayModel;
            set
            {
                _arrayModel = value;
                OnPropertyChanged();
            }
        }

        public CycleState CycleState
        {
            get => _cycleState;
            set
            {
                _cycleState = value;
                OnPropertyChanged();
            }
        }

        public string SelectedMode
        {
            get => _selectedMode;
            set
            {
                _selectedMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AlgorithmName));
                OnPropertyChanged(nameof(AlgorithmDescription));
                OnPropertyChanged(nameof(AlgorithmInvariantWords));
                OnPropertyChanged(nameof(AlgorithmInvariantFormula));
            }
        }

        public string WPResult
        {
            get => _wpResult;
            set
            {
                _wpResult = value;
                OnPropertyChanged();
            }
        }

        // Алгоритм информация
        public string AlgorithmName
        {
            get
            {
                switch (SelectedMode)
                {
                    case "PrefixSum": return "Prefix Sum (Префиксные суммы)";
                    case "CountGreaterThanT": return "Count Greater Than T (Подсчёт больше T)";
                    case "PrefixMax": return "Prefix Maximum (Максимум на префиксе)";
                    default: return "Неизвестный алгоритм";
                }
            }
        }

        public string AlgorithmDescription
        {
            get
            {
                switch (SelectedMode)
                {
                    case "PrefixSum": return "Вычисляет сумму элементов на каждом префиксе массива";
                    case "CountGreaterThanT": return "Подсчитывает количество элементов, больших заданного порога T";
                    case "PrefixMax": return "Находит максимальный элемент на каждом префиксе массива";
                    default: return "Описание отсутствует";
                }
            }
        }

        public string AlgorithmInvariantWords
        {
            get
            {
                switch (SelectedMode)
                {
                    case "PrefixSum": return "Сумма элементов от начала до текущей позиции корректна";
                    case "CountGreaterThanT": return "Количество элементов > T корректно подсчитано до текущей позиции";
                    case "PrefixMax": return "Максимальный элемент корректно определён для текущего префикса";
                    default: return "Инвариант не определён";
                }
            }
        }

        public string AlgorithmInvariantFormula
        {
            get
            {
                switch (SelectedMode)
                {
                    case "PrefixSum": return "res[i] = sum(arr[0..i])";
                    case "CountGreaterThanT": return "count = Σ [arr[j] > T] для j=0..i";
                    case "PrefixMax": return "res[i] = max(arr[0..i])";
                    default: return "Формула не определена";
                }
            }
        }

        // Команды
        public ICommand GenerateArrayCommand { get; }
        public ICommand InitializeCommand { get; }
        public ICommand StepCommand { get; }
        public ICommand RunAllCommand { get; }

        private void GenerateArray()
        {
            try
            {
                var random = new Random();
                var array = new int[ArrayModel.Size];

                for (int i = 0; i < ArrayModel.Size; i++)
                {
                    array[i] = random.Next(ArrayModel.MinValue, ArrayModel.MaxValue + 1);
                }

                ArrayModel.Array = array;
                WPResult = $"Сгенерирован массив: [{string.Join(", ", array)}]";

                // Сброс состояния алгоритма
                CycleState = new CycleState();
            }
            catch (Exception ex)
            {
                WPResult = $"Ошибка генерации: {ex.Message}";
            }
        }

        private void InitializeAlgorithm()
        {
            try
            {
                var newState = new CycleState
                {
                    J = 0,
                    VariantFunction = ArrayModel.Array.Length,
                    IsInvariantHeldBefore = true,
                    IsInvariantHeldAfter = true,
                    IsCompleted = false
                };

                // Инициализация Res в зависимости от выбранного режима
                switch (SelectedMode)
                {
                    case "PrefixSum":
                        newState.Res = new int[ArrayModel.Array.Length];
                        if (ArrayModel.Array.Length > 0)
                            newState.Res[0] = ArrayModel.Array[0];
                        break;
                    case "CountGreaterThanT":
                        newState.Res = new int[1] { 0 }; // count
                        break;
                    case "PrefixMax":
                        newState.Res = new int[ArrayModel.Array.Length];
                        if (ArrayModel.Array.Length > 0)
                            newState.Res[0] = ArrayModel.Array[0];
                        break;
                    default:
                        newState.Res = new int[0];
                        break;
                }

                CycleState = newState;
                WPResult = "Алгоритм инициализирован. Нажмите 'Шаг' для выполнения.";
            }
            catch (Exception ex)
            {
                WPResult = $"Ошибка инициализации: {ex.Message}";
            }
        }

        private void ExecuteStep()
        {
            if (CycleState.IsCompleted || CycleState.J >= ArrayModel.Array.Length)
            {
                WPResult = "Алгоритм уже завершён";
                return;
            }

            try
            {
                var array = ArrayModel.Array;
                int j = CycleState.J;

                // Проверка инварианта до выполнения шага
                CycleState.IsInvariantHeldBefore = CheckInvariantBefore(j);

                // Выполнение шага в зависимости от алгоритма
                switch (SelectedMode)
                {
                    case "PrefixSum":
                        if (j > 0) // j=0 уже инициализирован
                        {
                            CycleState.Res[j] = CycleState.Res[j - 1] + array[j];
                        }
                        break;

                    case "CountGreaterThanT":
                        if (array[j] > ArrayModel.Threshold)
                        {
                            CycleState.Res[0]++; // увеличиваем счетчик
                        }
                        break;

                    case "PrefixMax":
                        if (j > 0) // j=0 уже инициализирован
                        {
                            CycleState.Res[j] = Math.Max(CycleState.Res[j - 1], array[j]);
                        }
                        break;
                }

                // Обновление состояния
                CycleState.J++;
                CycleState.VariantFunction = array.Length - CycleState.J;

                // Проверка инварианта после выполнения шага
                CycleState.IsInvariantHeldAfter = CheckInvariantAfter(CycleState.J - 1);

                // Проверка завершения
                if (CycleState.J >= array.Length)
                {
                    CycleState.IsCompleted = true;
                    switch (SelectedMode)
                    {
                        case "PrefixSum":
                            WPResult = $"Префиксные суммы: [{string.Join(", ", CycleState.Res)}]";
                            break;
                        case "CountGreaterThanT":
                            WPResult = $"Количество элементов > {ArrayModel.Threshold}: {CycleState.Res[0]}";
                            break;
                        case "PrefixMax":
                            WPResult = $"Максимумы на префиксах: [{string.Join(", ", CycleState.Res)}]";
                            break;
                        default:
                            WPResult = $"Результат: {string.Join(", ", CycleState.Res)}";
                            break;
                    }
                }
                else
                {
                    WPResult = $"Выполнен шаг {CycleState.J} из {array.Length}";
                }
            }
            catch (Exception ex)
            {
                WPResult = $"Ошибка выполнения шага: {ex.Message}";
            }
        }

        private void RunAllSteps()
        {
            InitializeAlgorithm();

            while (!CycleState.IsCompleted && CycleState.J < ArrayModel.Array.Length)
            {
                ExecuteStep();
            }
        }

        private bool CheckInvariantBefore(int j)
        {
            try
            {
                var array = ArrayModel.Array;

                switch (SelectedMode)
                {
                    case "PrefixSum":
                        // Для j=0 res[0] должен равняться array[0]
                        if (j == 0)
                            return CycleState.Res.Length > 0 && CycleState.Res[0] == array[0];

                        // Для j>0 res[j-1] должен быть суммой элементов от 0 до j-1
                        if (j > 0 && j < CycleState.Res.Length)
                        {
                            int expectedSum = 0;
                            for (int i = 0; i < j; i++)
                                expectedSum += array[i];

                            return CycleState.Res[j - 1] == expectedSum;
                        }
                        break;

                    case "CountGreaterThanT":
                        // res[0] должен быть количеством элементов > T от 0 до j-1
                        int expectedCount = 0;
                        for (int i = 0; i < j; i++)
                        {
                            if (array[i] > ArrayModel.Threshold)
                                expectedCount++;
                        }
                        return CycleState.Res[0] == expectedCount;

                    case "PrefixMax":
                        // Для j=0 res[0] должен равняться array[0]
                        if (j == 0)
                            return CycleState.Res.Length > 0 && CycleState.Res[0] == array[0];

                        // Для j>0 res[j-1] должен быть максимумом от 0 до j-1
                        if (j > 0 && j < CycleState.Res.Length)
                        {
                            int expectedMax = array[0];
                            for (int i = 1; i < j; i++)
                            {
                                if (array[i] > expectedMax)
                                    expectedMax = array[i];
                            }
                            return CycleState.Res[j - 1] == expectedMax;
                        }
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool CheckInvariantAfter(int j)
        {
            try
            {
                var array = ArrayModel.Array;

                switch (SelectedMode)
                {
                    case "PrefixSum":
                        // res[j] должен быть суммой элементов от 0 до j
                        if (j < CycleState.Res.Length)
                        {
                            int expectedSum = 0;
                            for (int i = 0; i <= j; i++)
                                expectedSum += array[i];

                            return CycleState.Res[j] == expectedSum;
                        }
                        break;

                    case "CountGreaterThanT":
                        // res[0] должен быть количеством элементов > T от 0 до j
                        int expectedCount = 0;
                        for (int i = 0; i <= j; i++)
                        {
                            if (array[i] > ArrayModel.Threshold)
                                expectedCount++;
                        }
                        return CycleState.Res[0] == expectedCount;

                    case "PrefixMax":
                        // res[j] должен быть максимумом от 0 до j
                        if (j < CycleState.Res.Length)
                        {
                            int expectedMax = array[0];
                            for (int i = 1; i <= j; i++)
                            {
                                if (array[i] > expectedMax)
                                    expectedMax = array[i];
                            }
                            return CycleState.Res[j] == expectedMax;
                        }
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}