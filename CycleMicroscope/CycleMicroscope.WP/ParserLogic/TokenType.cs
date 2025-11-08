using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleMicroscope.WP.ParserLogic
{
    /// <summary>
    /// Типы токенов для парсера выражений и операторов
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Числовая константа
        /// </summary>
        Number,

        /// <summary>
        /// Идентификатор (имя переменной или функция)
        /// </summary>
        Identifier,

        /// <summary>
        /// Оператор (+, -, *, /, &&, || и т.д.)
        /// </summary>
        Operator,

        /// <summary>
        /// Знак пунктуации (скобки, точка с запятой и т.д.)
        /// </summary>
        Punctuation
    }
}
