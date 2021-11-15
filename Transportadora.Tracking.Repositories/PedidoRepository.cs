using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Transportadora.Tracking.Entities.Entities;
using Transportadora.Tracking.Repositories.Interfaces;

namespace Transportadora.Tracking.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _connectionString;

        public PedidoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<int> RemetenteCadastrado(Remetente remetente)
        {
            #region SQL

            try
            {
                string query;

                if (remetente.Cpf.Equals(string.Empty))
                {
                    query =
                    $@"DECLARE @RemetenteID INT;

                    SELECT @RemetenteID = REMETENTEID FROM REMETENTE WHERE CNPJ = '{remetente.Cnpj}';
                    
                    SELECT CASE WHEN @RemetenteID IS NOT NULL THEN @RemetenteID ELSE 0 END AS REMETENTEID;";
                }
                else
                {
                    query =
                    $@"DECLARE @RemetenteID INT;

                    SELECT @RemetenteID = REMETENTEID FROM REMETENTE WHERE CPF = '{remetente.Cpf}';
                    
                    SELECT CASE WHEN @RemetenteID IS NOT NULL THEN @RemetenteID ELSE 0 END AS REMETENTEID;";
                }

                var conn = new SqlConnection(_connectionString);
                return await conn.QueryFirstOrDefaultAsync<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task<int> DestinatarioCadastrado(Destinatario destinatario)
        {
            #region SQL

            try
            {
                string query;

                if (destinatario.Cpf.Equals(string.Empty))
                {
                    query =
                    $@"DECLARE @DestinatarioID INT;

                    SELECT @DestinatarioID = DESTINATARIOID FROM DESTINATARIO WHERE CNPJ = '{destinatario.Cnpj}';
                    
                    SELECT CASE WHEN @DestinatarioID IS NOT NULL THEN @DestinatarioID ELSE 0 END AS DESTINATARIOID;";
                }
                else
                {
                    query =
                    $@"DECLARE @DestinatarioID INT;

                    SELECT @DestinatarioID = DESTINATARIOID FROM DESTINATARIO WHERE CPF = '{destinatario.Cpf}';
                    
                    SELECT CASE WHEN @DestinatarioID IS NOT NULL THEN @DestinatarioID ELSE 0 END AS DESTINATARIOID;";
                }
                
                var conn = new SqlConnection(_connectionString);
                return await conn.QueryFirstOrDefaultAsync<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task<int> CadastrarEndereco(Endereco endereco)
        {
            #region SQL

            try
            {
                var query =
                $@" DECLARE @Id INT;

                    INSERT INTO ENDERECO
                    VALUES
                    (
                    	 '{endereco.Cep}'
                    	,'{endereco.Logradouro}'
                    	,'{endereco.Numero}'
                    	,'{endereco.Bairro}'
                    	,'{endereco.Cidade}'
                    	,'{endereco.Estado}'
                    	,'{endereco.Pais}'
                    	,1
                    	,GETDATE()
                    );

                    SET @Id = @@IDENTITY

                    SELECT @Id;";

                using var conn = new SqlConnection(_connectionString);
                return await conn.ExecuteScalarAsync<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task<int> CadastrarRemetente(Remetente remetente, int enderecoRemetenteId)
        {
            #region SQL

            try
            {
                var empresa = remetente.Empresa ? 1 : 0;

                var query =
                $@" DECLARE @Id INT;

                    INSERT INTO REMETENTE
                    VALUES
                    (
                    	 '{remetente.RazaoSocial}'
                    	,{empresa}
                    	,'{remetente.Cnpj}'
                    	,'{remetente.Cpf}'
                    	,{enderecoRemetenteId}
                    	,1
                    	,GETDATE()
                    );
                    
                    SET @Id = @@IDENTITY;
                    
                    SELECT @Id;";

                using var conn = new SqlConnection(_connectionString);
                return await conn.ExecuteScalarAsync<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task<int> CadastrarDestinario(Destinatario destinario, int enderecoDestinarioId)
        {
            #region SQL

            try
            {
                var empresa = destinario.Empresa ? 1 : 0;

                var query =
                $@" DECLARE @Id INT;

                    INSERT INTO DESTINATARIO
                    VALUES
                    (
                    	 '{destinario.RazaoSocial}'
                    	,{empresa}
                    	,'{destinario.Cnpj}'
                    	,'{destinario.Cpf}'
                    	,{enderecoDestinarioId}
                    	,1
                    	,GETDATE()
                    );
                    
                    SET @Id = @@IDENTITY;
                    
                    SELECT @Id;";

                using var conn = new SqlConnection(_connectionString);
                return await conn.ExecuteScalarAsync<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task<int> CadastrarPedido(Pedido pedido, int remetenteId, int destinatarioId)
        {
            #region SQL

            try
            {
                var query =
                $@" DECLARE @PedidoID INT;

                    INSERT INTO PEDIDO
                    VALUES
                    (
                         '{pedido.CodigoPedido}'
                    	,{remetenteId}
                    	,{destinatarioId}
                    	,1
                    	,GETDATE()
                    );
                    
                    SET @PedidoID = @@IDENTITY;
                    
                    SELECT @PedidoID;";

                using var conn = new SqlConnection(_connectionString);
                return await conn.ExecuteScalarAsync<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task CadastrarItem(Item item, int pedidoId)
        {
            #region SQL

            try
            {
                int itemId;

                var query =
                $@" DECLARE @ItemID INT;
                
                INSERT INTO ITEMS VALUES ('{item.Codigo}', '{item.Descricao}', {item.Quantidade}, '{item.Preco}', 1, GETDATE());

                SET @ItemID = @@IDENTITY;

                SELECT @ItemID;";

                using var conn = new SqlConnection(_connectionString);
                itemId = await conn.ExecuteScalarAsync<int>(query);

                query = $@"INSERT INTO PEDIDO_ITEMS VALUES ({pedidoId}, {itemId}, 1, GETDATE());";

                await conn.ExecuteAsync(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task<bool> PedidoExiste(string codigoPedido)
        {
            #region SQL

            try
            {
                var query = $@"SELECT 1 FROM PEDIDO WHERE CODIGOPEDIDO = '{codigoPedido}';";

                using var conn = new SqlConnection(_connectionString);
                return await conn.QueryFirstOrDefaultAsync<bool>(query, new { codigoPedido });
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public async Task<int> RetornaPedidoId(string codigoPedido)
        {
            #region SQL

            try
            {
                var query = $@"SELECT PEDIDOID FROM PEDIDO WHERE CODIGOPEDIDO = '{codigoPedido}'";

                using var conn = new SqlConnection(_connectionString);
                return await conn.QueryFirstOrDefaultAsync<int>(query, new { codigoPedido });
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }
    }
}
