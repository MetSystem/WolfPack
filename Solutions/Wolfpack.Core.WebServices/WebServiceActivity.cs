using System;
using System.Collections.Generic;
using ServiceStack.WebHost.Endpoints;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using ServiceStack.Common.Extensions;
using System.Linq;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Strategies;

namespace Wolfpack.Core.WebServices
{
    public class WebServiceActivity : AppHostHttpListenerBase, IActivityPlugin
    {
        private readonly WebServiceActivityConfig _config;
        private readonly IEnumerable<IWebServiceExtender> _extenders;

        public Status Status { get; set; }

        public WebServiceActivity(WebServiceActivityConfig config, IEnumerable<IWebServiceExtender> extenders)
            : base ("Wolfpack WebService", typeof(WebServiceActivity).Assembly)
        {
            _config = config;
            _extenders = extenders;

            _config.BaseUrl = _config.BaseUrl.TrimEnd('/');
            _config.BaseUrl += "/";
        }

        public void Initialise()
        {
            Logger.Info("Initialising Wolfpack WebService...", _config.BaseUrl);
            Logger.Debug("\t{0} WebService Extenders loaded...", _extenders.Count());
            _extenders.ForEach(ex => Logger.Debug("\t\t{0}", ex.GetType().Name));
            
            Init();
        }

        public override void Configure(Funq.Container container)
        {
            container.Register(_config);
            container.Register(c => Core.Container.Resolve<IWebServiceReceiverStrategy>());

            _extenders.ForEach(ex =>
                                   {
                                       try
                                       {
                                           ex.Add(this);
                                       }
                                       catch (Exception e)
                                       {
                                           Logger.Error(Logger.Event.During("WebService.Configure")
                                                     .Description("Executing extender {0}", ex.GetType().Name)
                                                     .Encountered(e));
                                           throw;
                                       }
                                   });         
        }

        public PluginDescriptor Identity
        {
            get
            {
                return new PluginDescriptor
                             {
                                 Name = "Wolfpack WebService",
                                 TypeId = new Guid("9CBDBFFF-1CA7-44C1-9E58-DF02B9017905"),
                                 Description = "Wolfpack WebService interface, powered by ServiceStack"
                             };
            }
        }

        public void Start()
        {
            Logger.Info("\t\tStarting Wolfpack WebService on {0}", _config.BaseUrl);
            Start(_config.BaseUrl);
        }

        public new void Stop()
        {
            Logger.Info("Stopping Wolfpack WebService...");
            base.Stop();
            Dispose();
            Logger.Info("Wolfpack WebService stopped");
        }

        public void Pause()
        {
            
        }

        public void Continue()
        {
            
        }

        public bool Enabled
        {
            get { return _config.Enabled; }
            set { _config.Enabled = value; }
        }
    }
}