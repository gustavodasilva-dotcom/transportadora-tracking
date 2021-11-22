using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Models.InputModel;
using Transportadora.Tracking.Entities.Models.ViewModel;

namespace Transportadora.Tracking.Services.Interfaces
{
    public interface IOcorrenciaService
    {
        Task<int> Inserir(OcorrenciaInputModel ocorrencia);

        Task<InfoOcorrenciaViewModel> Obter(string codigoPedido);
    }
}
