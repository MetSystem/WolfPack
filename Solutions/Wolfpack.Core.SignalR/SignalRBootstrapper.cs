using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.SignalR.Interfaces.Entities;

namespace Wolfpack.Core.SignalR
{
    public class SignalRBootstrapper : IStartupPlugin, ICanBeSwitchedOff
    {
        public bool Enabled { get; set; }
        public Status Status { get; set; }

        public SignalRBootstrapper(SignalRActivityConfig config)
        {
            Enabled = config.Enabled;
        }

        /// <summary>
        /// Run only if this startup plugin is Enabled
        /// </summary>
        public void Initialise()
        {
            Container.RegisterAsSingleton<IActivityPlugin>(typeof(SignalRActivity));
        }
    }
}