using AutoMapper;
using MediatR;
using Questao5.Application.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Repositories;

namespace Questao5.Application.Queries.Contas
{
    /// <summary>
    /// Handler for processing GetContaQuery requests
    /// </summary>
    public class GetContaHandler : IRequestHandler<GetContaQuery, GetContaResponse>
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaRepository _contaRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of GetContaHandler
        /// </summary>
        /// <param name="contaRepository">The account repository</param>
        /// <param name="movimentoRepository">The movimento repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public GetContaHandler(IMovimentoRepository movimentoRepository, IContaRepository contaRepository, IMapper mapper)
        {
            _movimentoRepository = movimentoRepository;
            _contaRepository = contaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the GetContaQuery request
        /// </summary>
        /// <param name="request">The GetConta query</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created account details</returns>
        public async Task<GetContaResponse> Handle(GetContaQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.GetByIdAsync(request.IdContaCorrente);

            if (conta == null)
                throw new NotFoundException("Account not found");

            if (conta.Ativo == ContaCorrenteStatus.Inativo)
                throw new BusinessException("Account is inactive");

            var movimentacoes = await _movimentoRepository.GetByIdContaCorrente(request.IdContaCorrente);

            var saldo = CalcularSaldo(movimentacoes);

            return new GetContaResponse
            {
                Data = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                Nome = conta.Nome,
                Numero = conta.Numero,
                SaldoAtual = saldo
            };
        }

        /// <summary>
        /// Calculates the total balance of transactions 
        /// </summary>
        /// <param name="movimentacoes">The GetConta query</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created account details</returns>
        private decimal CalcularSaldo(List<Movimento> movimentacoes)
        {
            var totalCredito = movimentacoes.Where(m => m.TipoMovimento == TipoMovimento.Credito).Sum(m => m.Valor);
            var totalDebito = movimentacoes.Where(m => m.TipoMovimento == TipoMovimento.Debito).Sum(m => m.Valor);
            return totalCredito - totalDebito;
        }
    }
}
