using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace Arrowhead.Utils
{
    public class Http
    {

        static HttpClient client;

        private string baseURI;

        /// <summary>
        /// Initialize a static HttpClient using the given certificate 
        /// </summary>
        /// <param name="certPath">Path to .p12 cert file</param>
        /// <param name="verifyCert">Should the certificate be validated? 
        /// if set to false then self-signed certs can be used</param>
        static void initHttp(string certPath, bool verifyCert)
        {
            HttpClientHandler handler = new HttpClientHandler();
            X509Certificate certificate = new X509Certificate2(certPath, "123456");
            handler.ClientCertificates.Add(certificate);

            if (!verifyCert)
            {
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            }
            client = new HttpClient(handler);
        }
        public Http(string baseURI, string certPath, bool verifyCert)
        {
            // only init the http client once, as it is static it can be used by every instance
            // without taking up more ports than needed
            if (client == null)
            {
                initHttp(certPath, verifyCert);
            }

            this.baseURI = baseURI;
        }

        public HttpResponseMessage Get(string apiEndpoint)
        {
            HttpResponseMessage response = client.GetAsync(this.baseURI + apiEndpoint).Result;
            return response;
        }

        public HttpResponseMessage Post(string apiEndpoint, Object payload)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(this.baseURI + apiEndpoint, content).Result;
            return response;
        }
    }
}
