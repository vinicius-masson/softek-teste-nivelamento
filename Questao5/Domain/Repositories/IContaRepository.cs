using Questao5.Domain.Entities;

namespace Questao5.Domain.Repositories
{
    /// <summary>
    /// Repository interface for ContaRepository entity operations
    /// </summary>
    public interface IContaRepository
    {
        /// <summary>
        /// Retrieves a account by their unique identifier
        /// </summary>
        /// <param name="idContaCorrente">The unique identifier of the account</param>
        /// <returns>The account if found, null otherwise</returns>
        Task<ContaCorrente?> GetByIdAsync(Guid idContaCorrente);
    }
}
