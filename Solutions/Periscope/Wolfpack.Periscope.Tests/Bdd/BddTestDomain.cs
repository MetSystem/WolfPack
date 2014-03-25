using System;
using System.Threading;
using NUnit.Framework;

namespace Wolfpack.Periscope.Tests.Bdd
{
    public abstract class BddTestDomain : IDisposable
    {
        protected Exception _expectedException;

        public abstract void Dispose();

        public void SafeExecute(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                _expectedException = ex;
            }
        }

        public void SimulateBackgroundEvent(Action action, int interval, Action asyncAction)
        {
            var bgThread = new Thread((() =>
            {
                Thread.Sleep(interval);
                asyncAction.Invoke();
            }));
            bgThread.Start();
            action.Invoke();
        }

        public void ThrewNoException()
        {
            Assert.That(_expectedException, Is.Null);
        }

        public void ShouldThrow_Exception(Type expected)
        {
            Assert.That(_expectedException, Is.Not.Null);
            Assert.That(_expectedException, Is.TypeOf(expected));
        }

        public void _ShouldBeInTheExceptionMesssage(string content)
        {
            Assert.That(_expectedException.Message, Contains.Substring(content));
        }
    }
}