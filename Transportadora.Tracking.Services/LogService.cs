using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Transportadora.Tracking.Repositories.Interfaces;
using Transportadora.Tracking.Services.Interfaces;

namespace Transportadora.Tracking.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task GravarLog(object model, string log, string codigoPedido)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);

                if (!json.Equals(string.Empty))
                    await _logRepository.GravarLog(json, log, codigoPedido);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
