using System;
using System.Text.Json.Serialization;

namespace Stone.Cobrancas.Aplicacacao.Response
{
    public class CobrancaResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("data-vencimento")]
        public DateTime DataVencimento { get; set; }
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }
        [JsonPropertyName("valor-cobranca")]
        public decimal ValorCobranca { get; set; }
    }
}
