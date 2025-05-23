using Questao5.Common.Validation;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Validation;

namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public Movimento(Guid idContaCorrente, decimal valor, TipoMovimento tipoMovimento)
        {
            IdContaCorrente = idContaCorrente;
            Valor = valor;
            TipoMovimento = tipoMovimento;

            DataMovimento = DateTime.UtcNow;
            IdMovimento = Guid.NewGuid();
            Validate();
        }

        public Movimento(decimal valor, TipoMovimento tipoMovimento)
        {
            Valor = valor;
            TipoMovimento = tipoMovimento;
        }

        public Guid IdMovimento { get; private set; }
        public DateTime DataMovimento { get; private set; }
        public TipoMovimento TipoMovimento { get; private set; }
        public decimal Valor { get; private set; }
        public Guid IdContaCorrente { get; private set; }
        public ContaCorrente ContaCorrente { get; set; }

        public void AlterarValor(decimal valor)
        {
            Valor = valor;
            Validate();
        }

        public void AlterarTipoMovimento(TipoMovimento tipoMovimento)
        {
            TipoMovimento = tipoMovimento;
            Validate();
        }

        public ValidationResultDetail Validate()
        {
            var validator = new MovimentoValidator();
            var result = validator.Validate(this);

            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
