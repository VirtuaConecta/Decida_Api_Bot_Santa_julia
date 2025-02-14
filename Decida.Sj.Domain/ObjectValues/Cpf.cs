using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Decida.Sj.Domain.ObjectValues
{
    public sealed class Cpf
    {
        /// <summary>
        /// Armazena o CPF sem pontuação, somente dígitos.
        /// </summary>
        public string Numero { get; }

        private Cpf(string numero)
        {
            Numero = numero;
        }

        /// <summary>
        /// Cria um objeto CPF a partir de uma string bruta.
        /// Lança exceção se o CPF for inválido.
        /// </summary>
        /// <param name="rawCpf">String com ou sem pontuação (ex: "123.456.789-09")</param>
        /// <returns>Cpf válido</returns>
        public static Cpf FromString(string rawCpf)
        {
            // 1. Remover caracteres não numéricos
            string apenasDigitos = Regex.Replace(rawCpf ?? "", "[^0-9]", "");

            // 2. Validar comprimento (deve ter 11 dígitos)
            if (apenasDigitos.Length != 11)
                throw new ArgumentException("CPF deve conter exatamente 11 dígitos.");

            // 3. Verificar se todos os dígitos são iguais (caso típico de CPF inválido como 11111111111)
            bool todosIguais = true;
            for (int i = 1; i < apenasDigitos.Length; i++)
            {
                if (apenasDigitos[i] != apenasDigitos[0])
                {
                    todosIguais = false;
                    break;
                }
            }
            if (todosIguais)
                throw new ArgumentException("CPF inválido: todos os dígitos são iguais.");

            // 4. Validar dígitos verificadores via algoritmo oficial
            if (!ValidaDigitosVerificadores(apenasDigitos))
                throw new ArgumentException("CPF inválido (dígitos verificadores incorretos).");

            // 5. Retornar instância válida
            return new Cpf(apenasDigitos);
        }

        private static bool ValidaDigitosVerificadores(string cpf)
        {
            // Calcula primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += (cpf[i] - '0') * (10 - i);
            }
            int resto = soma % 11;
            int dv1 = (resto < 2) ? 0 : 11 - resto;

            if (dv1 != (cpf[9] - '0'))
                return false;

            // Calcula segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += (cpf[i] - '0') * (11 - i);
            }
            resto = soma % 11;
            int dv2 = (resto < 2) ? 0 : 11 - resto;

            return dv2 == (cpf[10] - '0');
        }

        #region Comparisons (Equals/GetHashCode)

        public override bool Equals(object obj)
        {
            if (obj is not Cpf other)
                return false;

            return Numero == other.Numero;
        }

        public override int GetHashCode()
        {
            return Numero.GetHashCode();
        }

        #endregion

        /// <summary>
        /// Retorna CPF no formato "###.###.###-##"
        /// </summary>
        public override string ToString()
        {
            return ConvertToFormatted(Numero);
        }

        private static string ConvertToFormatted(string cpf)
        {
            return string.Format("{0}.{1}.{2}-{3}",
                cpf.Substring(0, 3),
                cpf.Substring(3, 3),
                cpf.Substring(6, 3),
                cpf.Substring(9, 2));
        }
    }
}
