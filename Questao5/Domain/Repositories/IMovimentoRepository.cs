using Questao5.Domain.Entities;

namespace Questao5.Domain.Repositories
{
    public interface IMovimentoRepository
    {
        /// <summary>
        /// Creates a new movimento in the repository
        /// </summary>
        /// <param name="movimento">The movimento to create</param>
        /// <param name="requisicaoId">The Id of Idempotencia</param>
        /// <returns>The created movimento</returns>
        Task<Movimento> CreateAsync(Movimento movimento, Guid requisicaoId);

        /// <summary>
        /// Return a list of Movimentos of an account
        /// </summary>
        /// <param name="idContaCorrente">The id of Conta Corrente</param>
        /// <returns>A list of Movimentos</returns>
        Task<List<Movimento>> GetByIdContaCorrente(Guid idContaCorrente);
    }
}
