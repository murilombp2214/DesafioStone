using System.Text.Json.Serialization;

namespace Stone.Clientes.Aplicacacao.Request.Cliente
{
    public class ClienteRequest
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
        [JsonPropertyName("estado")]
        public string Estado { get; set; }
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }
    }
}
