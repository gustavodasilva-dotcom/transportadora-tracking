using System.Collections.Generic;
using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Entities;

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
    }
}
