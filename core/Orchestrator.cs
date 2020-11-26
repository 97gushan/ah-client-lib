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

        public static string Orchestrate(Arrowhead.Models.System consumer, string requestedServiceDefinition)
        {
            JObject payload = new JObject();
            JObject requestedService = new JObject();
            JArray interfaces = new JArray();
            JArray security = new JArray();
            interfaces.Add("HTTPS-SECURE-JSON");
            security.Add("TOKEN");
            requestedService.Add("serviceDefinitionRequirement", requestedServiceDefinition);
            requestedService.Add("interfaceRequirements", interfaces);
            requestedService.Add("securityRequirements", security);
            payload.Add("requesterSystem", JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(consumer)));
            payload.Add("requestedService", requestedService);

            try
            {
                HttpResponseMessage resp = http.Post("orchestration", payload);
                resp.EnsureSuccessStatusCode();
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                return respMessage;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }
    }
}
