using System;
using System.Text.Json.Serialization;

namespace Stone.Clientes.Aplicacacao.Response.Cliente
{
    public class ClienteResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
        [JsonPropertyName("estado")]
        public string Estado { get; set; }
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }
    }
}
