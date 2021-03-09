using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Dominio.Services.Interfaces
{
    public interface ICobrancaStoneService
    {
        public Task<IOperation<CobrancaStone>> RegistrarCobranca(CobrancaStone cobrancaStone);
    }
}
