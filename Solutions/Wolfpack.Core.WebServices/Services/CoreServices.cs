﻿using System.Collections.Generic;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Messages;
using Status = Wolfpack.Core.WebServices.Interfaces.Messages.Status;

namespace Wolfpack.Core.WebServices.Services
{
    [ClientCanSwapTemplates]
    public class CoreServices : Service
    {
        private readonly IWebServiceReceiverStrategy _strategy;

        public CoreServices(IWebServiceReceiverStrategy strategy)
        {
            _strategy = strategy;
        }

        public object Get(Status request)
        {
            var response = new StatusResponse {Status = "Initialising"};

            if (Container.IsRegistered<NotificationEventAgentStart>())
            {
                response.Info = Container.Resolve<NotificationEventAgentStart>();
                response.Status = "Running";
            }

            return response;
        }

        /// <summary>
        /// Receive (and rebroadcast to publishers) a Start message
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public NotificationEventResponse Post(NotificationEvent request)
        {            
            //Messenger.Publish(request);
            Logger.Info("Received Notification ({0}) {1}", request.EventType, request.Id);
            _strategy.Execute(request);
            return new NotificationEventResponse();
        }

        public object Get(HealthCheckArtifact request)
        {
            var result = new HttpResult(ArtifactManager.Get(request.Name, request.NotificationId), request.ContentType);
            result.Headers.Add("Content-Disposition", string.Format("attachment; filename=wolfpack-{0}.txt", request.NotificationId));
            return result;
        }

        public object Get(Activity request)
        {
            return new ActivityResponse
                       {
                           CurrentNotifications = new List<NotificationEvent>()
                       };
        }
    }
}