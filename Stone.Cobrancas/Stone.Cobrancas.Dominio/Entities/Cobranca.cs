using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Stone.Cobrancas.Dominio.Entities
{
    public class Cobranca : BaseEntity
    {
        public Cobranca(DateTime dataVencimento, string cpf, decimal valorCobranca)
        {
            DataVencimento = dataVencimento;
            Cpf = cpf;
            ValorCobranca = valorCobranca;
        }
        public DateTime DataVencimento { get; private set; }
        public string Cpf { get; private set; }
        public decimal ValorCobranca { get; private set; }
    }
}
