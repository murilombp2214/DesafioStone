using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;

namespace Stone.Cobrancas.Dominio.Validations.Interfaces
{
    public interface ICobrancaValidation
    {
        IOperation<Cobranca> Validar(Cobranca cobranca);
    }
}
