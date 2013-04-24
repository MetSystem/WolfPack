using System;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Exceptions;

namespace Wolfpack.Core.WebServices.Strategies.Steps
{
    public class MessageStalenessCheckStep : StepBase<WebServiceReceiverContext>
    {
        private readonly MessageStalenessCheckConfig _config;

        public MessageStalenessCheckStep(MessageStalenessCheckConfig config)
        {
            _config = config;
        }

        public override void Execute(WebServiceReceiverContext context)
        {
            if (DateTime.UtcNow.Subtract(context.Notification.GeneratedOnUtc).TotalMinutes > _config.MaxAgeInMinutes)
            {
                Logger.Info("Message '{0}' is stale ({1} minutes old), discarding it!",
                    context.Notification.Id, DateTime.UtcNow.Subtract(context.Notification.GeneratedOnUtc).TotalMinutes);
                throw new StaleMessageException(context.Notification.Id);
            }
        }
    }
}