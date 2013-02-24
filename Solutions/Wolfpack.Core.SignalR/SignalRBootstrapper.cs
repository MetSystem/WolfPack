using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.SignalR
{
    public class SignalRBootstrapper : IStartupPlugin
    {
        public Status Status { get; set; }

        public void Initialise()
        {
            Container.RegisterAsSingleton<IActivityPlugin>(typeof(SignalRActivity));
        }
    }
}