using System.Threading.Tasks;

namespace Transportadora.Tracking.Services.Interfaces
{
    public interface ILogService
    {
        Task GravarLog(object model, string log, string codigoPedido);
    }
}
