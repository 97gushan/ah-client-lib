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

        public Client(string serviceName, string systemName, Settings settings)
        {
            this.settings = settings;

            this.InitCoreSystems();

            this.system = new Arrowhead.Models.System(systemName, "127.0.0.1", "8081", this.settings.getSSL());
            this.service = new Service(this.system, serviceName, new string[] { "HTTPS-SECURE-JSON" }, "/test");
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

        public JArray Orchestrate(string producerSystemName)
        {
            JObject resp = JsonConvert.DeserializeObject<JObject>(Orchestrator.OrchestrateStatic(this.system, producerSystemName));
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
