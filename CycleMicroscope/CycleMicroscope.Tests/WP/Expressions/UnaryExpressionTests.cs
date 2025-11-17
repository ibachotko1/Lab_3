using CycleMicroscope.WP.Expressions;
using Xunit;

namespace CycleMicroscope.Tests.WP.Expressions
{
    public class UnaryExpressionTests
    {
        [Theory]
        [InlineData("!", "!(5)")]
        [InlineData("abs", "abs(5)")]
        [InlineData("neg", "-5")]
        public void ToString_ReturnsCorrectFormat(string operation, string expected)
        {
            // Arrange
            var operand = new ConstantExpression(5);
            var unary = new UnaryExpression(operand, operation);

            // Act & Assert
            Assert.Equal(expected, unary.ToString());
        }

        [Fact]
        public void Substitute_ReplacesOperand()
        {
            // Arrange
            var operand = new VariableExpression("x");
            var unary = new UnaryExpression(operand, "neg");
            var replacement = new ConstantExpression(10);

            // Act
            var result = unary.Substitute("x", replacement);

            // Assert
            var resultUnary = Assert.IsType<UnaryExpression>(result);
            Assert.Equal("neg", resultUnary.Operator);
            var resultOperand = Assert.IsType<ConstantExpression>(resultUnary.Operand);
            Assert.Equal(10, resultOperand.Value);
        }

        [Fact]
        public void Clone_CreatesDeepCopy()
        {
            // Arrange
            var operand = new VariableExpression("x");
            var original = new UnaryExpression(operand, "abs");

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.IsType<UnaryExpression>(cloned);
            Assert.Equal(original.Operator, ((UnaryExpression)cloned).Operator);
            Assert.NotSame(original, cloned);
            Assert.NotSame(original.Operand, ((UnaryExpression)cloned).Operand);
        }

        [Fact]
        public void GetDefinitenessConditions_ReturnsOperandConditions()
        {
            // Arrange
            var operand = new BinaryExpression(
                new ConstantExpression(10),
                new ConstantExpression(0),
                "/"
            );
            var unary = new UnaryExpression(operand, "abs");

            // Act
            var conditions = unary.GetDefinitenessConditions();

            // Assert - Should include conditions from operand (division by zero)
            Assert.Single(conditions);
            Assert.Contains("0 != 0", conditions[0].ToString());
        }
    }
}