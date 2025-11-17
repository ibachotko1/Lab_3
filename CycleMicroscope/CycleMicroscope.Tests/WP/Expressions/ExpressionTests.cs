using CycleMicroscope.WP.Expressions;
using Xunit;

namespace CycleMicroscope.Tests.WP.Expressions
{
    public class ExpressionTests
    {
        [Fact]
        public void ConstantExpression_OperationsWorkCorrectly()
        {
            var constant = new ConstantExpression(5.5);

            Assert.Equal(5.5, constant.Value);
            Assert.Equal("5.5", constant.ToString());
            Assert.Equal(constant.Clone(), constant);
        }

        [Fact]
        public void VariableExpression_SubstitutionWorks()
        {
            var variable = new VariableExpression("x");

            var substituted = variable.Substitute("x", new ConstantExpression(10));
            Assert.IsType<ConstantExpression>(substituted);
            Assert.Equal(10, ((ConstantExpression)substituted).Value);
        }

        [Fact]
        public void BinaryExpression_CreatesCorrectly()
        {
            var left = new ConstantExpression(5);
            var right = new ConstantExpression(3);
            var binary = new BinaryExpression(left, right, "+");

            Assert.Equal(left, binary.Left);
            Assert.Equal(right, binary.Right);
            Assert.Equal("+", binary.Operator);
        }
    }
}