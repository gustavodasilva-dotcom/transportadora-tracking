using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Transportadora.Tracking.Services.CustomExceptions;
using Transportadora.Tracking.Entities.Models.InputModel;
using Transportadora.Tracking.Services.Interfaces;
using Transportadora.Tracking.Entities.Models.ViewModel;

namespace Transportadora.Tracking.API.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        private readonly ILogService _logService;

        public PedidosController(IPedidoService pedidoService, ILogService logService)
        {
            _pedidoService = pedidoService;

            _logService = logService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Inserir([FromBody] PedidoInputModel pedido)
        {
            try
            {
                await _logService.GravarLog(pedido, "Pedido recebido.", pedido.CodigoPedido);

                await _pedidoService.Inserir(pedido);

                await _logService.GravarLog(pedido, "Pedido cadastrado com sucesso!", pedido.CodigoPedido);

                return StatusCode(201, $@"Pedido {pedido.CodigoPedido} cadastrado com sucesso!");
            }
            catch (ConflictException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                await _logService.GravarLog(pedido, $"Ocorreu o seguinte erro ao realizar esse processo: {e.Message}.", pedido.CodigoPedido);

                return StatusCode(500, $"Ocorreu o seguinte erro ao realizar esse processo: {e.Message}.");
            }
        }

        [HttpGet("{codigoPedido}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PedidoViewModel>> Obter([FromRoute] string codigoPedido)
        {
            try
            {
                return Ok(await _pedidoService.Obter(codigoPedido));
            }
            catch (NotFoundException e)
            {
                await _logService.GravarLog(e.Message, $"Ocorreu o seguinte erro ao realizar esse processo: {e.Message}.", codigoPedido);

                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                await _logService.GravarLog(e.Message, $"Ocorreu o seguinte erro ao realizar esse processo: {e.Message}.", codigoPedido);

                return StatusCode(500, $"Ocorreu o seguinte erro ao realizar esse processo: {e.Message}.");
            }
        }
    }
}
