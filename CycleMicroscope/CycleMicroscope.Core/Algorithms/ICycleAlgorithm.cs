using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CycleMicroscope.Core.Models;

namespace CycleMicroscope.Core.Algorithms
{
    /// <summary>
    /// Интерфейс алгоритма выполнения цикла с проверкой инвариантов
    /// </summary>
    public interface ICycleAlgorithm
    {
        /// <summary>
        /// Название алгоритма
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Описание алгоритма
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Словесное описание инварианта цикла
        /// </summary>
        string InvariantWords { get; }

        /// <summary>
        /// Формула инварианта цикла
        /// </summary>
        string InvariantFormula { get; }

        /// <summary>
        /// Инициализация состояния перед выполнением цикла
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        void Initialize(ArrayModel array, CycleState state);

        /// <summary>
        /// Выполнение одного шага цикла
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        /// <returns>true - цикл может продолжаться, false - цикл завершен</returns>
        bool ExecuteStep(ArrayModel array, CycleState state);

        /// <summary>
        /// Проверка выполнения инварианта для текущего состояния
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        /// <returns>true - инвариант выполняется, false - инвариант нарушен</returns>
        bool CheckInvariant(ArrayModel array, CycleState state);

        /// <summary>
        /// Проверка постусловия после завершения цикла
        /// </summary>
        /// <param name="array">Модель массива данных</param>
        /// <param name="state">Состояние выполнения цикла</param>
        /// <returns>true - постусловие выполняется, false - постусловие нарушено</returns>
        bool CheckPostCondition(ArrayModel array, CycleState state);
    }
}
