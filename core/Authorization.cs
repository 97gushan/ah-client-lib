using System;
using System.Net.Http;
using Newtonsoft.Json;
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
    }
}
