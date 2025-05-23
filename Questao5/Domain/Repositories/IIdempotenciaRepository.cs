using Questao5.Domain.Entities;

namespace Questao5.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Idempotencia entity operations
    /// </summary>
    public interface IIdempotenciaRepository
    {
        /// <summary>
        /// Retrieves a idempotencia by their unique identifier
        /// </summary>
        /// <param name="requisicaoId">The unique identifier of the idempotencia</param>
        /// <returns>The idempotencia if found, null otherwise</returns>
        Task<Idempotencia?> GetByIdAsync(Guid requisicaoId);

        /// <summary>
        /// Creates a new idempotencia in the repository
        /// </summary>
        /// <param name="requisicaoId">The Id o request</param>
        /// <param name="requisicao">The body of request</param>
        /// <param name="resultado">The id of account movement</param>
        /// <returns>The created idempotencia</returns>
        Task CreateAsync(Guid requisicaoId, string requisicao, string resultado);
    }
}
