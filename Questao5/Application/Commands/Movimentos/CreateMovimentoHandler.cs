using AutoMapper;
using MediatR;
using Questao5.Application.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Repositories;

namespace Questao5.Application.Commands.Movimentos
{
    /// <summary>
    /// Handler for processing CreateMovimentoCommand requests
    /// </summary>
    public class CreateMovimentoHandler : IRequestHandler<CreateMovimentoCommand, CreateMovimentoResponse>
    {
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaRepository _contaRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of CreateMovimentoHandler
        /// </summary>
        /// <param name="idempotenciaRepository">The idempotencia repository</param>
        /// <param name="movimentoRepository">The movimento repository</param>
        /// <param name="contaRepository">The account repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public CreateMovimentoHandler(IIdempotenciaRepository idempotenciaRepository, IMovimentoRepository movimentoRepository, IContaRepository contaRepository, IMapper mapper)
        {
            _idempotenciaRepository = idempotenciaRepository;
            _movimentoRepository = movimentoRepository;
            _contaRepository = contaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the CreateMovimentoCommand request
        /// </summary>
        /// <param name="command">The CreateMovimento command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created movimento details</returns>
        public async Task<CreateMovimentoResponse> Handle(CreateMovimentoCommand command, CancellationToken cancellation)
        {
            var requisicao = await _idempotenciaRepository.GetByIdAsync(command.RequisicaoId);
            if (requisicao != null)
            {
                if (Guid.TryParse(requisicao.Resultado, out var resultado))
                    return new CreateMovimentoResponse { IdMovimento = resultado };

                throw new InvalidOperationException($"Requisição já processada com erro: {requisicao.Resultado}");
            }

            var movimento = new Movimento(
                command.IdContaCorrente,
                command.Valor,
                command.TipoMovimento == "C" ? TipoMovimento.Credito : TipoMovimento.Debito
            );

            var conta = await _contaRepository.GetByIdAsync(command.IdContaCorrente);

            if (conta == null)
                throw new NotFoundException("Account not found");

            if (conta.Ativo == ContaCorrenteStatus.Inativo)
                throw new BusinessException("Account is inactive.");

            var movimentoCriado = await _movimentoRepository.CreateAsync(movimento, command.RequisicaoId);

            var movimentoResult = _mapper.Map<CreateMovimentoResponse>(movimentoCriado);
            return movimentoResult;
        }
    }
}
