using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Entities;

namespace Wolfpack.Core.WebServices.Strategies.Steps
{
    public class RepublishMessageStep : StepBase<WebServiceReceiverContext>
    {
        public override void Execute(WebServiceReceiverContext context)
        {
            Messenger.Publish(context.Notification);
        }
    }
}