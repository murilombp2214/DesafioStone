using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Dominio.Services.Interfaces
{
    public interface IFaturamentoStoneService
    {
        public Task GerarCobrancasParaClientesStone();
        public Task GerarCobrancasParaClientesStoneEnfilerados();
    }
}
