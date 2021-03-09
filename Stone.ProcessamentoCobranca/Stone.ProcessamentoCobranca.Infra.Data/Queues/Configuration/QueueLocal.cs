using Stone.ProcessamentoCobranca.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.ProcessamentoCobranca.Infra.Data.Queues.Configuration
{
    public static class QueueLocal
    {
        public static readonly Queue<CobrancaStone> Instance = new Queue<CobrancaStone>();
    }
}
