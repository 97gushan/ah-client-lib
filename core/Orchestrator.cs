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
            http = new Http(baseUrl, settings.CertificatePath, settings.CertificatePassword, settings.VerifyCertificate);
        }

        /// <summary>
        /// Start static orchestration by fetching provider data from the Orchestrator.
        /// This will provide a list of 
        /// </summary>
        /// <remarks>
        /// This is a call to the Client endpoint of the Orchestrator API thus it requires:
        /// <list>
        /// <item> A certificate for the consumer system</item>
        /// <item> An intracloud ruleset stored in the Authenticator by an Admin via the management endpoint or the Management Tools</item>
        /// <item> An store entry in the Orchestrator by an Admin via the management endpoint or the Management Tools</item>
        /// </list>
        /// </remarks>
        /// <param name="consumer"></param>
        /// <returns></returns>
        public static JObject OrchestrateStatic(Arrowhead.Models.System consumer)
        {
            JObject payload = new JObject();
            JObject requesterSystem = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(consumer));
            requesterSystem.Remove("id");

            JObject orchestrationFlags = new JObject();
            orchestrationFlags.Add("overrideStore", false);

            payload.Add("requesterSystem", requesterSystem);
            payload.Add("orchestrationFlags", orchestrationFlags);

            HttpResponseMessage resp = http.Post("/orchestration", payload);
            string respMessage = resp.Content.ReadAsStringAsync().Result;
            JObject result = JsonConvert.DeserializeObject<JObject>(respMessage);
            return result;
        }

        /// <summary>
        /// Add a store entry to the orchestration store, this is needed for the Consumer to 
        /// be able to fetch the orchestration information about the Provider from the Orchestrator
        /// </summary>
        /// <remarks>
        /// NOTE This is a call to the Management endpoint of the Orchestrator, thus it requires a Sysop Certificate
        /// This can also be done via the Management Tool on the Arrowhead Tools Github page
        /// https://github.com/arrowhead-tools/mgmt-tool-js
        /// </remarks>
        /// <param name="consumerSystemId"></param>
        /// <param name="requestedServiceDefinition"></param>
        /// <param name="serviceInterfaceName"></param>
        /// <param name="providerSystem"></param>
        /// <param name="cloud"></param>
        /// <returns></returns>
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
