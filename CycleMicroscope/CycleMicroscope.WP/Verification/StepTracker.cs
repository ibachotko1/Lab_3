using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleMicroscope.WP.Verification
{
    /// <summary>
    /// Трекер для записи шагов вычисления слабейшего предусловия (WP)
    /// Совместим с .NET Framework и C# 7.3
    /// </summary>
    public class StepTracker
    {
        private readonly List<string> _steps = new List<string>();

        /// <summary>
        /// Записывает шаг вычисления
        /// </summary>
        /// <param name="step">Текст шага</param>
        public void RecordStep(string step)
        {
            _steps.Add(step);
        }

        /// <summary>
        /// Возвращает все записанные шаги
        /// </summary>
        /// <returns>Строковое представление всех шагов</returns>
        public override string ToString()
        {
            if (_steps.Count == 0)
                return "Шаги не записаны.";

            var sb = new StringBuilder();
            for (int i = 0; i < _steps.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {_steps[i]}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Очищает историю шагов
        /// </summary>
        public void Clear()
        {
            _steps.Clear();
        }

        /// <summary>
        /// Получает количество шагов
        /// </summary>
        public int StepCount => _steps.Count;

        /// <summary>
        /// Получает последний шаг
        /// </summary>
        public string LastStep
        {
            get
            {
                if (_steps.Count == 0)
                    return null;
                return _steps[_steps.Count - 1]; // ← вместо [^1]
            }
        }
    }
}