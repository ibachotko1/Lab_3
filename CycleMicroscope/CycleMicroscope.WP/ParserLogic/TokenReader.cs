using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleMicroscope.WP.ParserLogic
{
    /// <summary>
    /// Читатель токенов для последовательного доступа к списку токенов
    /// </summary>
    public class TokenReader
    {
        private readonly List<Token> _tokens;
        private int _index;

        /// <summary>
        /// Инициализирует новый читатель токенов
        /// </summary>
        /// <param name="tokens">Список токенов для чтения</param>
        public TokenReader(List<Token> tokens)
        {
            _tokens = tokens;
            _index = 0;
        }

        /// <summary>
        /// Возвращает текущий токен без продвижения вперед
        /// </summary>
        /// <returns>Текущий токен или null если достигнут конец</returns>
        public Token Peek()
        {
            return _index < _tokens.Count ? _tokens[_index] : null;
        }

        /// <summary>
        /// Возвращает текущий токен и продвигается вперед
        /// </summary>
        /// <returns>Текущий токен или null если достигнут конец</returns>
        public Token Read()
        {
            return _index < _tokens.Count ? _tokens[_index++] : null;
        }

        /// <summary>
        /// Проверяет, достигнут ли конец списка токенов
        /// </summary>
        public bool IsEnd => _index >= _tokens.Count;

        /// <summary>
        /// Возвращает количество оставшихся токенов
        /// </summary>
        public int RemainingTokens => _tokens.Count - _index;
    }
}
