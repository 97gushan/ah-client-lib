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
    }
}
