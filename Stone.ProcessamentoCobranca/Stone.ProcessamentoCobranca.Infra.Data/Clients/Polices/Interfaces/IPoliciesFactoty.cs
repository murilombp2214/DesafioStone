using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.Polices.Interfaces
{
    public interface IPoliciesFactoty
    {
        IAsyncPolicy<HttpResponseMessage> Retry();
        IAsyncPolicy<HttpResponseMessage> Timeout();
    }
}
