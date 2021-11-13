using System.Threading.Tasks;

namespace Transportadora.Tracking.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task GravarLog(string jsonEntrada, string mensagem, string codigoPedido);
    }
}
