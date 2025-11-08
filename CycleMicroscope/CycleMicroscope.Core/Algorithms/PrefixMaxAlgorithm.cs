using CycleMicroscope.Core.Models;

namespace CycleMicroscope.Core.Algorithms
{
    /// <summary>
    /// Алгоритм поиска максимального элемента в префиксе массива
    /// </summary>
    public class PrefixMaxAlgorithm : ICycleAlgorithm
    {
        /// <summary>
        /// Название алгоритма
        /// </summary>
        public string Name => "Prefix Max";

        /// <summary>
        /// Формальное описание алгоритма
        /// </summary>
        public string Description => "res = max(a[0..j−1])";

        /// <summary>
        /// Словесное описание инварианта цикла
        /// </summary>
        public string InvariantWords => "res содержит максимальный элемент в a[0..k-1], где 0 ≤ k ≤ j";

        /// <summary>
        /// Формула инварианта цикла
        /// </summary>
        public string InvariantFormula => "res = max(a[0..k)) ∧ 0 ≤ k ≤ j";

        /// <summary>
        /// Инициализация состояния перед выполнением цикла
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        public void Initialize(ArrayModel array, CycleState state)
        {
            state.Reset();
            // Аккуратная база при j=0: если массив пустой, устанавливаем минимальное значение
            state.Res = (array.Array.Length > 0) ? int.MinValue : 0;
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

            // Выполняем тело цикла: res = max(res, a[j]); j++;
            if (array.Array[state.J] > state.Res)
            {
                state.Res = array.Array[state.J];
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
            // Проверяем: res = max(a[0..k)) ∧ 0 ≤ k ≤ j
            // Для простоты будем считать, что k = j

            // Обработка базового случая: j = 0
            if (state.J == 0)
            {
                return state.Res == int.MinValue && state.J >= 0 && state.J <= array.Array.Length;
            }

            // Находим реальный максимум в префиксе
            int maxVal = int.MinValue;
            for (int i = 0; i < state.J && i < array.Array.Length; i++)
            {
                if (array.Array[i] > maxVal)
                {
                    maxVal = array.Array[i];
                }
            }

            return state.Res == maxVal && state.J >= 0 && state.J <= array.Array.Length;
        }

        /// <summary>
        /// Проверка постусловия после завершения цикла
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        /// <returns>true - постусловие выполняется, false - постусловие нарушено</returns>
        public bool CheckPostCondition(ArrayModel array, CycleState state)
        {
            // Post: res = max(a[0..n-1])
            if (array.Array.Length == 0)
                return state.Res == int.MinValue;

            int totalMax = int.MinValue;
            foreach (var item in array.Array)
            {
                if (item > totalMax)
                {
                    totalMax = item;
                }
            }
            return state.Res == totalMax;
        }
    }
}
