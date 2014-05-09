using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Exceptions;

namespace Wolfpack.Core.WebServices.Strategies.Steps
{
    public class CheckForDuplicateStep : StepBase<WebServiceReceiverContext>
    {
        private readonly INotificationRepository _repository;

        public CheckForDuplicateStep(INotificationRepository repository)
        {
            _repository = repository;
        }

        public override void Execute(WebServiceReceiverContext context)
        {
            NotificationEvent notification;
            if (_repository.GetById(context.Notification.Id, out notification))
            {
                Logger.Info("Message '{0}' has already been received (on {1}), discarding it!", context.Notification.Id,
                    notification.ReceivedOnUtc);
                throw new DuplicateMessageException(context.Notification.Id);
            }
        }
    }
}