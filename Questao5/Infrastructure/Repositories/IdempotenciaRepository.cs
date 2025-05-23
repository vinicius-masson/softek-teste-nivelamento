using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IIdempotenciaRepository using Dapper
    /// </summary>
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly IDatabaseConfig _databaseConfig;

        public IdempotenciaRepository(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        private IDbConnection CreateConnection() => new SqliteConnection(_databaseConfig.Name);

        public async Task<Idempotencia?> GetByIdAsync(Guid requisicaoId)
        {
            var sql = @"SELECT chave_idempotencia as Chave_Idempotencia,
                               requisicao as Requisicao,
                               resultado as Resultado
                        FROM   Idempotencia
                        WHERE  CHAVE_IDEMPOTENCIA = @requisicaoId";

            using var conn = CreateConnection();

            var result = await conn.QueryFirstOrDefaultAsync(sql, new { requisicaoId = requisicaoId.ToString().ToUpper() });

            if (result == null)
                return null;

            return new Idempotencia(Guid.Parse(result.Chave_Idempotencia), result.Requisicao, result.Resultado);
        }

        // <summary>
        /// Creates a new idempotencia in the database
        /// </summary>
        /// <param name="requisicaoId">The Id o request</param>
        /// <param name="requisicao">The body of request</param>
        /// <param name="resultado">The id of account movement</param>
        /// <returns>The created idempotencia</returns>
        public async Task CreateAsync(Guid requisicaoId, string requisicao, string resultado)
        {
            var sql = @"INSERT INTO IDEMPOTENCIA
                        (Chave_Idempotencia, Requisicao, Resultado)
                        VALUES (@requisicaoId, @requisicao, @resultado)";

            using var connection = CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(sql, new
                {
                    requisicaoId = requisicaoId.ToString().ToUpper(),
                    requisicao = requisicao,
                    resultado = resultado
                }, transaction);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
