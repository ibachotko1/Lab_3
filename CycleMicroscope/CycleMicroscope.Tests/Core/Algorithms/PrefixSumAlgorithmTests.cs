using CycleMicroscope.Core.Algorithms;
using CycleMicroscope.Core.Models;
using Xunit;

namespace CycleMicroscope.Tests.Core.Algorithms
{
    public class PrefixSumAlgorithmTests
    {
        private readonly PrefixSumAlgorithm _algorithm = new PrefixSumAlgorithm();
        private readonly ArrayModel _arrayModel = new ArrayModel();
        private readonly CycleState _state = new CycleState();

        [Fact]
        public void Properties_ReturnCorrectValues()
        {
            Assert.Equal("Prefix Sum", _algorithm.Name);
            Assert.Equal("res = Σ_{i=0}^{j−1} a[i]", _algorithm.Description);
        }

        [Fact]
        public void ExecuteStep_UpdatesStateCorrectly()
        {
            _arrayModel.Array = new[] { 1, 2, 3 };
            _algorithm.Initialize(_arrayModel, _state);

            var canContinue = _algorithm.ExecuteStep(_arrayModel, _state);

            Assert.True(canContinue);
            Assert.Equal(1, _state.J);
            Assert.Equal(1, _state.Res);
        }

        [Fact]
        public void CheckPostCondition_ValidatesCorrectly()
        {
            _arrayModel.Array = new[] { 1, 2, 3 };
            _state.Res = 6;

            var result = _algorithm.CheckPostCondition(_arrayModel, _state);

            Assert.True(result);
        }
    }
}