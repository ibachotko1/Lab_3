using CycleMicroscope.Core.Models;

namespace CycleMicroscope.Core.Algorithms
{
    /// <summary>
    /// Алгоритм вычисления суммы элементов префикса массива
    /// </summary>
    public class PrefixSumAlgorithm : ICycleAlgorithm
    {
        /// <summary>
        /// Название алгоритма
        /// </summary>
        public string Name => "Prefix Sum";

        /// <summary>
        /// Формальное описание алгоритма
        /// </summary>
        public string Description => "res = Σ_{i=0}^{j−1} a[i]";

        /// <summary>
        /// Словесное описание инварианта цикла
        /// </summary>
        public string InvariantWords => "res содержит сумму элементов a[0] до a[k-1], где 0 ≤ k ≤ j";

        /// <summary>
        /// Формула инварианта цикла
        /// </summary>
        public string InvariantFormula => "res = Σ_{i=0}^{k-1} a[i] ∧ 0 ≤ k ≤ j";

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

            // Выполняем тело цикла: res += a[j]; j++;
            state.Res += array.Array[state.J];
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
            // Проверяем: res = Σ_{i=0}^{k-1} a[i] ∧ 0 ≤ k ≤ j
            // Для простоты будем считать, что k = j
            int sum = 0;
            for (int i = 0; i < state.J && i < array.Array.Length; i++)
            {
                sum += array.Array[i];
            }

            return sum == state.Res && state.J >= 0 && state.J <= array.Array.Length;
        }

        /// <summary>
        /// Проверка постусловия после завершения цикла
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        /// <returns>true - постусловие выполняется, false - постусловие нарушено</returns>
        public bool CheckPostCondition(ArrayModel array, CycleState state)
        {
            // Post: res = Σ_{i=0}^{n-1} a[i]
            int totalSum = 0;
            foreach (var item in array.Array)
            {
                totalSum += item;
            }
            return state.Res == totalSum;
        }
    }
}
