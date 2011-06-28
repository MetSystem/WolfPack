
using System;
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

        protected T ResultMessage<T>(int index)
        {
            return (T)MessengerMock.Sent[index];
        }

        private static bool? GetDataResult(HealthCheckData msg)
        {
            return msg.Result;
        }

        private static bool? GetResult(HealthCheckResult msg)
        {
            return msg.Check.Result;
        }

        public virtual void TheDataMessageShouldIndicateSuccess()
        {
            TheMessageAtPosition_ShouldHaveA_Result<HealthCheckData>(0, true, GetDataResult);
        }

        public virtual void TheResultMessageShouldIndicateSuccess()
        {
            TheMessageAtPosition_ShouldHaveA_Result<HealthCheckResult>(0, true, GetResult);
        }

        public virtual void TheDataMessageShouldIndicateFailure()
        {
            TheMessageAtPosition_ShouldHaveA_Result<HealthCheckData>(0, false, GetDataResult);
        }

        public virtual void TheResultMessageShouldIndicateFailure()
        {
            TheMessageAtPosition_ShouldHaveA_Result<HealthCheckResult>(0, false, GetResult);
        }

        public virtual void TheMessageAtPosition_ShouldHaveA_Result<T>(int index, bool expectedResult, Func<T, bool?> getResult)
        {            
            Assert.That(getResult(ResultMessage<T>(index)) == expectedResult, Is.True);
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