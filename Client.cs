using System;
using Arrowhead.Core;
using Arrowhead.Models;
using Arrowhead.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Arrowhead
{
    public class Client
    {
        private Service service;
        private Arrowhead.Models.System system;

        public Settings settings;

        /// <summary>
        /// Creates a Client object containing a Arrowhead service.
        /// This service is then registered to the Service Registry
        /// <remarks>
        /// If the service already exists in the registry then the stored entry is unregistered and 
        /// the service is reregistered
        /// </remarks>
        /// </summary>
        /// <param name="settings"></param>
        public Client(Settings settings)
        {
            this.settings = settings;
            this.InitCoreSystems();

            this.system = new Arrowhead.Models.System(this.settings.SystemName, this.settings.Ip, this.settings.Port, this.settings.getSSL());
            this.service = new Service(this.system, this.settings.ServiceDefinition, this.settings.Interfaces, this.settings.ApiUri);
            try
            {
                ServiceResponse serviceResp = (ServiceResponse)ServiceRegistry.RegisterService(this.service);
                Console.WriteLine(serviceResp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Could not register service " + this.settings.ServiceDefinition + " with the system " + this.settings.SystemName);
            }
        }

        /// <summary>
        /// Start orchestation 
        /// </summary>
        /// <remarks>
        /// If the response is empty then the Authenticators Intrarules entry and/or the Ochestration Store entry is wrongly configured
        /// </remarks>
        /// <param name="providerServiceDefinition"></param>
        /// <returns>A JSON array containing all available providers the client system has the rights to consume</returns>
        public JArray Orchestrate()
        {
            JObject resp = Orchestrator.OrchestrateStatic(this.system);
            try
            {
                return (JArray)resp.SelectToken("response");
            }
            catch (Exception e)
            {
                throw new Exception("Could not get Static Orchestration: \n" + resp.ToString());
            }
        }

        /// <summary>
        /// Initializes connection to the mandatory core systems
        /// </summary>
        private void InitCoreSystems()
        {
            ServiceRegistry.InitServiceRegistry(this.settings);
            Authorization.InitAuthorization(this.settings);
            Orchestrator.InitOrchestrator(this.settings);
        }
    }
}
