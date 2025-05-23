using FluentValidation;
using Questao5.Domain.Entities;

namespace Questao5.Domain.Validation
{
    public class MovimentoValidator : AbstractValidator<Movimento>
    {
        public MovimentoValidator()
        {
            RuleFor(m => m.Valor)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("INVALID_VALUE");

            RuleFor(m => m.TipoMovimento)
                .IsInEnum()
                .WithMessage("INVALID_TYPE");
        }
    }
}
