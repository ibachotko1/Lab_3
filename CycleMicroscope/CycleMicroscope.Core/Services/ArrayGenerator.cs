using System;

namespace CycleMicroscope.Core.Services
{
    /// <summary>
    /// Реализация генератора массивов случайных чисел
    /// </summary>
    public class ArrayGenerator : IArrayGenerator
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Генерация массива случайных чисел заданного размера и диапазона
        /// </summary>
        /// <param name="size">Размер массива</param>
        /// <param name="minValue">Минимальное значение элемента</param>
        /// <param name="maxValue">Максимальное значение элемента</param>
        /// <returns>Массив случайных целых чисел</returns>
        public int[] GenerateArray(int size, int minValue, int maxValue)
        {
            var array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = _random.Next(minValue, maxValue + 1);
            }
            return array;
        }
    }
}
