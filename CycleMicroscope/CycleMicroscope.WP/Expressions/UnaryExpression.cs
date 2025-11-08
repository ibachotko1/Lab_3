using System;
using System.Collections.Generic;

namespace CycleMicroscope.WP.Expressions
{
    /// <summary>
    /// Представляет унарную операцию
    /// Поддерживаемые операции: ! (логическое отрицание), abs() (модуль)
    /// </summary>
    public class UnaryExpression : Expression
    {
        /// <summary>
        /// Операнд унарной операции
        /// </summary>
        public Expression Operand { get; }

        /// <summary>
        /// Оператор унарной операции
        /// </summary>
        public string Operator { get; }

        private static readonly HashSet<string> ValidOperators = new HashSet<string>() { "!", "abs", "neg" };

        /// <summary>
        /// Инициализирует новое унарное выражение
        /// </summary>
        /// <param name="operand">Операнд</param>
        /// <param name="operator">Оператор</param>
        /// <exception cref="ArgumentException">Выбрасывается при невалидном операторе</exception>
        public UnaryExpression(Expression operand, string @operator)
        {
            Operand = operand ?? throw new ArgumentNullException(nameof(operand));

            if (!ValidOperators.Contains(@operator))
                throw new ArgumentException($"Недопустимый унарный оператор: {@operator}", nameof(@operator));

            Operator = @operator;
        }

        /// <summary>
        /// Выполняет подстановку переменной в выражении
        /// </summary>
        public override Expression Substitute(string variableName, Expression replacement)
        {
            var newOperand = Operand.Substitute(variableName, replacement);
            return new UnaryExpression(newOperand, Operator);
        }

        /// <summary>
        /// Собирает условия определенности для выражения
        /// </summary>
        public override List<Expression> GetDefinitenessConditions()
        {
            // Унарные операции всегда определены, но собираем условия из операнда
            return Operand.GetDefinitenessConditions();
        }

        /// <summary>
        /// Преобразует выражение в строковое представление
        /// </summary>
        public override string ToString()
        {
            return Operator == "abs"
                ? $"abs({Operand})"
                : Operator == "neg"
                ? $"-{Operand}"
                : $"{Operator}({Operand})";
        }

        /// <summary>
        /// Создает глубокую копию выражения
        /// </summary>
        public override Expression Clone()
        {
            return new UnaryExpression(Operand.Clone(), Operator);
        }

        /// <summary>
        /// Проверяет эквивалентность двух унарных выражений
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is UnaryExpression other &&
                   Operand.Equals(other.Operand) &&
                   Operator == other.Operator;
        }

        /// <summary>
        /// Возвращает хэш-код выражения
        /// </summary>
        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + Operand.GetHashCode();
            hash = hash * 23 + Operator.GetHashCode();
            return hash;
        }
    }
}
