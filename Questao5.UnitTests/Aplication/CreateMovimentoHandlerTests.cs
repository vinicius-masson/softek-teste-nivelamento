using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Questao5.Application.Commands.Movimentos;
using Questao5.Application.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Repositories;
using Questao5.UnitTests.Aplication.TestData;

namespace Questao5.UnitTests.Aplication
{
    /// <summary>
    /// Contains unit tests for the <see cref="CreateMovimentoHandler"/> class.
    /// </summary>
    public class CreateMovimentoHandlerTests
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly IContaRepository _contaRepository;
        private readonly IMapper _mapper;
        private readonly CreateMovimentoHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMovimentoHandlerTests"/> class.
        /// Sets up the test dependencies and creates fake data generators.
        /// </summary>
        public CreateMovimentoHandlerTests()
        {
            _movimentoRepository = Substitute.For<IMovimentoRepository>();
            _contaRepository = Substitute.For<IContaRepository>();
            _idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateMovimentoHandler(_idempotenciaRepository, _movimentoRepository, _contaRepository, _mapper);
        }

        /// <summary>
        /// Tests that a valid movimento creation request is handled successfully.
        /// </summary>
        [Fact(DisplayName = "Given valid movimento data When creating movimento Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Given
            var command = CreateMovimentoHandlerTestData.GenerateValidCommand();
            var movimento = new Movimento(command.IdContaCorrente, command.Valor, (TipoMovimento)Convert.ToChar(command.TipoMovimento));
            var conta = new ContaCorrente(command.IdContaCorrente, 7864, "Conta Teste", ContaCorrenteStatus.Ativo);
            var result = new CreateMovimentoResponse
            {
                IdMovimento = movimento.IdMovimento
            };


            _mapper.Map<Movimento>(command).Returns(movimento);
            _mapper.Map<CreateMovimentoResponse>(movimento).Returns(result);

            _movimentoRepository.CreateAsync(Arg.Any<Movimento>(), command.RequisicaoId)
                .Returns(movimento);

            _contaRepository.GetByIdAsync(command.IdContaCorrente)
                .Returns(conta);

            _idempotenciaRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(Task.FromResult<Idempotencia?>(null));

            // When
            var createMovimentoResult = await _handler.Handle(command, CancellationToken.None);

            // Then
            createMovimentoResult.Should().NotBeNull();
            createMovimentoResult.IdMovimento.Should().Be(movimento.IdMovimento);
            await _movimentoRepository.Received(1).CreateAsync(Arg.Any<Movimento>(), command.RequisicaoId);
        }

        /// <summary>
        /// Tests that the account in movimento exists
        /// </summary>
        [Fact(DisplayName = "Given a movimento with a non-existent account When creating movimento Then throws NotFoundException exception")]
        public async Task Handle_InvalidRequest_ThrowsNotFoundException()
        {
            // Given
            var command = CreateMovimentoHandlerTestData.GenerateValidCommand();
            
            _idempotenciaRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(Task.FromResult<Idempotencia?>(null));

            // When
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<NotFoundException>();
        }

        /// <summary>
        /// Tests that the account in movimento is active
        /// </summary>
        [Fact(DisplayName = "Given a movimento with an account inactive When creating movimento Then throws Business exception")]
        public async Task Handle_InvalidRequest_ThrowsBusinessException()
        {
            // Given
            var command = CreateMovimentoHandlerTestData.GenerateValidCommand();
            var movimento = new Movimento(command.IdContaCorrente, command.Valor, (TipoMovimento)Convert.ToChar(command.TipoMovimento));
            var conta = new ContaCorrente(command.IdContaCorrente, 7864, "Conta Teste", ContaCorrenteStatus.Ativo);
            conta.InativarConta();
            var result = new CreateMovimentoResponse
            {
                IdMovimento = movimento.IdMovimento
            };


            _mapper.Map<Movimento>(command).Returns(movimento);
            _mapper.Map<CreateMovimentoResponse>(movimento).Returns(result);

            _movimentoRepository.CreateAsync(Arg.Any<Movimento>(), Guid.NewGuid())
                .Returns(movimento);

            _contaRepository.GetByIdAsync(command.IdContaCorrente)
                .Returns(conta);

            // When
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<BusinessException>();
        }
    }
}
