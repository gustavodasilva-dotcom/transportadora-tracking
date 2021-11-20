using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Models.InputModel;
using Transportadora.Tracking.Entities.Models.ViewModel;

namespace Transportadora.Tracking.Services.Interfaces
{
    public interface IPedidoService
    {
        Task Inserir(PedidoInputModel pedido);

        Task<bool> PedidoExiste(string codigoPedido);

        Task<int> RetornaPedidoId(string codigoPedido);

        Task<PedidoViewModel> Obter(string codigoPedido);
    }
}
