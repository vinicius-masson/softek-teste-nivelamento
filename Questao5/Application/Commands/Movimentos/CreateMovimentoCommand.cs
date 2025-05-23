using MediatR;

namespace Questao5.Application.Commands.Movimentos
{
    public class CreateMovimentoCommand : IRequest<CreateMovimentoResponse>
    {
        public Guid RequisicaoId { get; set; }
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; } = string.Empty;
    }
}
