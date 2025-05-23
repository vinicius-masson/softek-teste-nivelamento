using Questao5.Common.Validation;

namespace Questao5.API.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<ValidationErrorDetail> Errors { get; set; } = Array.Empty<ValidationErrorDetail>();
    }
}
