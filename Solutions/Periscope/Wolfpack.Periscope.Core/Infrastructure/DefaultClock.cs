using System;
using System.Threading;
using System.Threading.Tasks;
using Wolfpack.Periscope.Core.Interfaces.Infrastructure;

namespace Wolfpack.Periscope.Core.Infrastructure
{
    public class DefaultClock : IClock, IDisposable
    {
        private int _intervalMilliseconds;
        private Task _bgTask;

        private readonly ManualResetEvent _waitGate;
        
        public DefaultClock()
        {
            _intervalMilliseconds = 1000;
            _waitGate = new ManualResetEvent(false);
        }

        public void Start(Action callback)
        {
            _bgTask = new Task(() =>
                                      {
                                          while (true)
                                          {
                                              if (_waitGate.WaitOne(_intervalMilliseconds))
                                              {
                                                  // exit immediately
                                                  break;
                                              }

                                              callback();
                                          }                                          
                                      });

            _bgTask.Start();
        }

        public void Dispose()
        {
            _waitGate.Set();
        }

        public void SetIntervalInMilliseconds(int interval)
        {
            _intervalMilliseconds = interval;
        }
    }
}