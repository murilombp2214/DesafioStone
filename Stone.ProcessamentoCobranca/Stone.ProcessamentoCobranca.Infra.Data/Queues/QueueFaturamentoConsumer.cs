using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Dominio.Queues.Interfaces;
using Stone.ProcessamentoCobranca.Infra.Data.Queues.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Infra.Data.Queues
{
    public class QueueFaturamentoConsumer : IQueueFaturamentoConsumer
    {
        public IEnumerable<CobrancaStone> ObtenhaCobrancasNaoProcessadas()
        {
            while (QueueLocal.Instance.Any())
            {
                yield return QueueLocal.Instance.Dequeue();
            }
        }
    }
}
