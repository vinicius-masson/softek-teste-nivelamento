namespace Questao5.Application.Queries.Contas
{
    public class GetContaResponse
    {
        public int Numero { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public decimal SaldoAtual { get; set; }
    }
}
