using CycleMicroscope.WP.Verification;
using Xunit;

namespace CycleMicroscope.Tests.WP.Verification
{
    public class StepTrackerTests
    {
        [Fact]
        public void RecordStep_AddsStepToHistory()
        {
            var tracker = new StepTracker();

            tracker.RecordStep("Step 1");
            tracker.RecordStep("Step 2");

            Assert.Equal(2, tracker.StepCount);
            Assert.Equal("Step 2", tracker.LastStep);
        }

        [Fact]
        public void ToString_ReturnsFormattedSteps()
        {
            var tracker = new StepTracker();

            tracker.RecordStep("First step");
            tracker.RecordStep("Second step");

            var result = tracker.ToString();

            Assert.Contains("1. First step", result);
            Assert.Contains("2. Second step", result);
        }

        [Fact]
        public void Clear_RemovesAllSteps()
        {
            var tracker = new StepTracker();

            tracker.RecordStep("Test step");
            tracker.Clear();

            Assert.Equal(0, tracker.StepCount);
            Assert.Null(tracker.LastStep);
        }
    }
}