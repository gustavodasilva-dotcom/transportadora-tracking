using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Entities;
using Transportadora.Tracking.Entities.Models.InputModel;
using Transportadora.Tracking.Repositories.Interfaces;
using Transportadora.Tracking.Services.Interfaces;
using Transportadora.Tracking.Services.CustomExceptions;
using Transportadora.Tracking.Entities.Models.ViewModel;
using System.Linq;
using Transportadora.Tracking.Entities.Table;

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
                        Endereco = new Endereco
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

        public async Task<PedidoViewModel> Obter(string codigoPedido)
        {
            var itemsRetorno = new List<ItemViewModel>();

            try
            {
                if (!await _pedidoRepository.PedidoExiste(codigoPedido))
                    throw new NotFoundException($"Pedido {codigoPedido} não encontrado.");

                var rementente = await ObterRemetente(codigoPedido);

                var destinatario = await ObterDestinatario(codigoPedido);

                var items = await ObterItems(codigoPedido);

                foreach (var item in items)
                {
                    itemsRetorno.Add(new ItemViewModel
                    {
                        Codigo = item.Codigo,
                        Descricao = item.Descricao,
                        Quantidade = item.Quantidade,
                        Preco = Convert.ToString(item.Preco)
                    });
                }

                var pedido = await ObterPedido(codigoPedido);

                var enderecoRemetente = await ObterEndereco(rementente.EnderecoID, codigoPedido);

                var enderecoDestinatario = await ObterEndereco(destinatario.EnderecoID, codigoPedido);

                return new PedidoViewModel
                {
                    PedidoID = pedido.PedidoID,
                    CodigoPedido = pedido.CodigoPedido,
                    DataIntegracao = pedido.DataCadastro.ToString("yyyy-MM-dd HH-mm-ss"),
                    Remetente = new RemetenteViewModel
                    {
                        RazaoSocial = rementente.RazaoSocial,
                        Empresa = rementente.Empresa == 1 ? true : false,
                        Cnpj = rementente.Cnpj,
                        Cpf = rementente.Cpf,
                        Endereco = new EnderecoViewModel
                        {
                            Cep = enderecoRemetente.Cep,
                            Logradouro = enderecoRemetente.Logradouro,
                            Numero = enderecoRemetente.Numero,
                            Bairro = enderecoRemetente.Bairro,
                            Cidade = enderecoRemetente.Cidade,
                            Estado = enderecoRemetente.Estado,
                            Pais = enderecoRemetente.Pais
                        }
                    },
                    Destintario = new DestinatarioViewModel
                    {
                        RazaoSocial = destinatario.RazaoSocial,
                        Empresa = destinatario.Empresa == 1 ? true : false,
                        Cnpj = destinatario.Cnpj,
                        Cpf = destinatario.Cpf,
                        Endereco = new EnderecoViewModel
                        {
                            Cep = enderecoDestinatario.Cep,
                            Logradouro = enderecoDestinatario.Logradouro,
                            Numero = enderecoDestinatario.Numero,
                            Bairro = enderecoDestinatario.Bairro,
                            Cidade = enderecoDestinatario.Cidade,
                            Estado = enderecoDestinatario.Estado,
                            Pais = enderecoDestinatario.Pais
                        }
                    },
                    Items = itemsRetorno
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> PedidoExiste(string codigoPedido)
        {
            try
            {
                return await _pedidoRepository.PedidoExiste(codigoPedido);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> RetornaPedidoId(string codigoPedido)
        {
            try
            {
                return await _pedidoRepository.RetornaPedidoId(codigoPedido);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RemetenteTable> ObterRemetente(string codigoPedido)
        {
            try
            {
                var rementente = await _pedidoRepository.ObterRemetente(codigoPedido);

                if (rementente == null)
                    throw new NotFoundException($"Não foi encontrado remetente cadastrado para o pedido {codigoPedido}.");

                return rementente;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DestinatarioTable> ObterDestinatario(string codigoPedido)
        {
            try
            {
                var destinatario = await _pedidoRepository.ObterDestinatario(codigoPedido);

                if (destinatario == null)
                    throw new NotFoundException($"Não foi encontrado destinatário cadastrado para o pedido {codigoPedido}.");

                return destinatario;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ItemsTable>> ObterItems(string codigoPedido)
        {
            try
            {
                var items = await _pedidoRepository.ObterItems(codigoPedido);

                if (!items.Any())
                    throw new NotFoundException($"Pedido {codigoPedido} não possui items.");

                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PedidoTable> ObterPedido(string codigoPedido)
        {
            try
            {
                var pedido = await _pedidoRepository.ObterPedido(codigoPedido);

                if (pedido == null)
                    throw new NotFoundException($"Cabeçalho do pedido {codigoPedido} inexistente.");

                return pedido;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EnderecoTable> ObterEndereco(int enderecoId, string codigoPedido)
        {
            try
            {
                var enderecoRemetente = await _pedidoRepository.ObterEndereco(enderecoId);

                if (enderecoRemetente == null)
                    throw new NotFoundException($"Não foi encontrado endereco de remetente para o pedido {codigoPedido}.");

                return enderecoRemetente;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
