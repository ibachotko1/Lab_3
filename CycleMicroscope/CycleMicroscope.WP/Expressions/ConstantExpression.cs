using System;
using System.Collections.Generic;
using System.Globalization;

namespace CycleMicroscope.WP.Expressions
{
    /// <summary>
    /// Представляет константное значение (числовую константу)
    /// </summary>
    public class ConstantExpression : Expression
    {
        /// <summary>
        /// Числовое значение константы
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Инициализирует новое константное выражение
        /// </summary>
        /// <param name="value">Числовое значение константы</param>
        public ConstantExpression(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Выполняет подстановку переменной в выражении
        /// </summary>
        public override Expression Substitute(string variableName, Expression replacement)
        {
            // Константы не содержат переменных, возвращаем неизмененную копию
            return Clone();
        }

        /// <summary>
        /// Собирает условия определенности для выражения
        /// </summary>
        public override List<Expression> GetDefinitenessConditions()
        {
            // Константы всегда определены, дополнительных условий не требуется
            return new List<Expression>();
        }

        /// <summary>
        /// Преобразует выражение в строковое представление
        /// </summary>
        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Создает глубокую копию выражения
        /// </summary>
        public override Expression Clone()
        {
            return new ConstantExpression(Value);
        }

        /// <summary>
        /// Проверяет эквивалентность двух константных выражений
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is ConstantExpression other && Math.Abs(Value - other.Value) < 1e-10;
        }

        /// <summary>
        /// Возвращает хэш-код выражения
        /// </summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
