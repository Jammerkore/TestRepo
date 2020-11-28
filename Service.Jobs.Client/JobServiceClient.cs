using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

//using Newtonsoft.Json;

//using Logility.Foundation.Core.Discovery;
using Logility.ROServices;
using Logility.ROWebSharedTypes;

namespace Logility.ROServices
{
    public class ROWebJobServiceClient : IDisposable
    {
        private static readonly ConcurrentDictionary<string, ChannelFactory<IROWebJobService>> Factories = new ConcurrentDictionary<string, ChannelFactory<IROWebJobService>>();

        private const string JOB_SERVICE_ENDPOINT_NAME = "Logility.ROServices.ROWebJobService";
        private readonly IROWebJobService jobServiceChannel;

        public ROWebJobServiceClient()
        {
            jobServiceChannel = GetChannelFactory(JOB_SERVICE_ENDPOINT_NAME).CreateChannel();
        }

        public ROWebJobServiceClient(string sClientEndpointName)
        {
            //Uri jobServiceUri = GetJobServiceUri(sClientEndpointName);
            //EndpointAddress address = new EndpointAddress(jobServiceUri);
            //ChannelFactory<IROWebJobService> channelFactory = new ChannelFactory<IROWebJobService>(JOB_SERVICE_ENDPOINT_NAME, address);

            //jobServiceChannel = channelFactory.CreateChannel();
            jobServiceChannel = GetChannelFactory(sClientEndpointName).CreateChannel();
        }

        private Uri GetJobServiceUri(string sEnvironmentName)
        {
            //ServiceLocator locator = new ServiceLocator();
            //Task<ServiceInfo> lookupTask = locator.GetServiceInfoAsync(sEnvironmentName);

            //lookupTask.Wait();

            //string instanceUrl = lookupTask.Result?.ToString();

            //if (string.IsNullOrEmpty(instanceUrl))
            //{
            //    throw new Exception("Unable to find RO Job Service for environment " + sEnvironmentName);
            //}

            //return new Uri(instanceUrl);
            return null;
        }

        private static ChannelFactory<IROWebJobService> GetChannelFactory(string sClientEndpointName)
        {
            if (string.IsNullOrEmpty(sClientEndpointName))
            {
                throw new ArgumentException("Must supply an actual endpoint name");
            }

            ChannelFactory<IROWebJobService> factory = Factories.GetOrAdd(sClientEndpointName, endpoint => new ChannelFactory<IROWebJobService>(endpoint));
            return factory;
        }

        public void Dispose()
        {
            System.ServiceModel.Channels.IChannel channel = jobServiceChannel as System.ServiceModel.Channels.IChannel;
            if (channel == null)
            {
                return;
            }

            try
            {
                if (channel.State != CommunicationState.Faulted)
                {
                    channel.Close();
                }
            }
            finally
            {
                if (channel.State != CommunicationState.Closed)
                {
                    channel.Abort();
                }
            }
        }

        public string GetJobID()
        {
            return jobServiceChannel.GetJobID();
        }

        public string TestEventLog()
        {
            return jobServiceChannel.TestEventLog();
        }

        public double GetMachineAvailability(out string sPort)
        {
            return jobServiceChannel.GetMachineAvailability(out sPort);
        }

        public eROConnectionStatus StartROWebHost(string sROUserID, string sPassword, string sROSessionID, string sPort, out string sProcessDescription)
        {
            return jobServiceChannel.StartROWebHost(sROUserID, sPassword, sROSessionID, sPort, out sProcessDescription);
        }

        public bool ConnectToServices(string sROUserID, string sPassword, string sROSessionID, out string sProcessDescription)
        {
            eROConnectionStatus connectionStatus = jobServiceChannel.ConnectToServicesWithStatus(sROUserID, sPassword, sROSessionID, out sProcessDescription);
            switch (connectionStatus)
            {
                case eROConnectionStatus.Successful:
                case eROConnectionStatus.SuccessfulBatchOnlyMode:
                    return true;
                default:
                    return false;
            }
        }

        public eROConnectionStatus ConnectToServicesWithStatus(string sROUserID, string sPassword, string sROSessionID, out string sProcessDescription)
        {
            return jobServiceChannel.ConnectToServicesWithStatus(sROUserID, sPassword, sROSessionID, out sProcessDescription);
        }

        public bool KeepAlive(string sROUserID, string sROSessionID)
        {
            return jobServiceChannel.KeepAlive(sROUserID, sROSessionID);
        }

        public void DisconnectUser(string sROUserID)
        {
            jobServiceChannel.DisconnectUser(sROUserID);
        }

        public void DisconnectSession(string sROUserID, string sROSessionID)
        {
            jobServiceChannel.DisconnectSession(sROUserID, sROSessionID);
        }

        public ROOut ProcessRequest(ROParms Parms)
        {
            ROOut result = jobServiceChannel.ProcessRequest(Parms);

            //if (result.ROReturnCode != eROReturnCode.Successful)
            //{
            //    string sMsg = string.Format("Call to {0} failed: {1}", Parms.RORequest.ToString(), result.ROMessage);
            //    throw new Exception(sMsg);
            //}

            return result;
        }

        public void GetServiceServerInfo(string sROUserID, string sROSessionID, out string appSetControlServer, out string appSetLocalStoreServer, out string appSetLocalHierarchyServer, out string appSetLocalApplicationServer)
        {
            jobServiceChannel.GetServiceServerInfo(sROUserID, sROSessionID, out appSetControlServer, out appSetLocalStoreServer, out appSetLocalHierarchyServer, out appSetLocalApplicationServer);
        }
    }
}
