using System.ServiceModel;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Wcf
{
    [ServiceContract(Namespace = "http://Wolfpack.iagileservices.com")]
    public interface IWolfpack
    {
        [OperationContract(IsOneWay = true)]
        void CaptureAgentStart(HealthCheckAgentStart session);

        [OperationContract(IsOneWay = true)]
        void CaptureResult(HealthCheckResult result);
        
    }
}