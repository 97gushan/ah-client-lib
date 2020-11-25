using System;
using System.Net.Http;
using Newtonsoft.Json;
using Arrowhead.Models;
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
            Newtonsoft.Json.Linq.JObject payload = new Newtonsoft.Json.Linq.JObject();
            Newtonsoft.Json.Linq.JObject requestedService = new Newtonsoft.Json.Linq.JObject();
            Newtonsoft.Json.Linq.JArray interfaces = new Newtonsoft.Json.Linq.JArray();
            Newtonsoft.Json.Linq.JArray security = new Newtonsoft.Json.Linq.JArray();
            interfaces.Add("HTTPS-SECURE-JSON");
            security.Add("TOKEN");
            requestedService.Add("serviceDefinitionRequirement", requestedServiceDefinition);
            requestedService.Add("interfaceRequirements", interfaces);
            requestedService.Add("securityRequirements", security);
            payload.Add("requesterSystem", JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(JsonConvert.SerializeObject(consumer)));
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
