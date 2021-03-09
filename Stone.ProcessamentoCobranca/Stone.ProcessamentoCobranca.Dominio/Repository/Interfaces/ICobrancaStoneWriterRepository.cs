using Stone.ProcessamentoCobranca.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Dominio.Repository.Interfaces
{
    public interface ICobrancaStoneWriterRepository
    {
        public Task<CobrancaStone> RegistrarCobranca(CobrancaStone cobrancaStone);
    }
}