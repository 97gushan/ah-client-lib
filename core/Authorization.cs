using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Arrowhead.Models;
using Arrowhead.Utils;

namespace Arrowhead.Core
{
    public class Authorization
    {
        static Http http;

        public static void InitAuthorization(Settings settings)
        {
            string baseUrl = settings.getAuthorizationUrl() + "/authorization";
            http = new Http(baseUrl, settings.CertificatePath, settings.VerifyCertificate);
        }

        public static string Authorize(string consumerId, string[] providerIds, string[] interfaceIds, string[] serviceDefinitionIds)
        {
            JObject payload = new JObject();
            payload.Add("consumerId", consumerId);
            payload.Add("providerIds", new JArray(providerIds));
            payload.Add("interfaceIds", new JArray(interfaceIds));
            payload.Add("serviceDefinitionIds", new JArray(serviceDefinitionIds));
            try
            {
                HttpResponseMessage resp = http.Post("/mgmt/intracloud", payload);
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                return respMessage;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        public static string GetPubicKey()
        {
            HttpResponseMessage resp = http.Get("/publickey");
            string respMessage = resp.Content.ReadAsStringAsync().Result;
            return respMessage;
        }
    }
}
