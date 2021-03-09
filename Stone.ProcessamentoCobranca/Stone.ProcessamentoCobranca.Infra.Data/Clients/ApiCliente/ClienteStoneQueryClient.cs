using Microsoft.Extensions.Logging;
using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Dominio.Repository.Interfaces;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCliente.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCliente
{
    public class ClienteStoneQueryClient : IClienteStoneQueryRepository
    {
        private const string QUERY_CONSULTAR_CLIENTES = "?pagina=";
        private readonly HttpClient _httpClient;
        private readonly ILogger<IClienteStoneQueryRepository> _logger;
        public ClienteStoneQueryClient(HttpClient httpClient, ILogger<IClienteStoneQueryRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<List<ClienteStone>> ConsultarClientes(int pagina)
        {
            try
            {
                string query = $"{QUERY_CONSULTAR_CLIENTES}{pagina}";
                var result = await _httpClient.GetAsync(query);
                if (result.IsSuccessStatusCode)
                {
                    var streamJson = await result.Content.ReadAsStreamAsync();
                    var clientes = await JsonSerializer.DeserializeAsync<ClienteStoneResponse>(streamJson);
                    if (clientes.Data is object)
                    {
                        return clientes.Data
                              .Select(x => new ClienteStone(x.Id, x.Nome, x.Estado, x.Cpf))
                               .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Houve um erro ao consultar a API de Clientes", ex);
            }
            return null;
        }
    }
}
