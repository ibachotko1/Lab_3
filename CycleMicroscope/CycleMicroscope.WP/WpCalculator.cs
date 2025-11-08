using CycleMicroscope.WP.Expressions;
using CycleMicroscope.WP.Logs;
using CycleMicroscope.WP.ParserLogic;
using CycleMicroscope.WP.Statements;

namespace CycleMicroscope.WP
{
    /// <summary>
    /// Основной класс для вычисления слабейшего предусловия (Weakest Precondition)
    /// </summary>
    public class WpCalculator
    {
        private readonly Parser _parser = new Parser();

        /// <summary>
        /// Вычисляет слабейшее предусловие для оператора и постусловия
        /// </summary>
        /// <param name="statement">Оператор программы</param>
        /// <param name="postCondition">Постусловие</param>
        /// <param name="stepTracker">Трекер для записи шагов вычисления</param>
        /// <returns>Слабейшее предусловие</returns>
        public Expression CalculateWP(Statement statement, Expression postCondition, StepTracker stepTracker = null)
        {
            return statement.CalculateWP(postCondition, stepTracker);
        }

        /// <summary>
        /// Вычисляет слабейшее предусловие из строкового представления
        /// </summary>
        /// <param name="statementText">Текст оператора</param>
        /// <param name="postConditionText">Текст постусловия</param>
        /// <param name="stepTracker">Трекер для записи шагов вычисления</param>
        /// <returns>Слабейшее предусловие</returns>
        public Expression CalculateWP(string statementText, string postConditionText, StepTracker stepTracker = null)
        {
            var statement = _parser.ParseStatement(statementText);  
            var postCondition = _parser.ParseExpression(postConditionText);  
            return CalculateWP(statement, postCondition, stepTracker);
        }

        /// <summary>
        /// Проверяет корректность оператора относительно предусловия и постусловия
        /// </summary>
        /// <param name="preCondition">Предусловие</param>
        /// <param name="statement">Оператор</param>
        /// <param name="postCondition">Постусловие</param>
        /// <returns>true если оператор корректен, иначе false</returns>
        public bool VerifyCorrectness(Expression preCondition, Statement statement, Expression postCondition)
        {
            var wp = CalculateWP(statement, postCondition);

            // Для проверки корректности нужно проверить, что preCondition => wp
            // В упрощенной реализации считаем, что если выражения эквивалентны, то корректно
            return preCondition.Equals(wp);
        }

        /// <summary>
        /// Проверяет корректность оператора из строкового представления
        /// </summary>
        /// <param name="preConditionText">Текст предусловия</param>
        /// <param name="statementText">Текст оператора</param>
        /// <param name="postConditionText">Текст постусловия</param>
        /// <returns>true если оператор корректен, иначе false</returns>
        public bool VerifyCorrectness(string preConditionText, string statementText, string postConditionText)
        {
            var preCondition = _parser.ParseExpression(preConditionText);  
            var statement = _parser.ParseStatement(statementText); 
            var postCondition = _parser.ParseExpression(postConditionText);  
            return VerifyCorrectness(preCondition, statement, postCondition);
        }
    }
}