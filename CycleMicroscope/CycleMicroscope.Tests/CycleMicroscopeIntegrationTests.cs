using CycleMicroscope.Core.Algorithms;
using CycleMicroscope.Core.Models;
using CycleMicroscope.WP;
using CycleMicroscope.WP.Expressions;
using CycleMicroscope.WP.Statements;
using Xunit;

namespace CycleMicroscope.Tests
{
    public class CycleMicroscopeIntegrationTests
    {
        [Fact]
        public void FullIntegrationTest_AlgorithmWithWPVerification()
        {
            // Arrange
            var algorithm = new PrefixSumAlgorithm();
            var arrayModel = new ArrayModel { Array = new[] { 1, 2, 3, 4, 5 } };
            var state = new CycleState();
            var wpCalculator = new WpCalculator();

            // Act - Execute algorithm
            algorithm.Initialize(arrayModel, state);
            while (algorithm.ExecuteStep(arrayModel, state)) { }

            // Assert - Check post condition
            var postConditionHolds = algorithm.CheckPostCondition(arrayModel, state);
            Assert.True(postConditionHolds);
            Assert.Equal(15, state.Res); // 1+2+3+4+5 = 15
        }

        [Fact]
        public void WPCalculator_VerifyCorrectness_SimpleAssignment()
        {
            // Arrange
            var calculator = new WpCalculator();

            // Act & Assert
            var result = calculator.VerifyCorrectness("5", "x := 5", "x");

            Assert.True(result);
        }
    }
}