using System.Collections.Generic;
using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Entities;
using Transportadora.Tracking.Entities.Table;

namespace Transportadora.Tracking.Repositories.Interfaces
{
    public interface IOcorrenciaRepository
    {
        Task<bool> OcorrenciaExiste(int codigoOcorrencia);

        Task<int> ObterCodigoOcorrencia(int codigoOcorrencia);

        Task<int> Inserir(Ocorrencia ocorrencia);

        Task<IEnumerable<OcorrenciaTable>> ObterOcorrencias(string codigoPedido);
    }
}
