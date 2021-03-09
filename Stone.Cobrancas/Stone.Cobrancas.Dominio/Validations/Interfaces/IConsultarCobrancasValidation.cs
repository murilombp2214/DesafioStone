using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.Cobrancas.Dominio.Validations.Interfaces
{
    public interface IConsultarCobrancasValidation
    {
        IOperation<List<Cobranca>> Validar(string cpf, int pagina);
        IOperation<List<Cobranca>> Validar(int mes, int pagina);
    }
}
