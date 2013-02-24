using System;
using System.Threading.Tasks;
using Magnum.Pipeline;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.SignalR.Interfaces.Entities;

namespace Wolfpack.Core.SignalR
{
    public class SignalRActivity : IActivityPlugin, IConsumer<NotificationEvent>
    {
        private readonly SignalRActivityConfig _config;

        private IDisposable _appHost;

        public Status Status { get; set; }

        public SignalRActivity(SignalRActivityConfig config)
        {
            _config = config;

            //_config.BaseUrl = _config.BaseUrl.TrimEnd('/');
            //_config.BaseUrl += "/";
        }

        public void Initialise()
        {
            Logger.Info("Initialising Wolfpack SignalR host...", _config.BaseUrl);
            Messenger.Subscribe(this);
        }

        public PluginDescriptor Identity
        {
            get
            {
                return new PluginDescriptor
                             {
                                 Name = "Wolfpack SignalR Host",
                                 TypeId = new Guid("B672BE60-7D84-4E1C-A287-C387F5845F07"),
                                 Description = "Wolfpack SignalR Hosting Activity"
                             };
            }
        }

        public void Start()
        {
            Logger.Info("\t\tStarting Wolfpack SignalR host on {0}", _config.BaseUrl);

            _appHost = WebApplication.Start<Startup>(_config.BaseUrl);
        }

        public void Stop()
        {
            if (_appHost != null)
                _appHost.Dispose();
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

        public void Consume(NotificationEvent message)
        {
            var context = GlobalHost.ConnectionManager.GetConnectionContext<MyEndpoint>();
            context.Connection.Broadcast(message);
        }
    }

    public class MyEndpoint : PersistentConnection
    {
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            Logger.Debug("Connection id:={0} connected!", connectionId);            
            return base.OnConnected(request, connectionId);
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapConnection<MyEndpoint>("/notifications");
        }
    }
}