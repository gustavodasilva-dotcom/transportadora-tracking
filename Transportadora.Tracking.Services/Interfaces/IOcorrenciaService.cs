using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Models.InputModel;

namespace Transportadora.Tracking.Services.Interfaces
{
    public interface IOcorrenciaService
    {
        Task<int> Inserir(OcorrenciaInputModel ocorrencia);
    }
}
