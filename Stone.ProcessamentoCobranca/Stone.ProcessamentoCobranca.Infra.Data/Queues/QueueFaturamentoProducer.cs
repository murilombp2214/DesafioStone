using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Dominio.Queues.Interfaces;
using Stone.ProcessamentoCobranca.Infra.Data.Queues.Configuration;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Infra.Data.Queues
{
    public class QueueFaturamentoProducer : IQueueFaturamentoProducer
    {
        public Task EnfilerarCobranca(CobrancaStone cobranca)
        {
            lock(QueueLocal.Instance)
            {
                QueueLocal.Instance.Enqueue(cobranca);
            }

            return Task.CompletedTask;
        }
    }
}
