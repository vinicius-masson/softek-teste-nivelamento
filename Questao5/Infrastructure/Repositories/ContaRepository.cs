using Dapper;
using Microsoft.Data.Sqlite;
using NSubstitute.Core;
using Questao5.Domain.DTOs;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IContaRepository using Dapper
    /// </summary>
    public class ContaRepository : IContaRepository
    {
        private readonly IDatabaseConfig _databaseConfig;

        public ContaRepository(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        private IDbConnection CreateConnection() => new SqliteConnection(_databaseConfig.Name);

        // <summary>
        /// Retrieves a account by by their unique identifier
        /// </summary>
        /// <param name="idContaCorrente">The unique identifier of the account</param>
        /// <returns>The account if found, null otherwise</returns>
        public async Task<ContaCorrente?> GetByIdAsync(Guid idContaCorrente)
        {
            var sql = @"SELECT 
                        idcontacorrente as IdContaCorrente,
                        numero as Numero, 
                        nome as Nome, 
                        ativo as Ativo
                        FROM contacorrente 
                        WHERE idcontacorrente = @id";

            using var connection = CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync(sql, new { id = idContaCorrente.ToString().ToUpper() });

            return new ContaCorrente(Guid.Parse(result.IdContaCorrente), Convert.ToInt32(result.Numero), result.Nome, result.Ativo == 1 ? ContaCorrenteStatus.Ativo : ContaCorrenteStatus.Inativo);
        }
    }
}
