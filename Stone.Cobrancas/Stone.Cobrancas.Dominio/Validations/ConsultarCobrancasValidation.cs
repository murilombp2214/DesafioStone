using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Dominio.Validations.Interfaces;
using Stone.Cobrancas.Infra.CrossCutting.Utils;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stone.Cobrancas.Dominio.Validations
{
    public class ConsultarCobrancasValidation : IConsultarCobrancasValidation
    {
        private readonly ICpfValidation _cpfValidation;
        public ConsultarCobrancasValidation(ICpfValidation cpfValidation)
        {
            _cpfValidation = cpfValidation;
        }

        public IOperation<List<Cobranca>> Validar(string cpf, int pagina)
        {
            var erros = new List<DetalhesDaMensagem>();
            ValidarCpf(cpf, erros);
            ValidarPagina(pagina,erros);

            if (erros.Any())
                return Result.CreateFailure<List<Cobranca>>("Houve um erro ao validar os dados da consulta.", erros);

            return Result.CreateSuccess<List<Cobranca>>();
        }


        public IOperation<List<Cobranca>> Validar(int mes, int pagina)
        {
            var erros = new List<DetalhesDaMensagem>();
            ValidarMes(mes, erros);
            ValidarPagina(pagina, erros);

            if (erros.Any())
                return Result.CreateFailure<List<Cobranca>>("Houve um erro ao validar os dados da consulta.", erros);

            return Result.CreateSuccess<List<Cobranca>>();
        }

        private void ValidarMes(int mes, List<DetalhesDaMensagem> erros)
        {
            if(mes < 1 || mes > 12)
            {
                erros.Add(new DetalhesDaMensagem
                {
                    Campo = "mes",
                    Valor = mes.ToString(),
                    Mensagem = "O mês informado é invalido."
                });
            }
        }

        private void ValidarPagina(in int pagina, List<DetalhesDaMensagem> erros)
        {
            if(pagina < 1)
            {
                erros.Add(new DetalhesDaMensagem
                {
                    Campo = nameof(pagina),
                    Valor = pagina.ToString(),
                    Mensagem = "A pagina deve ser maior ou igual a 1."
                });
            }
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


    }
}
