using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.API.Common;
using Questao5.API.Validation;
using Questao5.Application.Commands.Movimentos;
using Questao5.Application.Queries.Contas;

namespace Questao5.API.Controllers
{
    // <summary>
    /// Controller for managing movimento operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of MovimentosController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public MovimentosController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new movimento
        /// </summary>
        /// <param name="request">The movimento creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created movimento id</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateMovimentoCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateMovimentoCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var response = await _mediator.Send(request);

            return Ok(response.IdMovimento);
        }

        /// <summary>
        /// Retrieves account details, including balance (calculated from linked movements)
        /// </summary>
        /// <param name="id">The unique identifier of the account</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The movimento details if found</returns>
        [HttpGet("{idContaCorrente}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetContaQuery>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetContaWithBalance([FromRoute] Guid idContaCorrente, CancellationToken cancellationToken)
        {
            var request = new GetContaQuery { IdContaCorrente = idContaCorrente };
            var validator = new GetContaQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var response = await _mediator.Send(request, cancellationToken);

            return Ok(new ApiResponseWithData<GetContaResponse>
            {
                Success = true,
                Message = "Account retrieved successfully",
                Data = _mapper.Map<GetContaResponse>(response)
            });
        }
    }
}
