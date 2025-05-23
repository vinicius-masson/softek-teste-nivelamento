using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        public long Numero { get; }
        public string NomeTitular { get; private set; } = string.Empty;
        public double Saldo { get; private set; }

        public ContaBancaria(long numero, string nomeTitular, double? deposito = null)
        {
            if (string.IsNullOrWhiteSpace(nomeTitular))
                throw new ArgumentException("O nome do titular é obrigatório.");

            Numero = numero;
            NomeTitular = nomeTitular;
            Saldo = deposito.GetValueOrDefault();
        }

        public void Deposito(double valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do depósito deve ser positivo.");

            Saldo += valor;
        }

        public void Saque(double valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do saque deve ser positivo.");

            Saldo -= valor + 3.5;
        }

        public void AtualizarNomeTitular(string nome) => NomeTitular = nome;

        public override string ToString() => $"Conta {Numero}, Titular: {NomeTitular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";

    }
}