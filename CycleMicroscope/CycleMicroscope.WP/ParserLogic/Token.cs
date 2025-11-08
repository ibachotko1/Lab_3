using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleMicroscope.WP.ParserLogic
{
    /// <summary>
    /// Представляет токен - минимальную единицу разбора исходного кода
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Тип токена
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// Строковое значение токена
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Инициализирует новый токен
        /// </summary>
        /// <param name="type">Тип токена</param>
        /// <param name="value">Строковое значение токена</param>
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Преобразует токен в строковое представление
        /// </summary>
        public override string ToString()
        {
            return $"{Type}: '{Value}'";
        }
    }
}
