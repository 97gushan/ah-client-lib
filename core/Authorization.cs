using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Arrowhead.Utils;

namespace Arrowhead.Core
{
    public class Authorization
    {
        private Http http;
        private string baseUrl;

        public Authorization(Http http, Settings settings)
        {
            this.baseUrl = settings.getAuthorizationUrl() + "/authorization";
            this.http = http;
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
        public void Authorize(string consumerSystemId, string[] providerIds, string[] interfaceIds, string[] serviceDefinitionIds)
        {
            JObject payload = new JObject();
            payload.Add("consumerId", consumerSystemId);
            payload.Add("providerIds", new JArray(providerIds));
            payload.Add("interfaceIds", new JArray(interfaceIds));
            payload.Add("serviceDefinitionIds", new JArray(serviceDefinitionIds));

            HttpResponseMessage resp = this.http.Post(this.baseUrl, "/mgmt/intracloud", payload);
            resp.EnsureSuccessStatusCode();

            JObject respMessage = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
            if (respMessage.SelectToken("count").ToObject<int>() > 0)
            {
                Console.WriteLine("Intracloud ruleset created");
            }
            else
            {
                Console.WriteLine("Intracloud ruleset already exists, continuing...");
            }
        }

        public string GetPubicKey()
        {
            HttpResponseMessage resp = this.http.Get(this.baseUrl, "/publickey");
            string respMessage = resp.Content.ReadAsStringAsync().Result;
            return respMessage;
        }
    }
}
