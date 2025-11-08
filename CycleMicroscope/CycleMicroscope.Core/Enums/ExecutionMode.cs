using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleMicroscope.Core.Enums
{
    /// <summary>
    /// Режимы выполнения алгоритмов цикла
    /// </summary>
    public enum ExecutionMode
    {
        /// <summary>
        /// Сумма префикса массива
        /// </summary>
        PrefixSum,

        /// <summary>
        /// Подсчет элементов больше порога T
        /// </summary>
        CountGreaterThanT,

        /// <summary>
        /// Максимальный элемент в префиксе массива
        /// </summary>
        PrefixMax
    }
}
