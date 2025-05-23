using FluentValidation;
using Questao5.Application.Commands.Movimentos;

namespace Questao5.API.Validation
{
    public class CreateMovimentoCommandValidator : AbstractValidator<CreateMovimentoCommand>
    {
        public CreateMovimentoCommandValidator()
        {
            RuleFor(m => m.RequisicaoId)
                .NotEmpty()
                .WithMessage("Requisition Id is required.")
                .Must(id => id != Guid.Empty && id != default(Guid))
                .WithMessage("Requisition Id must be a valid GUID.");

            RuleFor(m => m.IdContaCorrente)
                .NotEmpty()
                .WithMessage("Account Id is required.")
                .Must(id => id != Guid.Empty && id != default(Guid))
                .WithMessage("Account Id must be a valid GUID.");

            RuleFor(m => m.Valor)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Valor must be greater than 0");

            RuleFor(m => m.TipoMovimento)
                .NotEmpty()
                .WithMessage("TipoMovimento is required.")
                .Must(t => t?.Trim().ToUpper() == "C" || t?.Trim().ToUpper() == "D")
                .WithMessage("Movimento type must be 'C' (Credit) or 'D' (Debit).");
        }
    }
}
