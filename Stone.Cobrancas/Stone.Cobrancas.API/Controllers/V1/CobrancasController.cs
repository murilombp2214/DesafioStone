using Microsoft.AspNetCore.Mvc;
using Stone.Cobrancas.Aplicacacao.AppService.Interfaces;
using Stone.Cobrancas.Aplicacacao.Request;
using Stone.Cobrancas.Aplicacacao.Response;
using Stone.Cobrancas.Infra.CrossCutting.Utils;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Stone.Cobrancas.API.Controllers.V1
{
    [Route("stone/v1/cobranca")]
    [Produces("application/json")]
    public class CobrancasController : ControllerBase
    {
        private readonly ICobrancaAppService _cobrancaAppService;
        public CobrancasController(ICobrancaAppService cobrancaAppService)
        {
            _cobrancaAppService = cobrancaAppService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OperationSuccess<CobrancaResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(OperationFail<CobrancaResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RegistrarCobranca([FromBody] CobrancaRequest request)
        {
            var cobranca = await _cobrancaAppService.Cadastrar(request);
            if (cobranca is OperationFail<CobrancaResponse>)
                return BadRequest(cobranca);

            return Ok(cobranca);
        }

        [HttpGet("cpf/{cpf}/pagina/{pagina}")]
        public async Task<IActionResult> ConsultarCobranca(string cpf, int pagina)
        {
            var cobrancas = await _cobrancaAppService.ConsultarCobrancasPorCpf(cpf, pagina);

            if(cobrancas is OperationFail<IEnumerable<CobrancaResponse>>)
                return BadRequest(cobrancas);

            return Ok(cobrancas);
        }

        [HttpGet("mes/{mes}/pagina/{pagina}")]
        public async Task<IActionResult> ConsultarCobranca(int mes, int pagina)
        {
            var cobrancas = await _cobrancaAppService.ConsultarCobrancasPorMes(mes, pagina);

            if (cobrancas is OperationFail<IEnumerable<CobrancaResponse>>)
                return BadRequest(cobrancas);

            return Ok(cobrancas);
        }
    }
}
