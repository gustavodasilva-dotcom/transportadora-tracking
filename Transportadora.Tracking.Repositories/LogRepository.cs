using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Transportadora.Tracking.Repositories.Interfaces;

namespace Transportadora.Tracking.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly string _connectionString;

        public LogRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task GravarLog(string jsonEntrada, string mensagem, string codigoPedido)
        {
            #region SQL

            try
            {
                var query = $@"INSERT INTO LOGS VALUES('{jsonEntrada}', '{mensagem}', '{codigoPedido}', 1, GETDATE());";

                var conn = new SqlConnection(_connectionString);
                await conn.ExecuteAsync(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }
    }
}
