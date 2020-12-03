using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Arrowhead.Utils;

namespace Arrowhead.Core
{
    public class Authorization
    {
        static Http http;

        public static void InitAuthorization(Settings settings)
        {
            string baseUrl = settings.getAuthorizationUrl() + "/authorization";
            http = new Http(baseUrl, settings.CertificatePath, settings.CertificatePassword, settings.VerifyCertificate);
        }


        /// <summary>
        /// Add a intracloud Authorization ruleset for a consumer system to a list of provider systems with a list of service definitions and interaces
        /// </summary>
        /// <remarks>
        /// NOTE This is a call to the Management endpoint of the Authorization, thus it requires a Sysop Certificate
        /// This can also be done via the Management Tool on the Arrowhead Tools Github page
        /// https://github.com/arrowhead-tools/mgmt-tool-js
        /// </remarks>
        /// <param name="consumerSystemId"></param>
        /// <param name="providerIds"></param>
        /// <param name="interfaceIds"></param>
        /// <param name="serviceDefinitionIds"></param>
        /// <returns></returns>
        public static string Authorize(string consumerSystemId, string[] providerIds, string[] interfaceIds, string[] serviceDefinitionIds)
        {
            JObject payload = new JObject();
            payload.Add("consumerId", consumerSystemId);
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
