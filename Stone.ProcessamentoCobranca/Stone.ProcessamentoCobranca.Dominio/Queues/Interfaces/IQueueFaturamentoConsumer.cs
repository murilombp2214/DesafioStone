﻿using Stone.ProcessamentoCobranca.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Dominio.Queues.Interfaces
{
    public interface IQueueFaturamentoConsumer
    {
        public IEnumerable<CobrancaStone> ObtenhaCobrancasNaoProcessadas();
    }
}
