using Bogus;
using Questao5.Application.Commands.Movimentos;
using Questao5.Domain.Enumerators;

namespace Questao5.UnitTests.Aplication.TestData
{
    /// <summary>
    /// Provides methods for generating test data using the Bogus library.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provide both valid and invalid data scenarios.
    /// </summary>
    public static class CreateMovimentoHandlerTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid MovimentoCommands entities.
        /// The generated MovimentoCommands will have valid:
        /// - IdContaCorrente (valid guid)
        /// - TipoMovimento (valid characterd C or D)
        /// - Valor (greater than 0)
        /// </summary>
        private static readonly Faker<CreateMovimentoCommand> createMovimentoHandlerFaker = new Faker<CreateMovimentoCommand>()
            .RuleFor(m => m.RequisicaoId, f => f.Random.Guid())
            .RuleFor(m => m.IdContaCorrente, f => f.Random.Guid())
            .RuleFor(m => m.TipoMovimento, f => f.PickRandom("C", "D"))
            .RuleFor(m => m.Valor, f => f.Random.Decimal(0.01m, 50000));


        /// <summary>
        /// Generates a valid MovimentoCommand class with randomized data.
        /// The generated movimento will have all properties populated with valid values
        /// that meet the system's validation requirements.
        /// </summary>
        /// <returns>A valid MovimentoCmmand class with randomly generated data.</returns>
        public static CreateMovimentoCommand GenerateValidCommand()
        {
            return createMovimentoHandlerFaker.Generate();
        }
    }
}
