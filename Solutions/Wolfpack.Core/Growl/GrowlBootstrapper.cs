using System;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Growl
{
    public class GrowlBootstrapper : IStartupPlugin, ICanBeSwitchedOff
    {
        protected readonly GrowlConfiguration _config;
        protected readonly PluginDescriptor _identity;

        public GrowlBootstrapper(GrowlConfiguration config)
        {
            _config = config;
            _identity = new PluginDescriptor
                             {
                                 Description = string.Format("Initalises a Growl connection"),
                                 TypeId = new Guid("D040058E-8E88-49d2-B9E1-4794CE92702F"),
                                 Name = "GrowlConnector"
                             };
        }

        public PluginDescriptor Identity
        {
            get { return _identity; }
        }


        public bool Enabled
        {
            get { return _config.Enabled; }
            set { _config.Enabled = value; }
        }

        public Status Status { get; set; }

        public void Initialise()
        {
            if (!Enabled)
                return;

            Container.RegisterAsSingleton<IGrowlConnection>(typeof (GrowlConnection));
        }
    }
}