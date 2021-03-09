using Microsoft.Extensions.Logging;
using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Dominio.Repository.Interfaces;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Mapper;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca
{
    public class CobrancaStoneWriterClient : ICobrancaStoneWriterRepository
    {
        private const string URL_SALVAR_COBRANCA = "";

        private readonly HttpClient _httpClient;
        private readonly ILogger<ICobrancaStoneWriterRepository> _logger;
        public CobrancaStoneWriterClient(HttpClient httpClient, ILogger<ICobrancaStoneWriterRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<CobrancaStone> RegistrarCobranca(CobrancaStone cobrancaStone)
        {
            var requestBody = CobrancaStoneRequestMapper.ConverterCobrancaEmCobrancaStoneRequest(cobrancaStone);
            var request = new HttpRequestMessage(HttpMethod.Post, URL_SALVAR_COBRANCA)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await _httpClient.SendAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError("Houve um erro ao registrar a cobrança", ex);
                return null;
            }

            if(httpResponse.IsSuccessStatusCode)
            {
                var streamJson = await httpResponse.Content.ReadAsStreamAsync();
                var cobrancaResponse = await JsonSerializer.DeserializeAsync<CobrancaStoneResponse>(streamJson);
                return CobrancaStoneResponseMapper.ConverterCobrancaStoneResponseEmCobranca(cobrancaResponse);
            }

            if(httpResponse.StatusCode == HttpStatusCode.BadRequest 
                                       || httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogError($"[erro : 'houve um erro ao salvar a cobrança' , json : {json}]");
                return null;
            }

            _logger.LogError("Houve um erro ao estabelecer a comunicação com a API.");
            return null;


        }
    }
}
