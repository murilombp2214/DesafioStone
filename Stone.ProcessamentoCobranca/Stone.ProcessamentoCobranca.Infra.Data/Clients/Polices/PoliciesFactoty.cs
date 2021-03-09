using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.Polices.Contants;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.Polices.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.Polices
{
    public class PoliciesFactoty : IPoliciesFactoty
    {
        private static Lazy<IAsyncPolicy<HttpResponseMessage>> lazyRetry;
        private static Lazy<IAsyncPolicy<HttpResponseMessage>> lazyTimeout;
        private readonly ILogger<IPoliciesFactoty> _logger;

        public PoliciesFactoty(ILogger<IPoliciesFactoty> logger)
        {
            _logger = logger;
            ConfigureRetry();
            ConfigureTimeout();
        }

        private void ConfigureTimeout()
        {
            if (lazyTimeout is null)
            {
                int tempoTimeoutEmSegundos = int.Parse(Environment.GetEnvironmentVariable(PolicesContants.TIME_OUT) ?? "3");
                var time = TimeSpan.FromSeconds(tempoTimeoutEmSegundos);
                var policy = Policy.TimeoutAsync<HttpResponseMessage>(time, TimeoutStrategy.Optimistic, LogTimeout);
                lazyTimeout = new Lazy<IAsyncPolicy<HttpResponseMessage>>(() => policy, LazyThreadSafetyMode.ExecutionAndPublication);
            }
        }

        private Task LogTimeout(Context context, TimeSpan time, Task task)
        {
            return Task.Run(() => _logger.LogWarning("Politica de timeout iniciada."));
        }

        private void ConfigureRetry()
        {
            if (lazyRetry is null)
            {
                int qtdRetentativas = int.Parse(Environment.GetEnvironmentVariable(PolicesContants.NUMERO_RETENTATIVAS) ?? "3");
                var policy = HttpPolicyExtensions.HandleTransientHttpError()
                                                 .Or<TimeoutRejectedException>()
                                                 .OrResult(x => x.StatusCode == HttpStatusCode.NotFound)
                                                 .RetryAsync(qtdRetentativas, LogRetry);

                lazyRetry = new Lazy<IAsyncPolicy<HttpResponseMessage>>(() => policy, LazyThreadSafetyMode.ExecutionAndPublication);
            }
        }

        private void LogRetry(DelegateResult<HttpResponseMessage> @delegate, int vezRetry)
        {
            _logger.LogWarning($"Politica de Retry iniciada em {vezRetry}.");
        }

        public IAsyncPolicy<HttpResponseMessage> Retry() => lazyRetry.Value;

        public IAsyncPolicy<HttpResponseMessage> Timeout() => lazyTimeout.Value;
    }
}
