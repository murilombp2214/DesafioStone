using Stone.Clientes.Dominio.Constants;
using Stone.Clientes.Dominio.Entities;
using Stone.Clientes.Dominio.Validations.Interfaces;
using Stone.Clientes.Infra.CrossCutting.Utils;
using Stone.Clientes.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Stone.Clientes.Dominio.Validations
{
    public class ClienteValidation : IClienteValidation
    {
        private readonly ICpfValidation _cpfValidation;

        public ClienteValidation(ICpfValidation cpfValidation)
        {
            _cpfValidation = cpfValidation;
        }

        public IOperation<Cliente> Validar(Cliente cliente)
        {
            if (cliente is null)
                return Result.CreateFailure<Cliente>("A entidade não pode ser vazia.");

            var erros = new List<DetalhesDaMensagem>();

            ValidaDadosVazios(cliente, erros);
            ValidarCpf(cliente.Cpf, erros);

            if (erros.Any())
                return Result.CreateFailure<Cliente>("Houve um erro ao validar os dados do cliente.", erros);

            return Result.CreateSuccess<Cliente>();

        }

        private void ValidarCpf(in string cpf, List<DetalhesDaMensagem> erros)
        {
            if (string.IsNullOrEmpty(cpf))
            {
                erros.Add(new DetalhesDaMensagem
                {
                    Campo = nameof(cpf),
                    Valor = cpf,
                    Mensagem = "CPF não pode ser vazio."
                });
            }
            else
            {
                if (!_cpfValidation.Validar(cpf))
                {
                    erros.Add(new DetalhesDaMensagem
                    {
                        Campo = nameof(cpf),
                        Valor = cpf,
                        Mensagem = "O CPF informado é invalido."

                    });
                }
            }
        }

        private void ValidaDadosVazios(Cliente cliente, List<DetalhesDaMensagem> erros)
        {
            if (string.IsNullOrEmpty(cliente.Nome))
            {
                erros.Add(new DetalhesDaMensagem
                {
                    Campo = nameof(cliente.Nome).ToLower(),
                    Valor = cliente.Nome,
                    Mensagem = "Nome não pode ser vazio."

                });
            }
            else
            {
                if (cliente.Nome.Length > ClienteConstants.TAMANHO_MAX_NOME)
                {
                    erros.Add(new DetalhesDaMensagem
                    {
                        Campo = nameof(cliente.Nome).ToLower(),
                        Valor = cliente.Nome ,
                        Mensagem = $"Nome não pode ser maior que {ClienteConstants.TAMANHO_MAX_NOME}."

                    });
                }
            }

            if (string.IsNullOrEmpty(cliente.Estado))
            {
                erros.Add(new DetalhesDaMensagem
                {
                    Campo = nameof(cliente.Estado).ToLower(),
                    Valor = cliente.Estado,
                    Mensagem = "Estado não pode ser vazio."
                });
            }
            else
            {
                if (cliente.Estado.Length > ClienteConstants.TAMANHO_MAX_ESTADO)
                {
                    erros.Add(new DetalhesDaMensagem
                    {
                        Campo = nameof(cliente.Estado).ToLower(),
                        Valor = cliente.Estado,
                        Mensagem = $"Estado não pode ser maior que {ClienteConstants.TAMANHO_MAX_ESTADO}."

                    });
                }
            }

        }
    }
}
