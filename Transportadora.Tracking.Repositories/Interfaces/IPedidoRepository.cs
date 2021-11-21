using System.Collections.Generic;
using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Entities;
using Transportadora.Tracking.Entities.Table;

namespace Transportadora.Tracking.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<int> RemetenteCadastrado(Remetente remetente);

        Task<int> DestinatarioCadastrado(Destinatario destinatario);

        Task<int> CadastrarEndereco(Endereco endereco);

        Task<int> CadastrarRemetente(Remetente remetente, int enderecoRemetenteId);

        Task<int> CadastrarDestinario(Destinatario destinario, int enderecoDestinarioId);

        Task<bool> PedidoExiste(string codigoPedido);

        Task<int> CadastrarPedido(Pedido pedido, int remetenteId, int destinatarioId);

        Task CadastrarItem(Item item, int pedidoId);

        Task<int> RetornaPedidoId(string codigoPedido);

        Task<PedidoTable> ObterPedido(string codigoPedido);

        Task<RemetenteTable> ObterRemetente(string identificacao);

        Task<DestinatarioTable> ObterDestinatario(string codigoPedido);

        Task<IEnumerable<ItemsTable>> ObterItems(string codigoPedido);

        Task<EnderecoTable> ObterEndereco(int idEndereco);
    }
}
