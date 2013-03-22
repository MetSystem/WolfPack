using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Notification
{
    public class NotificationHubBootstrapper : IStartupPlugin
    {
        public Status Status { get; set; }

        public void Initialise()
        {
            Container.RegisterAll<INotificationRequestFilter>();

            if (!Container.IsRegistered<INotificationHub>())
            {
                Logger.Info("Registering default NotificationHub");
                Container.RegisterInstance<INotificationHub>(
                    new NotificationHub(Container.ResolveAll<INotificationRequestFilter>()));
            }            
        }
    }
}