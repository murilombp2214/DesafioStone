using System.Text.Json.Serialization;

namespace Stone.Cobrancas.Infra.CrossCutting.Utils
{
    public sealed class DetalhesDaMensagem
    {
        [JsonPropertyName("campo")]
        public string Campo { get; set; }
        [JsonPropertyName("mensagem")]
        public string Mensagem { get; set; }
        [JsonPropertyName("valor")]
        public string Valor { get; set; }
    }
}
