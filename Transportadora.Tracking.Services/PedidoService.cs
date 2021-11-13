using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Entities;
using Transportadora.Tracking.Entities.Models.InputModel;
using Transportadora.Tracking.Repositories.Interfaces;
using Transportadora.Tracking.Services.Interfaces;
using Transportadora.Tracking.Services.CustomExceptions;

namespace Transportadora.Tracking.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        private readonly ILogService _logService;

        public PedidoService(IPedidoRepository pedidoRepository, ILogService logService)
        {
            _pedidoRepository = pedidoRepository;

            _logService = logService;
        }

        public async Task Inserir(PedidoInputModel pedido)
        {
            try
            {
                int remetenteId, destinatarioId;

                #region InstanciandoObjetos

                var items = new List<Item>();

                foreach (var item in pedido.Items)
                {
                    items.Add(new Item
                    {
                        Codigo = item.Codigo,
                        Descricao = item.Descricao,
                        Quantidade = item.Quantidade,
                        Preco = item.Preco
                    });
                }

                var pedidoInput = new Pedido
                {
                    CodigoPedido = pedido.CodigoPedido,
                    Destintario = new Destinatario
                    {
                        RazaoSocial = pedido.Destintario.RazaoSocial,
                        Empresa = pedido.Destintario.Empresa,
                        Cnpj = (pedido.Destintario.Empresa) ? pedido.Destintario.Cnpj : string.Empty,
                        Cpf = (pedido.Destintario.Empresa) ? string.Empty : pedido.Destintario.Cpf,
                        Endereco  = new Endereco
                        {
                            Cep = pedido.Destintario.Endereco.Cep,
                            Logradouro = pedido.Destintario.Endereco.Logradouro,
                            Numero = pedido.Destintario.Endereco.Numero,
                            Bairro = pedido.Destintario.Endereco.Bairro,
                            Cidade = pedido.Destintario.Endereco.Cidade,
                            Estado = pedido.Destintario.Endereco.Estado,
                            Pais = pedido.Destintario.Endereco.Pais
                        }
                    },
                    Remetente = new Remetente
                    {
                        RazaoSocial = pedido.Remetente.RazaoSocial,
                        Empresa = pedido.Remetente.Empresa,
                        Cnpj = (pedido.Remetente.Empresa) ? pedido.Remetente.Cnpj : string.Empty,
                        Cpf = (pedido.Remetente.Empresa) ? string.Empty : pedido.Destintario.Cpf,
                        Endereco = new Endereco
                        {
                            Cep = pedido.Remetente.Endereco.Cep,
                            Logradouro = pedido.Remetente.Endereco.Logradouro,
                            Numero = pedido.Remetente.Endereco.Numero,
                            Bairro = pedido.Remetente.Endereco.Bairro,
                            Cidade = pedido.Remetente.Endereco.Cidade,
                            Estado = pedido.Remetente.Endereco.Estado,
                            Pais = pedido.Remetente.Endereco.Pais
                        }
                    },
                    Items = items
                };

                #endregion InstanciandoObjetos

                remetenteId = await _pedidoRepository.RemetenteCadastrado(pedidoInput.Remetente);
                
                if (remetenteId == 0)
                {
                    var remetenteEnderecoId = await _pedidoRepository.CadastrarEndereco(pedidoInput.Remetente.Endereco);

                    await _logService.GravarLog(pedido, "Endereço do remetente cadastrado com sucesso.", pedido.CodigoPedido);

                    if (remetenteEnderecoId != 0)
                    {
                        remetenteId = await _pedidoRepository.CadastrarRemetente(pedidoInput.Remetente, remetenteEnderecoId);

                        await _logService.GravarLog(pedido, "Remetente cadastrado com sucesso.", pedido.CodigoPedido);
                    }
                }

                destinatarioId = await _pedidoRepository.DestinatarioCadastrado(pedidoInput.Destintario);

                if (destinatarioId == 0)
                {
                    var destinatarioEnderecoId = await _pedidoRepository.CadastrarEndereco(pedidoInput.Destintario.Endereco);

                    await _logService.GravarLog(pedido, "Endereço do destinatário cadastrado com sucesso.", pedido.CodigoPedido);

                    if (destinatarioEnderecoId != 0)
                    {
                        destinatarioId = await _pedidoRepository.CadastrarDestinario(pedidoInput.Destintario, destinatarioEnderecoId);

                        await _logService.GravarLog(pedido, "Destinatário cadastrado com sucesso.", pedido.CodigoPedido);
                    }
                }

                if (!await _pedidoRepository.PedidoExiste(pedido.CodigoPedido))
                {
                    var pedidoId = await _pedidoRepository.CadastrarPedido(pedidoInput, remetenteId, destinatarioId);

                    if (pedidoId != 0)
                    {
                        await _logService.GravarLog(pedido, "Cabeçalho do pedido cadastrado com sucesso.", pedido.CodigoPedido);

                        foreach (var item in items)
                        {
                            await _pedidoRepository.CadastrarItem(item, pedidoId);

                            await _logService.GravarLog(pedido, $"Item {item.Codigo} do pedido {pedidoId} cadastrado com sucesso!.", pedido.CodigoPedido);
                        }
                    }
                }
                else
                {
                    await _logService.GravarLog(pedido, $"O pedido {pedido.CodigoPedido} já existe cadastrado! Procedimento inválido.", pedido.CodigoPedido);

                    throw new ConflictException($"O pedido {pedido.CodigoPedido} já existe cadastrado! Procedimento inválido.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
