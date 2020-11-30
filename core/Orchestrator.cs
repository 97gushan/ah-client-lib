using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Arrowhead.Utils;

namespace Arrowhead.Core
{
    public class Orchestrator
    {
        static Http http;

        public static void InitOrchestrator(Settings settings)
        {
            string baseUrl = settings.getOrchestratorUrl() + "/orchestrator";
            http = new Http(baseUrl, settings.CertificatePath, settings.VerifyCertificate);
        }

        public static string OrchestrateStatic(Arrowhead.Models.System consumer, string requestedServiceDefinition)
        {
            JObject payload = new JObject();
            JObject requesterSystem = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(consumer));
            requesterSystem.Remove("id");

            JObject orchestrationFlags = new JObject();
            orchestrationFlags.Add("overrideStore", false);

            payload.Add("requesterSystem", requesterSystem);
            payload.Add("orchestrationFlags", orchestrationFlags);

            Console.WriteLine(payload);
            try
            {
                HttpResponseMessage resp = http.Post("/orchestration", payload);
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                Console.WriteLine(resp.StatusCode);

                return respMessage;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        public static string StoreOrchestrate(string consumerSystemId, string requestedServiceDefinition, string serviceInterfaceName, JObject providerSystem, JObject cloud)
        {
            JObject entry = new JObject();
            entry.Add("serviceDefinitionName", requestedServiceDefinition);
            entry.Add("consumerSystemId", consumerSystemId);
            entry.Add("cloud", cloud);
            entry.Add("providerSystem", providerSystem);
            entry.Add("serviceInterfaceName", serviceInterfaceName);
            entry.Add("priority", 1);

            // the api takes the input as a list of new entries
            JArray payload = new JArray();
            payload.Add(entry);

            try
            {
                HttpResponseMessage resp = http.Post("/mgmt/store", payload);
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                return respMessage;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        public static string GetOrchestrationById(string id)
        {
            HttpResponseMessage resp = http.Get("/orchestration/" + id);
            string respMessage = resp.Content.ReadAsStringAsync().Result;
            return respMessage;
        }
    }
}
