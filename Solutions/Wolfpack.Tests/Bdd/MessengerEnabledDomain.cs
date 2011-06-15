
using Wolfpack.Core;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Tests.Mocks;
using NUnit.Framework;

namespace Wolfpack.Tests.Bdd
{
    public abstract class MessengerEnabledDomain : BddTestDomain
    {
        protected IMockMessenger MessengerMock { get; set; }

        protected MessengerEnabledDomain()
        {
            MessengerMock = new MockMessenger();
            Messenger.Initialise(MessengerMock);
        }

        protected HealthCheckResult ResultMessage(int index)
        {
            return (HealthCheckResult)MessengerMock.Sent[index];
        }

        public virtual void TheMessageShouldIndicateSuccess()
        {
            TheMessageAtPosition_ShouldHaveA_Result(0, true);
        }

        public virtual void TheMessageShouldIndicateFailure()
        {
            TheMessageAtPosition_ShouldHaveA_Result(0, false);
        }

        public virtual void TheMessageAtPosition_ShouldHaveA_Result(int index, bool expectedResult)
        {            
            Assert.That(ResultMessage(index).Check.Result == expectedResult, Is.True);
        }

        public virtual void ShouldHavePublished_Messages(int expected)
        {
            Assert.That(MessengerMock.Sent.Count, Is.EqualTo(expected));
        }

        public override void Dispose()
        {
            
        }
    }
}