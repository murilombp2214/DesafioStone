using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;
using System.Text.Json.Serialization;

namespace Stone.Cobrancas.Infra.CrossCutting.Utils
{
    public sealed class OperationSuccess<T> : IOperation<T>
    {
        public OperationSuccess(T data)
        {
            Data = data;
        }
        [JsonPropertyName("data")]
        public T Data { get; }
    }
}
