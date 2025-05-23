using Bogus;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.UnitTests.Domain.Entities.TestData
{
    /// <summary>
    /// Provides methods for generating test data using the Bogus library.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provide both valid and invalid data scenarios.
    /// </summary>
    public static class MovimentoTestData
    {
        // <summary>
        /// Configures the Faker to generate valid movimento entities.
        /// The generated product will have valid:
        /// - TipoMovimento (valid format)
        /// - Valor (valid size)
        /// - IdContaCorrente (valid Guid)
        /// </summary>
        private static readonly Faker<Movimento> MovimentoFaker = new Faker<Movimento>()
            .CustomInstantiator(m => new Movimento(
                tipoMovimento: m.PickRandom(TipoMovimento.Credito, TipoMovimento.Debito),
                valor: m.Random.Decimal(0.01m, 50000),
                idContaCorrente: m.Random.Guid()
            ));

        // <summary>
        /// Generates a valid Movimento entity with randomized data.
        /// The generated user will have all properties populated with valid values
        /// that meet the system's validation requirements.
        /// </summary>
        /// <returns>A valid Movimento entity with randomly generated data.</returns>
        public static Movimento GenerateValidMovimento()
        {
            return MovimentoFaker.Generate();
        }

        /// <summary>
        /// Generates an invalid valor for testing negative scenarios.
        /// The generated valor will:
        /// - Be lesser or equal 0
        /// This is useful for testing valor validation error cases.
        /// </summary>
        /// <returns>An invalid valor</returns>
        public static decimal GenerateInvalidValor()
        {
            return new Faker().Random.Decimal(-100, 0m);
        }
    }
}
