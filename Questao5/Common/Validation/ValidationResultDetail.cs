using FluentValidation.Results;

namespace Questao5.Common.Validation
{
    public class ValidationResultDetail
    {
        public bool IsValid { get; set; }
        public IEnumerable<ValidationErrorDetail> Errors { get; set; } = Array.Empty<ValidationErrorDetail>();

        public ValidationResultDetail()
        {

        }

        public ValidationResultDetail(ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            Errors = validationResult.Errors.Select(o => (ValidationErrorDetail)o);
        }
    }
}
