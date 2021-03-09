using Stone.Cobrancas.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stone.Cobrancas.Dominio.Repository.Interface
{
    public interface ICobrancaQueryRepository
    {
        Task<List<Cobranca>> ConsultarCobrancas(string cpf, int pagina);
        Task<List<Cobranca>> ConsultarCobrancas(int mes, int pagina);
    }
}
