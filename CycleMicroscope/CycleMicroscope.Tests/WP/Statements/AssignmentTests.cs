using CycleMicroscope.WP.Expressions;
using CycleMicroscope.WP.Statements;
using Xunit;

namespace CycleMicroscope.Tests.WP.Statements
{
    public class AssignmentTests
    {
        [Fact]
        public void Constructor_ValidParameters_CreatesAssignment()
        {
            var assignment = new Assignment("x", new ConstantExpression(5));

            Assert.Equal("x", assignment.VariableName);
            Assert.IsType<ConstantExpression>(assignment.Value);
        }

        [Fact]
        public void CalculateWP_ReturnsCorrectPrecondition()
        {
            var assignment = new Assignment("x", new ConstantExpression(5));
            var postCondition = new VariableExpression("x");

            var wp = assignment.CalculateWP(postCondition);

            Assert.IsType<ConstantExpression>(wp);
            Assert.Equal(5, ((ConstantExpression)wp).Value);
        }

        [Fact]
        public void ToString_ReturnsCorrectFormat()
        {
            var assignment = new Assignment("result", new ConstantExpression(10));

            Assert.Equal("result := 10", assignment.ToString());
        }
    }
}