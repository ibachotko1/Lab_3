using CycleMicroscope.Core.Models;

namespace CycleMicroscope.Core.Algorithms
{
    /// <summary>
    /// Алгоритм подсчета элементов массива, превышающих заданный порог T
    /// </summary>
    public class CountGreaterThanTAlgorithm : ICycleAlgorithm
    {
        /// <summary>
        /// Название алгоритма
        /// </summary>
        public string Name => "Count > T";

        /// <summary>
        /// Формальное описание алгоритма
        /// </summary>
        public string Description => "res = |{ i < j : a[i] > T }|";

        /// <summary>
        /// Словесное описание инварианта цикла
        /// </summary>
        public string InvariantWords => "res содержит количество элементов a[0..k-1] больших T, где 0 ≤ k ≤ j";

        /// <summary>
        /// Формула инварианта цикла
        /// </summary>
        public string InvariantFormula => "res = |{ i < k : a[i] > T }| ∧ 0 ≤ k ≤ j";

        /// <summary>
        /// Инициализация состояния перед выполнением цикла
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        public void Initialize(ArrayModel array, CycleState state)
        {
            state.Reset();
            state.Res = 0;
            state.VariantFunction = array.Array.Length;
            state.IsInvariantHeldBefore = CheckInvariant(array, state);
        }

        /// <summary>
        /// Выполнение одного шага цикла
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        /// <returns>true - цикл может продолжаться, false - цикл завершен</returns>
        public bool ExecuteStep(ArrayModel array, CycleState state)
        {
            if (state.J >= array.Array.Length)
                return false;

            // Проверяем инвариант до выполнения шага
            state.IsInvariantHeldBefore = CheckInvariant(array, state);

            // Выполняем тело цикла: if (a[j] > T) res++; j++;
            if (array.Array[state.J] > array.Threshold)
            {
                state.Res++;
            }
            state.J++;
            state.VariantFunction = array.Array.Length - state.J;

            // Проверяем инвариант после выполнения шага
            state.IsInvariantHeldAfter = CheckInvariant(array, state);

            // Проверяем условие завершения цикла
            state.IsCompleted = state.J >= array.Array.Length;

            return !state.IsCompleted;
        }

        /// <summary>
        /// Проверка выполнения инварианта для текущего состояния
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        /// <returns>true - инвариант выполняется, false - инвариант нарушен</returns>
        public bool CheckInvariant(ArrayModel array, CycleState state)
        {
            // Проверяем: res = |{ i < k : a[i] > T }| ∧ 0 ≤ k ≤ j
            // Для простоты будем считать, что k = j
            int count = 0;
            for (int i = 0; i < state.J && i < array.Array.Length; i++)
            {
                if (array.Array[i] > array.Threshold)
                {
                    count++;
                }
            }

            return count == state.Res && state.J >= 0 && state.J <= array.Array.Length;
        }

        /// <summary>
        /// Проверка постусловия после завершения цикла
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        /// <returns>true - постусловие выполняется, false - постусловие нарушено</returns>
        public bool CheckPostCondition(ArrayModel array, CycleState state)
        {
            // Post: res = |{ i < n : a[i] > T }|
            int totalCount = 0;
            foreach (var item in array.Array)
            {
                if (item > array.Threshold)
                {
                    totalCount++;
                }
            }
            return state.Res == totalCount;
        }
    }
}
