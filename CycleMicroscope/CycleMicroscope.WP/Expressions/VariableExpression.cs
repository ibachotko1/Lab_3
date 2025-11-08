using System;
using System.Collections.Generic;

namespace CycleMicroscope.WP.Expressions
{
    /// <summary>
    /// Представляет переменную в выражении
    /// </summary>
    public class VariableExpression : Expression
    {
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Инициализирует новое выражение переменной
        /// </summary>
        /// <param name="name">Имя переменной</param>
        /// <exception cref="ArgumentException">Выбрасывается если имя null или пустое</exception>
        public VariableExpression(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя переменной не может быть пустым", nameof(name));

            Name = name.Trim();
        }

        /// <summary>
        /// Выполняет подстановку переменной в выражении
        /// </summary>
        public override Expression Substitute(string variableName, Expression replacement)
        {
            if (Name == variableName)
            {
                // Заменяем эту переменную на указанное выражение
                return replacement.Clone();
            }

            // Имя не совпадает - возвращаем неизмененную копию
            return Clone();
        }

        /// <summary>
        /// Собирает условия определенности для выражения
        /// </summary>
        public override List<Expression> GetDefinitenessConditions()
        {
            // Переменные всегда определены (в контексте wp-вычислений)
            return new List<Expression>();
        }

        /// <summary>
        /// Преобразует выражение в строковое представление
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Создает глубокую копию выражения
        /// </summary>
        public override Expression Clone()
        {
            return new VariableExpression(Name);
        }

        /// <summary>
        /// Проверяет эквивалентность двух переменных выражений
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is VariableExpression other && Name == other.Name;
        }

        /// <summary>
        /// Возвращает хэш-код выражения
        /// </summary>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}