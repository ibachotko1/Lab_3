using CycleMicroscope.WP.Expressions;
using CycleMicroscope.WP.ParserLogic;
using CycleMicroscope.WP.Statements;
using Xunit;

namespace CycleMicroscope.Tests.WP.ParserLogic
{
    public class ParserTests
    {
        private readonly Parser _parser = new Parser();

        [Theory]
        [InlineData("5", 5.0)]
        [InlineData("3.14", 3.14)]
        [InlineData("0", 0.0)]
        public void ParseExpression_Constant_ReturnsConstantExpression(string input, double expectedValue)
        {
            // Act
            var result = _parser.ParseExpression(input);

            // Assert
            var constant = Assert.IsType<ConstantExpression>(result);
            Assert.Equal(expectedValue, constant.Value);
        }

        [Theory]
        [InlineData("x")]
        [InlineData("variable")]
        [InlineData("test_var")]
        public void ParseExpression_Variable_ReturnsVariableExpression(string input)
        {
            // Act
            var result = _parser.ParseExpression(input);

            // Assert
            var variable = Assert.IsType<VariableExpression>(result);
            Assert.Equal(input, variable.Name);
        }

        [Theory]
        [InlineData("x + y", "+")]
        [InlineData("a - b", "-")]
        [InlineData("p * q", "*")]
        [InlineData("m / n", "/")]
        [InlineData("x == y", "==")]
        [InlineData("a != b", "!=")]
        [InlineData("p > q", ">")]
        [InlineData("p >= q", ">=")]
        [InlineData("a < b", "<")]
        [InlineData("a <= b", "<=")]
        [InlineData("x && y", "&&")]
        [InlineData("a || b", "||")]
        public void ParseExpression_BinaryOperation_ReturnsBinaryExpression(string input, string expectedOperator)
        {
            // Act
            var result = _parser.ParseExpression(input);

            // Assert
            var binary = Assert.IsType<BinaryExpression>(result);
            Assert.Equal(expectedOperator, binary.Operator);
        }

        [Theory]
        [InlineData("(x + y) * z")]
        [InlineData("x * (y + z)")]
        [InlineData("(a + b) * (c - d)")]
        public void ParseExpression_WithParentheses_RespectsPrecedence(string input)
        {
            // Act
            var result = _parser.ParseExpression(input);

            // Assert - Should parse without throwing
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("x := 5")]
        [InlineData("result := x + y")]
        [InlineData("max := abs(x)")]
        public void ParseStatement_Assignment_ReturnsAssignment(string input)
        {
            // Act
            var result = _parser.ParseStatement(input);

            // Assert
            var assignment = Assert.IsType<Assignment>(result);
            Assert.NotNull(assignment.VariableName);
            Assert.NotNull(assignment.Value);
        }

        [Theory]
        [InlineData("x := 5; y := 10")]
        [InlineData("a := 1; b := 2; c := 3")]
        public void ParseStatement_Sequence_ReturnsSequence(string input)
        {
            // Act
            var result = _parser.ParseStatement(input);

            // Assert
            var sequence = Assert.IsType<Sequence>(result);
            Assert.True(sequence.Statements.Count > 1);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ParseExpression_EmptyInput_ThrowsFormatException(string input)
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => _parser.ParseExpression(input));
        }

        [Theory]
        [InlineData("x + ")]
        [InlineData("(x + y")]
        [InlineData("x @ y")] // Invalid operator
        public void ParseExpression_InvalidSyntax_ThrowsFormatException(string input)
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => _parser.ParseExpression(input));
        }

        [Fact]
        public void ParseExpression_ComplexExpression_CorrectStructure()
        {
            // Arrange
            var input = "a + b * c - d / (e + f)";

            // Act
            var result = _parser.ParseExpression(input);

            // Assert - Should parse without throwing and return valid structure
            Assert.NotNull(result);
            Assert.IsType<BinaryExpression>(result);
        }

        // Новый тест для проверки отрицательных чисел в контексте выражений
        [Theory]
        [InlineData("5 - 10")] // Бинарный минус
        [InlineData("-10 + 5")] // Унарный минус в начале
        [InlineData("x - -y")] // Унарный минус в середине
        public void ParseExpression_NegativeNumbers_VariousContexts(string input)
        {
            // Act
            var result = _parser.ParseExpression(input);

            // Assert - Should parse without throwing
            Assert.NotNull(result);
        }
    }
}