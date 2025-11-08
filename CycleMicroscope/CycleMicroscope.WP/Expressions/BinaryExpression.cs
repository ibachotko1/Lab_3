using System;
using System.Collections.Generic;

namespace CycleMicroscope.WP.Expressions
{
    /// <summary>
    /// Представляет бинарную операцию между двумя выражениями
    /// Поддерживаемые операции: +, -, *, /, ==, !=, >, >=, <, <=, &&, ||
    /// </summary>
    public class BinaryExpression : Expression
    {
        /// <summary>
        /// Левый операнд бинарной операции
        /// </summary>
        public Expression Left { get; }

        /// <summary>
        /// Правый операнд бинарной операции  
        /// </summary>
        public Expression Right { get; }

        /// <summary>
        /// Оператор бинарной операции
        /// </summary>
        public string Operator { get; }

        private static readonly HashSet<string> ValidOperators = new HashSet<string>()
        {
            "+", "-", "*", "/", "==", "!=", ">", ">=", "<", "<=", "&&", "||"
        };

        /// <summary>
        /// Инициализирует новое бинарное выражение
        /// </summary>
        /// <param name="left">Левый операнд</param>
        /// <param name="right">Правый операнд</param>
        /// <param name="operator">Оператор</param>
        /// <exception cref="ArgumentException">Выбрасывается при невалидном операторе или операндах</exception>
        public BinaryExpression(Expression left, Expression right, string @operator)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));

            if (!ValidOperators.Contains(@operator))
                throw new ArgumentException($"Недопустимый оператор: {@operator}", nameof(@operator));

            Operator = @operator;
        }

        /// <summary>
        /// Выполняет подстановку переменной в выражении
        /// </summary>
        public override Expression Substitute(string variableName, Expression replacement)
        {
            // Рекурсивно применяем подстановку к обоим операндам
            var newLeft = Left.Substitute(variableName, replacement);
            var newRight = Right.Substitute(variableName, replacement);

            return new BinaryExpression(newLeft, newRight, Operator);
        }

        /// <summary>
        /// Собирает условия определенности для выражения
        /// </summary>
        public override List<Expression> GetDefinitenessConditions()
        {
            var conditions = new List<Expression>();

            // Для операции деления добавляем условие: знаменатель ≠ 0
            if (Operator == "/")
            {
                var denominatorNotZero = new BinaryExpression(
                    Right.Clone(),
                    new ConstantExpression(0),
                    "!="
                );
                conditions.Add(denominatorNotZero);
            }

            // Рекурсивно собираем условия из операндов
            conditions.AddRange(Left.GetDefinitenessConditions());
            conditions.AddRange(Right.GetDefinitenessConditions());

            return conditions;
        }

        /// <summary>
        /// Преобразует выражение в строковое представление
        /// </summary>
        public override string ToString()
        {
            // Для логических операторов используем более читаемое форматирование
            if (Operator == "&&" || Operator == "||")
            {
                return $"({Left} {Operator} {Right})";
            }

            return $"{Left} {Operator} {Right}";
        }

        /// <summary>
        /// Создает глубокую копию выражения
        /// </summary>
        public override Expression Clone()
        {
            return new BinaryExpression(Left.Clone(), Right.Clone(), Operator);
        }

        /// <summary>
        /// Проверяет эквивалентность двух бинарных выражений
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is BinaryExpression other &&
                   Left.Equals(other.Left) &&
                   Right.Equals(other.Right) &&
                   Operator == other.Operator;
        }

        /// <summary>
        /// Возвращает хэш-код выражения
        /// </summary>
        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + Left.GetHashCode();
            hash = hash * 23 + Right.GetHashCode();
            hash = hash * 23 + Operator.GetHashCode();
            return hash;
        }
    }
}
