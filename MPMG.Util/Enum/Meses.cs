using System.Collections.Generic;
using System.Linq;

namespace MPMG.Util.Enum
{
    public class Meses
    {
        public static readonly Meses JANEIRO = new Meses(1, "janeiro", "jan");
        public static readonly Meses FEVEREIRO = new Meses(2, "fevereiro", "fev");
        public static readonly Meses MARCO = new Meses(3, "março", "mar");
        public static readonly Meses ABRIL = new Meses(4, "abril", "abr");
        public static readonly Meses MAIO = new Meses(5, "maio", "mai");
        public static readonly Meses JUNHO = new Meses(6, "junho", "jun");
        public static readonly Meses JULHO = new Meses(7, "julho", "jul");
        public static readonly Meses AGOSTO = new Meses(8, "agosto", "ago");
        public static readonly Meses SETEMBRO = new Meses(9, "setembro", "set");
        public static readonly Meses OUTUBRO = new Meses(10, "outubro", "out");
        public static readonly Meses NOVEMBRO = new Meses(11, "novembro", "nov");
        public static readonly Meses DEZEMBRO = new Meses(12, "dezembro", "dez");

        public static readonly List<Meses> Todos = new List<Meses>()
        {
            JANEIRO, FEVEREIRO, MARCO, ABRIL, MAIO, JUNHO,
            JULHO, AGOSTO, SETEMBRO, OUTUBRO, NOVEMBRO, DEZEMBRO
        };

        public int Numero;
        public string Nome;
        public string NomeReduzido;

        public Meses(int numero, string nome, string nomeReduzido)
        {
            Numero = numero;
            Nome = nome;
            NomeReduzido = nomeReduzido;
        }

        public static Meses ObterMesPorNomeReduzido(string nomeReduzido)
        {
            return Todos.FirstOrDefault(mes => mes.NomeReduzido == nomeReduzido);
        }

        public static Meses ObterMesPorNome(string nome)
        {
            return Todos.FirstOrDefault(mes => mes.Nome == nome);
        }

        public static Meses ObterMesPorNumero(int numero)
        {
            return Todos.FirstOrDefault(mes => mes.Numero == numero);
        }
    }
}
