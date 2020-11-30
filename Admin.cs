using System;
using Arrowhead.Core;
using Arrowhead.Models;
using Arrowhead.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Arrowhead
{
    public class Admin
    {
        public Settings settings;

        public Admin(string consumerSystemId, string producerSystemDefinition, string systemName, Settings settings)
        {
            this.settings = settings;

            this.InitCoreSystems();

            JObject providerSystem = new JObject();
            providerSystem.Add("systemName", "test_producer");
            providerSystem.Add("address", "127.0.0.1");
            providerSystem.Add("port", 8080);
            providerSystem.Add("id", "18");

            JObject cloud = new JObject();
            cloud.Add("operator", "aitia");
            cloud.Add("name", "testcloud2");

            string interfaceName = "HTTPS-SECURE-JSON";

            try
            {
                Console.WriteLine(Authorization.Authorize(consumerSystemId, new string[] { "18" }, new string[] { "3" }, new string[] { "13" }));
                Console.WriteLine(Orchestrator.StoreOrchestrate(consumerSystemId, producerSystemDefinition, interfaceName, providerSystem, cloud));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void InitCoreSystems()
        {
            ServiceRegistry.InitServiceRegistry(this.settings);
            Authorization.InitAuthorization(this.settings);
            Orchestrator.InitOrchestrator(this.settings);
        }
    }
}
