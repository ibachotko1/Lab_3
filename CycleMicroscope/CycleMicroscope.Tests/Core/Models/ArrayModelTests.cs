using CycleMicroscope.Core.Models;
using Xunit;

namespace CycleMicroscope.Tests.Core.Models
{
    public class ArrayModelTests
    {
        [Fact]
        public void PropertyChanged_IsRaised_WhenPropertyChanges()
        {
            var model = new ArrayModel();
            var changedProperties = new List<string>();
            model.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName);

            model.Size = 20;

            Assert.Contains(nameof(ArrayModel.Size), changedProperties);
        }

        [Fact]
        public void Properties_CanBeSetAndGet()
        {
            var model = new ArrayModel();

            model.Size = 15;
            model.MinValue = 0;
            model.MaxValue = 100;
            model.Threshold = 50;

            Assert.Equal(15, model.Size);
            Assert.Equal(0, model.MinValue);
            Assert.Equal(100, model.MaxValue);
            Assert.Equal(50, model.Threshold);
        }
    }
}