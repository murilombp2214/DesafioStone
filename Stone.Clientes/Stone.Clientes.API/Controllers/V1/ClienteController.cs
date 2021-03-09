using Microsoft.AspNetCore.Mvc;
using Stone.Clientes.Aplicacacao.AppService.Interfaces;
using Stone.Clientes.Aplicacacao.Request.Cliente;
using Stone.Clientes.Aplicacacao.Response.Cliente;
using Stone.Clientes.Infra.CrossCutting.Utils;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Stone.Clientes.API.Controllers.V1
{
    [Route("stone/v1/cliente")]
    [Produces("application/json")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteAppService _clienteAppService;

        public ClienteController(IClienteAppService clienteAppService)
        {
            _clienteAppService = clienteAppService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OperationSuccess<ClienteResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(OperationFail<ClienteResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Cadastrar([FromBody]ClienteRequest request)
        {
            var cliente =  await _clienteAppService.Cadastrar(request);
            if (cliente is OperationFail<ClienteResponse>)
                return BadRequest(cliente);

            return Ok(cliente);
        }


        [HttpGet("cpf/{cpf}")]
        [ProducesResponseType(typeof(OperationSuccess<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ConsultarPorCpf(string cpf)
        {
            var cliente = await _clienteAppService.ConsultarPorCpf(cpf);
            if (cliente is OperationFail<ClienteResponse>)
                return NotFound();

            return Ok(cliente);
        }

        [HttpGet]
        [ProducesResponseType(typeof(OperationSuccess<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Consultar(int pagina)
        {
            var cliente = await _clienteAppService.Consultar(pagina);
            if (cliente is OperationFail<List<ClienteResponse>>)
                return BadRequest(cliente);

            return Ok(cliente);
        }
    }
}
