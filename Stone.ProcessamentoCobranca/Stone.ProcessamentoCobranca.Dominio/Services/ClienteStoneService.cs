using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Dominio.Repository.Interfaces;
using Stone.ProcessamentoCobranca.Dominio.Services.Interfaces;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Dominio.Services
{
    public class ClienteStoneService : IClienteStoneService
    {
        private readonly IClienteStoneQueryRepository _clienteStoneQueryRepository;
        public ClienteStoneService(IClienteStoneQueryRepository clienteStoneQueryRepository)
        {
            _clienteStoneQueryRepository = clienteStoneQueryRepository;
        }

        public async Task<IOperation<List<ClienteStone>>> ObterClientes(int pagina)
        {
            if (pagina <= 0)
                return CriarFalhaConsultaClientes(pagina);

            var clientes = await _clienteStoneQueryRepository.ConsultarClientes(pagina);
            if (clientes is null)
                return Result.CreateFailure<List<ClienteStone>>($"Houve um erro ao consultar a pagina {pagina}.");

            return Result.CreateSuccess(clientes);

        }

        private IOperation<List<ClienteStone>> CriarFalhaConsultaClientes(in int pagina)
        {
            return Result.CreateFailure<List<ClienteStone>>("Houve um erro ao iniciar a busca.",
                new DetalhesDaMensagem
                {
                    Campo = nameof(pagina),
                    Mensagem = "A pagina não pode ser menor ou igual a 0.",
                    Valor = pagina.ToString()
                });
        }
    }
}
