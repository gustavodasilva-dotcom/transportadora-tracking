using System;
using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Entities;
using Transportadora.Tracking.Entities.Models.InputModel;
using Transportadora.Tracking.Repositories.Interfaces;
using Transportadora.Tracking.Services.CustomExceptions;
using Transportadora.Tracking.Services.Interfaces;

namespace Transportadora.Tracking.Services
{
    public class OcorrenciaService : IOcorrenciaService
    {
        private readonly IOcorrenciaRepository _ocorrenciaRepository;

        private readonly IPedidoService _pedidoService;

        public OcorrenciaService(IOcorrenciaRepository ocorrenciaRepository, IPedidoService pedidoService)
        {
            _ocorrenciaRepository = ocorrenciaRepository;

            _pedidoService = pedidoService;
        }

        public async Task<int> Inserir(OcorrenciaInputModel ocorrencia)
        {
            try
            {
                if (!await _pedidoService.PedidoExiste(ocorrencia.CodigoPedido))
                    throw new NotFoundException($"O código {ocorrencia.CodigoPedido} não foi encontrado para nenhum pedido.");

                if (!await _ocorrenciaRepository.OcorrenciaExiste(ocorrencia.CodigoOcorrencia))
                    throw new NotFoundException($"O código {ocorrencia.CodigoOcorrencia} não corresponde a uma ocorrência válida.");

                var pedidoId = await _pedidoService.RetornaPedidoId(ocorrencia.CodigoPedido);

                if (pedidoId != 0)
                {
                    var ocorrenciaId = await _ocorrenciaRepository.ObterCodigoOcorrencia(ocorrencia.CodigoOcorrencia);

                    if (ocorrenciaId != 0)
                    {
                        var ocorrenciaInput = new Ocorrencia
                        {
                            CodigoPedido = ocorrencia.CodigoPedido,
                            PedidoId = pedidoId,
                            OcorrenciaId = ocorrenciaId,
                            CodigoOcorrencia = ocorrencia.CodigoOcorrencia,
                            DataOcorrencia = ocorrencia.DataOcorrencia
                        };

                        return await _ocorrenciaRepository.Inserir(ocorrenciaInput);
                    }
                    else
                    {
                        throw new NotFoundException($"Não foi encontrado OcorrenciaID para o código de ocorrência {ocorrencia.CodigoOcorrencia}.");
                    }
                }
                else
                {
                    throw new NotFoundException($"Não foi encontrado PedidoID para o código de pedido {ocorrencia.CodigoPedido}.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
