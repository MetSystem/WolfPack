using Growl.Connector;
using Wolfpack.Core.Growl.Formatters;
using Wolfpack.Core.Interfaces.Entities;
using NUnit.Framework;
using Wolfpack.Core.Notification;
using Wolfpack.Core.Testing;
using Wolfpack.Core.Testing.Drivers;

namespace Wolfpack.Tests.Growl
{
    [TestFixture]
    public abstract class WhenUsingTheGrowlCountNotificationFinaliserBase
    {
        protected Notification _growlNotification;
        protected GrowlCountFormatter _sut;
        protected Priority _expectedPriorityWhenLower;
        protected Priority _expectedPriorityWhenEqual;
        protected Priority _expectedPriorityWhenHigher;

        public const double PRIORITY_THRESHOLD = 2.34;

        [TestFixtureSetUp]
        public virtual void Given()
        {            
            SetupNotification();
            _expectedPriorityWhenLower = ExpectedPriorityWhenResultCountIsLower();         
            _expectedPriorityWhenEqual = ExpectedPriorityWhenResultCountIsEqual();
            _expectedPriorityWhenHigher = ExpectedPriorityWhenResultCountIsHigher();
            SetupFormatter();    
        }

        protected virtual GrowlCountFormatter SetupFormatter()
        {
            _sut = new GrowlCountFormatter
            {
                Check = "*",
                Threshold = PRIORITY_THRESHOLD,
                HigherPriority = _expectedPriorityWhenHigher.ToString(),
                LowerPriority = _expectedPriorityWhenLower.ToString()
            };

            return _sut;
        }

        protected virtual void SetupNotification()
        {
            _growlNotification = new Notification("testApp", "testName", "testId", "testTitle", "testText")
            {
                Priority = Priority.Normal
            };
        }

        protected abstract Priority ExpectedPriorityWhenResultCountIsLower();
        protected abstract Priority ExpectedPriorityWhenResultCountIsEqual();
        protected abstract Priority ExpectedPriorityWhenResultCountIsHigher();

        [Test]
        public virtual void ThenThePriorityShouldBeCorrectForALowerResultCount()
        {
            SetupNotification();
            var message = NotificationEventBuilder.From(new NotificationEventHealthCheck
                                                            {
                                                                ResultCount = PRIORITY_THRESHOLD - 1
                                                            }).Build();

            _sut.Format(message, _growlNotification);

            Assert.That(_growlNotification.Priority == _expectedPriorityWhenLower, Is.True, "Expected {0}, Actual {1}",
                _expectedPriorityWhenLower, _growlNotification.Priority);
        }

        [Test]
        public void ThenThePriorityShouldBeCorrectForAnEqualResultCount()
        {
            SetupNotification();
            var message = NotificationEventBuilder.From(new NotificationEventHealthCheck
                                                            {
                                                                ResultCount = PRIORITY_THRESHOLD
                                                            }).Build();

            _sut.Format(message, _growlNotification);
            Assert.That(_growlNotification.Priority == _expectedPriorityWhenEqual, Is.True, "Expected {0}, Actual {1}",
                _expectedPriorityWhenEqual, _growlNotification.Priority);
        }

        [Test]
        public void ThenThePriorityShouldBeCorrectForAHigherResultCount()
        {
            SetupNotification();
            var message = NotificationEventBuilder.From(new NotificationEventHealthCheck
                                                            {
                                                                ResultCount = PRIORITY_THRESHOLD + 1
                                                            }).Build();

            _sut.Format(message, _growlNotification);
            Assert.That(_growlNotification.Priority == _expectedPriorityWhenHigher, Is.True, "Expected {0}, Actual {1}",
                _expectedPriorityWhenHigher, _growlNotification.Priority);
        }
    }

    [TestFixture]
    public class WhenNoPrioritiesHaveBeenSet : WhenUsingTheGrowlCountNotificationFinaliserBase
    {
        protected override Priority ExpectedPriorityWhenResultCountIsLower()
        {
            return Priority.Normal;
        }

        protected override Priority ExpectedPriorityWhenResultCountIsEqual()
        {
            return Priority.Normal;
        }

        protected override Priority ExpectedPriorityWhenResultCountIsHigher()
        {
            return Priority.Normal;
        }
    }

    [TestFixture]
    public class WhenSettingOnlyTheHigherPriority : WhenUsingTheGrowlCountNotificationFinaliserBase
    {
        protected override Priority ExpectedPriorityWhenResultCountIsLower()
        {
            return Priority.Normal;
        }

        protected override Priority ExpectedPriorityWhenResultCountIsEqual()
        {
            return Priority.Emergency;
        }

        protected override Priority ExpectedPriorityWhenResultCountIsHigher()
        {
            return Priority.Emergency;
        }
    }

    [TestFixture]
    public class WhenSettingOnlyTheLowerPriority : WhenUsingTheGrowlCountNotificationFinaliserBase
    {
        protected override Priority ExpectedPriorityWhenResultCountIsLower()
        {
            return Priority.Emergency;
        }

        protected override Priority ExpectedPriorityWhenResultCountIsEqual()
        {
            return Priority.Normal;
        }

        protected override Priority ExpectedPriorityWhenResultCountIsHigher()
        {
            return Priority.Normal;
        }
    }

    [TestFixture]
    public class WhenSettingTheHigherAndLowerPriority : WhenUsingTheGrowlCountNotificationFinaliserBase
    {
        protected override Priority ExpectedPriorityWhenResultCountIsLower()
        {
            return Priority.Moderate;
        }

        protected override Priority ExpectedPriorityWhenResultCountIsEqual()
        {
            return Priority.High;
        }

        protected override Priority ExpectedPriorityWhenResultCountIsHigher()
        {
            return Priority.High;
        }
    }
}