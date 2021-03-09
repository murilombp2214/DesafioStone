using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using Stone.Clientes.API;
using Stone.Clientes.Aplicacacao.AppService;
using Stone.Clientes.Aplicacacao.AppService.Interfaces;
using Stone.Clientes.Aplicacacao.Request.Cliente;
using Stone.Clientes.Aplicacacao.Response.Cliente;
using Stone.Clientes.Infra.CrossCutting.Utils;
using Stone.Clientes.Infra.CrossCutting.Utils.Interfaces;
using Stone.Clientes.Infra.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Stone.Clientes.Tests.Integration.Controllers.V1
{
    public class ClienteControllerTest
    {

        [Fact]
        public async Task Se_CadastrarCliente_Entao_RetornarIdDeCorrelacaoEmFormatoGuid()
        {
            const string cpf = "12345789";
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<IClienteAppService>();
                    mockAppService.Setup(x => x.Cadastrar(null))
                                    .Returns(Task.FromResult(default(IOperation<ClienteResponse>)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/stone/v1/cliente/cpf/{cpf}");
            response.Headers.TryGetValues("x-correlation-id", out var valuesHeadrs);
            string correlationId = valuesHeadrs.First();
            var ehGuid = Guid.TryParse(correlationId, out var correlationIdGuid);
            Assert.NotNull(correlationId);
            Assert.True(ehGuid);

        }

        [Fact]
        public async Task Se_ClienteNaoCadastrado_Entao_Retornar400()
        {
            ClienteRequest request = null;
            var requestBody = new HttpRequestMessage(HttpMethod.Post,"")
            {
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };


            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<IClienteAppService>();
                    mockAppService.Setup(x => x.Cadastrar(request))
                                    .Returns(Task.FromResult(Result.CreateFailure<ClienteResponse>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.PostAsync($"/stone/v1/cliente", requestBody.Content);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Se_ClienteCadastrado_Entao_Retornar200()
        {
            var requestBody = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(JsonSerializer.Serialize(default(ClienteRequest)),
                                            Encoding.UTF8, "application/json")
            };


            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<IClienteAppService>();
                    mockAppService.Setup(x => x.Cadastrar(null))
                                  .Returns(Task.FromResult(Result.CreateSuccess(new ClienteResponse())));

                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.PostAsync($"/stone/v1/cliente", requestBody.Content);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task Se_CpfNaoExistir_Entao_Retornar404()
        {
            const string cpf = "12345789";
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<IClienteAppService>();
                    mockAppService.Setup(x => x.ConsultarPorCpf(cpf))
                                    .Returns(Task.FromResult(Result.CreateFailure<ClienteResponse>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();


            var response = await _httpClient.GetAsync($"/stone/v1/cliente/cpf/{cpf}");
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Se_ConsultarPorCpf_Entao_RetornarIdDeCorrelacaoEmFormatoGuid()
        {
            const string cpf = "12345789";
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<IClienteAppService>();
                    mockAppService.Setup(x => x.ConsultarPorCpf(cpf))
                                    .Returns(Task.FromResult(Result.CreateFailure<ClienteResponse>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/stone/v1/cliente/cpf/{cpf}");
            response.Headers.TryGetValues("x-correlation-id", out var valuesHeadrs);
            string correlationId = valuesHeadrs.First();
            var ehGuid = Guid.TryParse(correlationId, out var correlationIdGuid);
            Assert.NotNull(correlationId);
            Assert.True(ehGuid);

        }


        [Fact]
        public async Task Se_CpfExistir_Entao_Retornar200()
        {
            const string cpf = "12345789";
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<IClienteAppService>();
                    mockAppService.Setup(x => x.ConsultarPorCpf(cpf))
                                    .Returns(Task.FromResult(Result.CreateSuccess(new ClienteResponse())));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/stone/v1/cliente/cpf/{cpf}");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }



        [Fact]
        public async Task Se_ConsultaPaginadaRetornarErro_Entao_Retornar400()
        {
            const int pagina = 1;
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<IClienteAppService>();
                    mockAppService.Setup(x => x.Consultar(pagina))
                                    .Returns(Task.FromResult(Result.CreateFailure<List<ClienteResponse>>("")));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/stone/v1/cliente?pagina={pagina}");
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Se_ConsultaPaginadaRetornarSucesso_Entao_Retornar200()
        {
            const int pagina = 1;
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<IClienteAppService>();
                    mockAppService.Setup(x => x.Consultar(pagina))
                                    .Returns(Task.FromResult(Result.CreateSuccess<List<ClienteResponse>>(null)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/stone/v1/cliente?pagina={pagina}");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }


        [Fact]
        public async Task Se_ConsultaPaginada_Entao_RetornarIdDeCorrelacaoEmFormatoGuid()
        {
            const int pagina = 1;
            FakeStartup.MockService = (IServiceCollection services) => {
                services.AddSingleton(x => {
                    var mockAppService = new Mock<IClienteAppService>();
                    mockAppService.Setup(x => x.Consultar(pagina))
                                    .Returns(Task.FromResult(Result.CreateFailure<List<ClienteResponse>>(null)));
                    return mockAppService.Object;
                });
            };
            var _server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
            var _httpClient = _server.CreateClient();

            var response = await _httpClient.GetAsync($"/stone/v1/cliente?pagina={pagina}");
            response.Headers.TryGetValues("x-correlation-id", out var valuesHeadrs);
            string correlationId = valuesHeadrs.First();
            var ehGuid = Guid.TryParse(correlationId, out var correlationIdGuid);
            Assert.NotNull(correlationId);
            Assert.True(ehGuid);

        }
    }

}
