using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Constants;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Configuration
{
    public static class ConfigurationApiCobranca
    {
        public static void ConfigureHttpClient(HttpClient httpClient)
        {
            string baseurl = Environment.GetEnvironmentVariable(ContantsApiCobranca.BASE_URL);

            httpClient.BaseAddress = new Uri(baseurl);
            httpClient.DefaultRequestHeaders.Add(ContantsApiCobranca.CORRELATION_ID, Guid.NewGuid().ToString());

        }
    }
}
