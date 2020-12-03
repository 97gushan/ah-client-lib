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
            }
        }

        public JArray Orchestrate(string providerServiceDefinition)
        {
            JObject resp = Orchestrator.OrchestrateStatic(this.system);
            return (JArray)resp.SelectToken("response");
        }

        private void InitCoreSystems()
        {
            ServiceRegistry.InitServiceRegistry(this.settings);
            Authorization.InitAuthorization(this.settings);
            Orchestrator.InitOrchestrator(this.settings);
        }
    }
}
