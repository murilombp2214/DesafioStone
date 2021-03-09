using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.ProcessamentoCobranca.Dominio.Entities
{
    public class CobrancaStone 
    {

        public CobrancaStone(DateTime dataVencimento, string cpf, decimal valorCobranca)
        {
            DataVencimento = dataVencimento;
            Cpf = cpf;
            ValorCobranca = valorCobranca;
        }

        public CobrancaStone(Guid id, DateTime dataVencimento, string cpf, decimal valorCobranca) 
            :this(dataVencimento,cpf,valorCobranca)
        {
            Id = id;
        }
        public Guid Id { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public string Cpf { get; private set; }
        public decimal ValorCobranca { get; private set; }
    }
}
