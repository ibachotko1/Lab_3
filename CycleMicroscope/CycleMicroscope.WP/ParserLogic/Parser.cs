using System;
using System.Collections.Generic;
using System.Globalization;
using CycleMicroscope.WP.Expressions;
using CycleMicroscope.WP.Statements;

namespace CycleMicroscope.WP.ParserLogic
{
    /// <summary>
    /// Парсер для разбора выражений и операторов из текстового представления
    /// </summary>
    public class Parser  // Убираем static
    {
        private readonly Dictionary<string, int> OperatorPrecedence = new Dictionary<string, int>()
        {
            ["||"] = 1,
            ["&&"] = 2,
            ["=="] = 3,
            ["!="] = 3,
            [">"] = 4,
            [">="] = 4,
            ["<"] = 4,
            ["<="] = 4,
            ["+"] = 5,
            ["-"] = 5,
            ["*"] = 6,
            ["/"] = 6,
            ["!"] = 7,
            ["abs"] = 7,
            ["neg"] = 7
        };

        /// <summary>
        /// Разбирает выражение из строки
        /// </summary>
        /// <param name="input">Входная строка с выражением</param>
        /// <returns>Разобранное выражение</returns>
        /// <exception cref="FormatException">Выбрасывается при синтаксической ошибке</exception>
        public Expression ParseExpression(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new FormatException("Входная строка не может быть пустой");

            var tokens = Tokenize(input);
            var reader = new TokenReader(tokens);
            var expression = ParseExpression(reader, 0);

            if (!reader.IsEnd)
                throw new FormatException($"Неожиданный токен в конце выражения: {reader.Peek()}");

            return expression;
        }

        /// <summary>
        /// Разбирает оператор из строки
        /// </summary>
        /// <param name="input">Входная строка с оператором</param>
        /// <returns>Разобранный оператор</returns>
        /// <exception cref="FormatException">Выбрасывается при синтаксической ошибке</exception>
        public Statement ParseStatement(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new FormatException("Входная строка не может быть пустой");

            var tokens = Tokenize(input);
            var reader = new TokenReader(tokens);
            var statement = ParseStatement(reader);

            if (!reader.IsEnd)
                throw new FormatException($"Неожиданный токен в конце оператора: {reader.Peek()}");

            return statement;
        }

        private Expression ParseExpression(TokenReader reader, int minPrecedence)
        {
            Expression left = ParsePrimary(reader);

            while (true)
            {
                var operatorToken = reader.Peek();
                if (operatorToken == null || operatorToken.Type != TokenType.Operator)
                    break;

                string op = operatorToken.Value;
                if (!OperatorPrecedence.ContainsKey(op) || OperatorPrecedence[op] < minPrecedence)
                    break;

                reader.Read();
                int nextMinPrecedence = OperatorPrecedence[op] + 1;
                Expression right = ParseExpression(reader, nextMinPrecedence);
                left = new BinaryExpression(left, right, op);
            }

            return left;
        }

        private Expression ParsePrimary(TokenReader reader)
        {
            var token = reader.Read();
            if (token == null)
                throw new FormatException("Неожиданный конец выражения");

            // Обработка унарного минуса
            if (token.Type == TokenType.Operator && token.Value == "-")
            {
                var operand = ParsePrimary(reader);
                return new UnaryExpression(operand, "neg");
            }

            switch (token.Type)
            {
                case TokenType.Number:
                    if (double.TryParse(token.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                        return new ConstantExpression(value);
                    throw new FormatException($"Неверный формат числа: {token.Value}");

                case TokenType.Identifier:
                    if (token.Value == "abs")
                    {
                        var next = reader.Read();
                        if (next == null || next.Type != TokenType.Punctuation || next.Value != "(")
                            throw new FormatException("Ожидалась '(' после 'abs'");
                        var inner = ParseExpression(reader, 0);
                        var close = reader.Read();
                        if (close == null || close.Type != TokenType.Punctuation || close.Value != ")")
                            throw new FormatException("Ожидалась ')' после выражения в 'abs'");
                        return new UnaryExpression(inner, "abs");
                    }
                    else if (token.Value == "!")
                    {
                        var inner = ParsePrimary(reader);
                        return new UnaryExpression(inner, "!");
                    }
                    else
                    {
                        return new VariableExpression(token.Value);
                    }

                case TokenType.Punctuation when token.Value == "(":
                    var expression = ParseExpression(reader, 0);
                    var closeParen = reader.Read();
                    if (closeParen == null || closeParen.Type != TokenType.Punctuation || closeParen.Value != ")")
                        throw new FormatException("Ожидалась ')'");
                    return expression;

                default:
                    throw new FormatException($"Неожиданный токен: {token.Value}");
            }
        }

        private Statement ParseStatement(TokenReader reader)
        {
            var token = reader.Peek();

            // Обработка блока: { ... }
            if (token != null && token.Type == TokenType.Punctuation && token.Value == "{")
            {
                reader.Read(); // '{'

                var statements = new List<Statement>();
                while (true)
                {
                    var next = reader.Peek();
                    if (next == null) throw new FormatException("Ожидалась '}'");
                    if (next.Type == TokenType.Punctuation && next.Value == "}")
                        break;

                    statements.Add(ParseStatement(reader));

                    // Пропускаем ';', если есть
                    var semi = reader.Peek();
                    if (semi != null && semi.Type == TokenType.Punctuation && semi.Value == ";")
                        reader.Read();
                }

                reader.Read(); // '}'

                return statements.Count == 1 ? statements[0] : new Sequence(statements);
            }

            // Обработка условного оператора if
            if (token != null && token.Type == TokenType.Identifier && token.Value == "if")
            {
                return ParseIfStatement(reader);
            }

            // Обработка последовательности операторов
            return ParseSequence(reader);
        }

        private Statement ParseIfStatement(TokenReader reader)
        {
            reader.Read(); // 'if'
            var openParen = reader.Read();
            if (openParen == null || openParen.Type != TokenType.Punctuation || openParen.Value != "(")
                throw new FormatException("Ожидалась '(' после 'if'");

            var condition = ParseExpression(reader, 0);

            var closeParen = reader.Read();
            if (closeParen == null || closeParen.Type != TokenType.Punctuation || closeParen.Value != ")")
                throw new FormatException("Ожидалась ')' после условия if");

            var thenBranch = ParseStatement(reader);

            var elseToken = reader.Peek();
            if (elseToken != null && elseToken.Type == TokenType.Identifier && elseToken.Value == "else")
            {
                reader.Read(); // 'else'
                var elseBranch = ParseStatement(reader);
                return new IfStatement(condition, thenBranch, elseBranch);
            }

            throw new FormatException("Ожидалось 'else'");
        }

        private Statement ParseSequence(TokenReader reader)
        {
            var statements = new List<Statement>();
            while (true)
            {
                statements.Add(ParseSingleStatement(reader));
                var next = reader.Peek();
                if (next == null || next.Value != ";")
                    break;
                reader.Read(); // ';'
            }
            return statements.Count == 1 ? statements[0] : new Sequence(statements);
        }

        private Statement ParseSingleStatement(TokenReader reader)
        {
            var identifier = reader.Read();
            if (identifier == null || identifier.Type != TokenType.Identifier)
                throw new FormatException("Ожидался идентификатор");

            var assignOp = reader.Read();
            if (assignOp == null || assignOp.Type != TokenType.Operator || assignOp.Value != ":=")
                throw new FormatException("Ожидался оператор присваивания ':='");

            var expression = ParseExpression(reader, 0);
            return new Assignment(identifier.Value, expression);
        }

        private List<Token> Tokenize(string input)
        {
            var tokens = new List<Token>();
            int index = 0;

            while (index < input.Length)
            {
                char c = input[index];

                if (char.IsWhiteSpace(c))
                {
                    index++;
                    continue;
                }

                // Присваивание :=
                if (c == ':' && index + 1 < input.Length && input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.Operator, ":="));
                    index += 2;
                    continue;
                }

                // Логические операторы
                if (c == '&' && index + 1 < input.Length && input[index + 1] == '&')
                {
                    tokens.Add(new Token(TokenType.Operator, "&&"));
                    index += 2;
                    continue;
                }

                if (c == '|' && index + 1 < input.Length && input[index + 1] == '|')
                {
                    tokens.Add(new Token(TokenType.Operator, "||"));
                    index += 2;
                    continue;
                }

                // Операторы сравнения
                if (c == '=' && index + 1 < input.Length && input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.Operator, "=="));
                    index += 2;
                    continue;
                }

                if (c == '!' && index + 1 < input.Length && input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.Operator, "!="));
                    index += 2;
                    continue;
                }

                if (c == '>' && index + 1 < input.Length && input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.Operator, ">="));
                    index += 2;
                    continue;
                }

                if (c == '<' && index + 1 < input.Length && input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.Operator, "<="));
                    index += 2;
                    continue;
                }

                // Числа (включая отрицательные, но минус обрабатывается отдельно)
                if (char.IsDigit(c) || c == '.')
                {
                    int start = index;
                    while (index < input.Length && (char.IsDigit(input[index]) || input[index] == '.'))
                        index++;
                    tokens.Add(new Token(TokenType.Number, input.Substring(start, index - start)));
                    continue;
                }

                // Идентификаторы
                if (char.IsLetter(c))
                {
                    int start = index;
                    while (index < input.Length && (char.IsLetterOrDigit(input[index]) || input[index] == '_'))
                        index++;
                    tokens.Add(new Token(TokenType.Identifier, input.Substring(start, index - start)));
                    continue;
                }

                // Одиночные операторы
                if (OperatorPrecedence.ContainsKey(c.ToString()))
                {
                    tokens.Add(new Token(TokenType.Operator, c.ToString()));
                    index++;
                    continue;
                }

                // Знаки пунктуации
                if (c == '(' || c == ')' || c == ';' || c == '{' || c == '}')
                {
                    tokens.Add(new Token(TokenType.Punctuation, c.ToString()));
                    index++;
                    continue;
                }

                throw new FormatException($"Неизвестный символ: {c}");
            }

            return tokens;
        }
    }
}