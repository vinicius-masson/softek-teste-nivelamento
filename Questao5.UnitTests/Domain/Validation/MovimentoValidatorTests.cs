using FluentValidation.TestHelper;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Validation;
using Questao5.UnitTests.Domain.Entities.TestData;

namespace Questao5.UnitTests.Domain.Validation
{
    /// <summary>
    /// Contains unit tests for the MovimentoValidator class.
    /// Tests cover validation of Movimento properties
    /// </summary>
    public class MovimentoValidatorTests
    {
        private readonly MovimentoValidator _validator;

        public MovimentoValidatorTests()
        {
            _validator = new MovimentoValidator();
        }

        /// <summary>
        /// Tests that validation passes when all movimento properties are valid.
        /// This test verifies that a movimento with valid:
        /// - TipoMovimento ('C' or 'D' characters)
        /// - Valor (Greater than 0)
        /// passes all validation rules without any errors.
        /// </summary>
        [Fact(DisplayName = "Valid movimento should pass all validation rules")]
        public void Given_ValidMovimento_When_Validated_Then_ShouldNotHaveErrors()
        {
            // Arrange
            var movimento = MovimentoTestData.GenerateValidMovimento();

            // Act
            var result = _validator.TestValidate(movimento);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        /// <summary>
        /// Tests that validation fails for invalid valor formats.
        /// This test verifies that valor that:
        /// - Negative or zero
        /// fail validation with appropriate error messages.
        /// </summary>
        [Fact(DisplayName = "Invalid valor formats should fail validation")]
        public void Given_InvalidValor_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
            var movimento = MovimentoTestData.GenerateValidMovimento();
            movimento.AlterarValor(MovimentoTestData.GenerateInvalidValor());

            // Act
            var result = _validator.TestValidate(movimento);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Valor);
        }

        /// <summary>
        /// Tests that validation fails when TipoMovimento has an invalid enum value
        /// </summary>
        [Fact(DisplayName = "Invalid TipoMovimento should fail validation")]
        public void Given_InvalidTipoMovimento_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
            var movimento = MovimentoTestData.GenerateValidMovimento();
            movimento.AlterarTipoMovimento((TipoMovimento)'X');

            // Act
            var result = _validator.TestValidate(movimento);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TipoMovimento);
        }
    }
}
