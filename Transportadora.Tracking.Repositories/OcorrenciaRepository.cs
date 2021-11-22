using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Entities;
using Transportadora.Tracking.Entities.Table;
using Transportadora.Tracking.Repositories.Interfaces;

namespace Transportadora.Tracking.Repositories
{
    public class OcorrenciaRepository : IOcorrenciaRepository
    {
        private readonly string _connectionString;

        public OcorrenciaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<bool> OcorrenciaExiste(int codigoOcorrencia)
        {
            #region SQL

            try
            {
                var query = $@"SELECT 1 FROM OCORRENCIA WHERE CODIGOOCORRENCIA = {codigoOcorrencia};";

                using var conn = new SqlConnection(_connectionString);
                return await conn.QueryFirstOrDefaultAsync<bool>(query, new { codigoOcorrencia });
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task<int> ObterCodigoOcorrencia(int codigoOcorrencia)
        {
            #region SQL

            try
            {
                var query = $@"SELECT OCORRENCIAID FROM OCORRENCIA WHERE CODIGOOCORRENCIA = {codigoOcorrencia};";

                using var conn = new SqlConnection(_connectionString);
                return await conn.QueryFirstOrDefaultAsync<int>(query, new { codigoOcorrencia });
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task<int> Inserir(Ocorrencia ocorrencia)
        {
            #region SQL

            try
            {
                var query = $@" DECLARE @OcorrenciaPedidoId INT;

                INSERT INTO OCORRENCIA_PEDIDO VALUES({ocorrencia.PedidoId}, {ocorrencia.OcorrenciaId}, '{ocorrencia.DataOcorrencia}', 1, GETDATE());

                SET @OcorrenciaPedidoId = @@IDENTITY;

                SELECT @OcorrenciaPedidoId;";

                using var conn = new SqlConnection(_connectionString);
                return await conn.ExecuteScalarAsync<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task<IEnumerable<OcorrenciaTable>> ObterOcorrencias(string codigoPedido)
        {
            #region SQL

            try
            {
                var query =
                $@"SELECT		 CODIGOOCORRENCIA
                    			,DESCRICAO
                    			,DATAOCORRENCIA
                    FROM		OCORRENCIA			AS O
                    INNER JOIN	OCORRENCIA_PEDIDO	AS OP ON O.OCORRENCIAID = OP.OCORRENCIAID
                    INNER JOIN	PEDIDO				AS P  ON OP.PEDIDOID	= P.PEDIDOID
                    WHERE		P.CODIGOPEDIDO = '{codigoPedido}';";

                using var conn = new SqlConnection(_connectionString);
                return await conn.QueryAsync<OcorrenciaTable>(query, new { codigoPedido });
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }
    }
}
