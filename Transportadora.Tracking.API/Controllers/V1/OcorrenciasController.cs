using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Models.InputModel;
using Transportadora.Tracking.Services.CustomExceptions;
using Transportadora.Tracking.Services.Interfaces;

namespace Transportadora.Tracking.API.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class OcorrenciasController : ControllerBase
    {
        private readonly IOcorrenciaService _ocorrenciaService;

        private readonly ILogService _logService;

        public OcorrenciasController(IOcorrenciaService ocorrenciaService, ILogService logService)
        {
            _ocorrenciaService = ocorrenciaService;

            _logService = logService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Inserir([FromBody] OcorrenciaInputModel ocorrencia)
        {
            try
            {
                await _logService.GravarLog(ocorrencia, "Ocorrência recebida.", ocorrencia.CodigoPedido);

                var ocorrenciaPedidoId = await _ocorrenciaService.Inserir(ocorrencia);

                await _logService.GravarLog(ocorrencia, $"Ocorrência {ocorrencia.CodigoOcorrencia} inserida com sucesso: {ocorrenciaPedidoId}.", ocorrencia.CodigoPedido);

                return StatusCode(201, $"Ocorrência {ocorrencia.CodigoOcorrencia} insira com sucesso: {ocorrenciaPedidoId}.");
            }
            catch (NotFoundException e)
            {
                await _logService.GravarLog(ocorrencia, e.Message, ocorrencia.CodigoPedido);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                await _logService.GravarLog(ocorrencia, $"Ocorreu o seguinte erro ao realizar esse processo: {e.Message}.", ocorrencia.CodigoPedido);

                return StatusCode(500, $"Ocorreu o seguinte erro ao realizar esse processo: {e.Message}.");
            }
        }
    }
}
