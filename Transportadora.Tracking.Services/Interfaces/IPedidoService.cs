using System.Collections.Generic;
using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Models.InputModel;
using Transportadora.Tracking.Entities.Models.ViewModel;
using Transportadora.Tracking.Entities.Table;

namespace Transportadora.Tracking.Services.Interfaces
{
    public interface IPedidoService
    {
        Task Inserir(PedidoInputModel pedido);

        Task<PedidoViewModel> Obter(string codigoPedido);

        Task<bool> PedidoExiste(string codigoPedido);

        Task<int> RetornaPedidoId(string codigoPedido);

        Task<RemetenteTable> ObterRemetente(string codigoPedido);

        Task<DestinatarioTable> ObterDestinatario(string codigoPedido);

        Task<IEnumerable<ItemsTable>> ObterItems(string codigoPedido);

        Task<PedidoTable> ObterPedido(string codigoPedido);

        Task<EnderecoTable> ObterEndereco(int enderecoId, string codigoPedido);
    }
}
