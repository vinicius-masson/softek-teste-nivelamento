using FluentValidation;
using Questao5.Application.Queries.Contas;

namespace Questao5.API.Validation
{
    public class GetContaQueryValidator : AbstractValidator<GetContaQuery>
    {
        public GetContaQueryValidator()
        {
            RuleFor(c => c.IdContaCorrente)
                .NotEmpty()
                .WithMessage("Account Id is Required.")
                .Must(id => id != Guid.Empty && id != default(Guid))
                .WithMessage("Account Id must be a valid GUID.");
        }
    }
}
