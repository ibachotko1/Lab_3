using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleMicroscope.Core.Services
{
    /// <summary>
    /// Интерфейс генератора массивов случайных чисел
    /// </summary>
    public interface IArrayGenerator
    {
        /// <summary>
        /// Генерация массива случайных чисел
        /// </summary>
        /// <param name="size">Размер массива</param>
        /// <param name="minValue">Минимальное значение элемента</param>
        /// <param name="maxValue">Максимальное значение элемента</param>
        /// <returns>Массив случайных целых чисел</returns>
        int[] GenerateArray(int size, int minValue, int maxValue);
    }
}
