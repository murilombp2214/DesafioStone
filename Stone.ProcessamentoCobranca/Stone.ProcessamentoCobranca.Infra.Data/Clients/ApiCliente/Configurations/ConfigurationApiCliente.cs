using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCliente.Constants;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCliente.Configurations
{
    public static class ConfigurationApiCliente
    {
        public static void ConfigureHttpClient(HttpClient httpClient)
        {
            string baseurl = Environment.GetEnvironmentVariable(ContantsApiCliente.BASE_URL);

            httpClient.BaseAddress = new Uri(baseurl);
            httpClient.DefaultRequestHeaders.Add(ContantsApiCliente.CORRELATION_ID, Guid.NewGuid().ToString());

        }
    }
}
