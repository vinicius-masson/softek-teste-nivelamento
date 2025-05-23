using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Text.Json;

namespace Questao5.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IMovimentoRepository using Dapper
    /// </summary>
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly IDatabaseConfig _databaseConfig;

        public MovimentoRepository(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        private IDbConnection CreateConnection() => new SqliteConnection(_databaseConfig.Name);

        /// <summary>
        /// Creates a new movimento in the database
        /// </summary>
        /// <param name="movimento">The movimento to create</param>
        /// <param name="requisicaoId">The id of Idempotencia</param>
        /// <returns>The created movimento</returns>
        public async Task<Movimento> CreateAsync(Movimento movimento, Guid requisicaoId)
        {   
            var sqlMovimento = @"INSERT INTO MOVIMENTO
                        (IdMovimento, DataMovimento, TipoMovimento, Valor, IdContaCorrente)
                        VALUES (@IdMovimento, @DataMovimento, @TipoMovimento, @Valor, @IdContaCorrente)";

            var sqlIdempotencia = @"INSERT INTO IDEMPOTENCIA
                        (Chave_Idempotencia, Requisicao, Resultado)
                        VALUES (@requisicaoId, @requisicao, @resultado)";

            using var connection = CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(sqlMovimento, new
                {
                    IdMovimento = movimento.IdMovimento.ToString().ToUpper(),
                    IdContaCorrente = movimento.IdContaCorrente.ToString().ToUpper(),
                    Valor = movimento.Valor,
                    TipoMovimento = movimento.TipoMovimento.ToString()[0],
                    DataMovimento = movimento.DataMovimento
                }, transaction);

                await connection.ExecuteAsync(sqlIdempotencia, new
                {
                    requisicaoId = requisicaoId.ToString().ToUpper(),
                    requisicao = JsonSerializer.Serialize(movimento),
                    resultado = movimento.IdMovimento.ToString().ToUpper()
                }, transaction);

                transaction.Commit();
                return movimento;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Retrieves all movimentos by account id
        /// </summary>
        /// <param name="idContaCorrente">The unique identifier of the account</param>
        /// <returns>The movimentos of an account</returns>
        public async Task<List<Movimento>> GetByIdContaCorrente(Guid idContaCorrente)
        {
            var sql = @"SELECT   tipomovimento as TipoMovimento,
                                 valor as Valor
                        FROM     MOVIMENTO
                        WHERE    IdContaCorrente = @idContaCorrente";

            using var connection = CreateConnection();
            connection.Open();

            try
            {
                var result = await connection.QueryAsync(sql, new { idContaCorrente = idContaCorrente.ToString().ToUpper() });
                List<Movimento> movimentacoes = new List<Movimento>();

                foreach (var movimento in result)
                {
                    movimentacoes.Add(new Movimento(Convert.ToDecimal(movimento.Valor), movimento.TipoMovimento == "C" ? TipoMovimento.Credito : TipoMovimento.Debito));
                }
                
                connection.Close();
                return movimentacoes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            
        }
    }
}
