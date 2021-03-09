using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stone.Cobrancas.Aplicacacao.AppService.Interfaces;
using Stone.Cobrancas.Aplicacacao.Request;
using Stone.Cobrancas.Aplicacacao.Response;
using Stone.Cobrancas.Infra.CrossCutting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Stone.Cobrancas.Tests.Integration.Controllers.V1
{
    public class CobrancasControllerTest
    {
        [Fact]
        public async Task Se_CobrancaNaoCadastrado_Entao_Retornar400()
        {
            CobrancaRequest request = null;
            var requestBody = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };


            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICobrancaAppService>();
                    mockAppService.Setup(x => x.Cadastrar(request))
                                    .Returns(Task.FromResult(Result.CreateFailure<CobrancaResponse>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.PostAsync($"/stone/v1/cobranca", requestBody.Content);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Se_CobrancaCadastrado_Entao_Retornar200()
        {
            CobrancaRequest request = null;
            var requestBody = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };


            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICobrancaAppService>();
                    mockAppService.Setup(x => x.Cadastrar(request))
                                    .Returns(Task.FromResult(Result.CreateSuccess<CobrancaResponse>(null)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.PostAsync($"/stone/v1/cobranca", requestBody.Content);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task Se_CadastroDeCobranca_Entao_RetornarIdDeCorrelacaoEmFormatoGuid()
        {
            CobrancaRequest request = null;

            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICobrancaAppService>();
                    mockAppService.Setup(x => x.Cadastrar(request))
                                    .Returns(Task.FromResult(Result.CreateSuccess<CobrancaResponse>(null)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/stone/v1/cobranca");
            response.Headers.TryGetValues("x-correlation-id", out var valuesHeadrs);
            string correlationId = valuesHeadrs.First();
            var ehGuid = Guid.TryParse(correlationId, out var correlationIdGuid);
            Assert.NotNull(correlationId);
            Assert.True(ehGuid);
        }

        [Fact]
        public async Task Se_ConsultaPorCpfFalhar_Entao_Retornar400()
        {
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICobrancaAppService>();
                    mockAppService.Setup(x => x.ConsultarCobrancasPorCpf("123",1))
                                   .Returns(Task.FromResult(Result.CreateFailure<IEnumerable<CobrancaResponse>>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync("stone/v1/cobranca/cpf/123/pagina/1");
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Se_ConsultaPorCpfOk_Entao_Retornar200()
        {
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICobrancaAppService>();
                    mockAppService.Setup(x => x.ConsultarCobrancasPorCpf("123", 1))
                                  .Returns(Task.FromResult(Result.CreateSuccess<IEnumerable<CobrancaResponse>>(null)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync("stone/v1/cobranca/cpf/123/pagina/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }


        [Fact]
        public async Task Se_ConsultaPorCpf_Entao_RetornarIdDeCorrelacao()
        {
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICobrancaAppService>();
                    mockAppService.Setup(x => x.ConsultarCobrancasPorCpf("123", 1))
                                  .Returns(Task.FromResult(Result.CreateSuccess<IEnumerable<CobrancaResponse>>(null)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync("cpf/123/pagina/1");
            response.Headers.TryGetValues("x-correlation-id", out var valuesHeadrs);
            string correlationId = valuesHeadrs.First();
            var ehGuid = Guid.TryParse(correlationId, out var correlationIdGuid);
            Assert.NotNull(correlationId);
            Assert.True(ehGuid);
        }


        [Fact]
        public async Task Se_ConsultaPorMesFalhar_Entao_Retornar400()
        {
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICobrancaAppService>();
                    mockAppService.Setup(x => x.ConsultarCobrancasPorMes(1, 1))
                                   .Returns(Task.FromResult(Result.CreateFailure<IEnumerable<CobrancaResponse>>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync("stone/v1/cobranca/mes/1/pagina/1");
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Se_ConsultaPorMesOk_Entao_Retornar200()
        {
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICobrancaAppService>();
                    mockAppService.Setup(x => x.ConsultarCobrancasPorMes(1, 1))
                                  .Returns(Task.FromResult(Result.CreateSuccess<IEnumerable<CobrancaResponse>>(null)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync("stone/v1/cobranca/mes/1/pagina/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }


        [Fact]
        public async Task Se_ConsultaPorMes_Entao_RetornarIdDeCorrelacao()
        {
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<ICobrancaAppService>();
                    mockAppService.Setup(x => x.ConsultarCobrancasPorMes(1, 1))
                                  .Returns(Task.FromResult(Result.CreateSuccess<IEnumerable<CobrancaResponse>>(null)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync("mes/1/pagina/1");
            response.Headers.TryGetValues("x-correlation-id", out var valuesHeadrs);
            string correlationId = valuesHeadrs.First();
            var ehGuid = Guid.TryParse(correlationId, out var correlationIdGuid);
            Assert.NotNull(correlationId);
            Assert.True(ehGuid);
        }
    }
}
