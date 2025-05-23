using MediatR;

namespace Questao5.Application.Queries.Contas
{
    public class GetContaQuery : IRequest<GetContaResponse>
    {
        public Guid IdContaCorrente { get; set; }
    }
}
