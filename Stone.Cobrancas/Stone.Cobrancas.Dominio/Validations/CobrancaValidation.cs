using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Dominio.Validations.Interfaces;
using Stone.Cobrancas.Infra.CrossCutting.Utils;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stone.Cobrancas.Dominio.Validations
{
    public class CobrancaValidation : ICobrancaValidation
    {
        private readonly ICpfValidation _cpfValidation;

        public CobrancaValidation(ICpfValidation cpfValidation)
        {
            _cpfValidation = cpfValidation;
        }

        public IOperation<Cobranca> Validar(Cobranca cobranca)
        {
            if (cobranca is null)
                return Result.CreateFailure<Cobranca>("A entidade não pode ser vazia.");

            var erros = new List<DetalhesDaMensagem>();

            ValidaDadosVazios(cobranca, erros);
            ValidarCpf(cobranca.Cpf, erros);

            if (erros.Any())
                return Result.CreateFailure<Cobranca>("Houve um erro ao validar os dados da cobrança.", erros);

            return Result.CreateSuccess<Cobranca>();


        }

        private void ValidarCpf(string cpf, List<DetalhesDaMensagem> erros)
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
            };
        }

        private void ValidaDadosVazios(Cobranca cobranca, List<DetalhesDaMensagem> erros)
        {
            if (cobranca.DataVencimento == default)
            {
                erros.Add(new DetalhesDaMensagem
                {
                    Campo = "data-vencimento",
                    Valor = cobranca.DataVencimento.ToString(),
                    Mensagem = "A data de vencimento é invalida."

                });
            }
            else
            {
                if (cobranca.DataVencimento < DateTime.Now.AddDays(-1))
                {
                    erros.Add(new DetalhesDaMensagem
                    {
                        Campo = "data-vencimento",
                        Valor = cobranca.DataVencimento.ToString(),
                        Mensagem = "A data de vencimento não pode ser antes do dia atual."

                    });
                }
            }

            if (cobranca.ValorCobranca <= decimal.Zero)
            {
                erros.Add(new DetalhesDaMensagem
                {
                    Campo = "valor-cobranca",
                    Valor = cobranca.ValorCobranca.ToString(),
                    Mensagem = "O valor da cobrança deve ser maior que zero."

                });
            }
            else
            {
                if (cobranca.ValorCobranca >= decimal.MaxValue)
                {
                    erros.Add(new DetalhesDaMensagem
                    {
                        Campo = "valor-cobranca",
                        Valor = cobranca.ValorCobranca.ToString(),
                        Mensagem = $"O valor da cobrança deve ser menor que {decimal.MaxValue}."

                    });
                }
            }
        }
    }
}
