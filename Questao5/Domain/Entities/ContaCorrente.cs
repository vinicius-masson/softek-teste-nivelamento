using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class ContaCorrente
    {
        public ContaCorrente(Guid idContaCorrente, int numero, string nome, ContaCorrenteStatus ativo)
        {
            Numero = numero;
            Nome = nome;
            IdContaCorrente = idContaCorrente;
            Ativo = ativo;
        }

        public Guid IdContaCorrente { get; private set; }
        public int Numero { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public ContaCorrenteStatus Ativo { get; private set; }

        public void InativarConta() => Ativo = ContaCorrenteStatus.Inativo;
    }
}
