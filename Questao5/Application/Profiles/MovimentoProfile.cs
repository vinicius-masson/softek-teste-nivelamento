using AutoMapper;
using Questao5.Application.Commands.Movimentos;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
namespace Questao5.Application.Profiles
{
    public class MovimentoProfile : Profile
    {
        public MovimentoProfile()
        {
            CreateMap<Movimento, CreateMovimentoResponse>();
        }
    }
}
