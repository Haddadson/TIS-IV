using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MPMG.Repositories.Util
{
    public static class SqlUtil
    {
        public static string FormatarListaParametrosInteiros(List<int> valoresParametros)
        {
            return "(" + string.Join(",", valoresParametros) + ")";
        }

        public static string FormatarListaParametrosAlfanumericos(List<string> valoresParametros)
        {
            Regex alfanumericos = new Regex(@"^[a-zA-Z0-9]+$");
            StringBuilder parametros = new StringBuilder("(");
            bool primeiro = true;

            foreach (var valor in valoresParametros)
            {
                if (valor != null)
                {
                    // Validação para evitar sql injection
                    if (!alfanumericos.IsMatch(valor))
                        throw new ArgumentException("O parâmetro " + valor + " possui caracteres não alfanuméricos.");

                    if (primeiro)
                    {
                        primeiro = false;
                    }
                    else
                    {
                        parametros.Append(",");
                    }

                    parametros.Append("'").Append(valor).Append("'");
                }
            }
            parametros.Append(")");

            return parametros.ToString();
        }

        public static bool ParametroAlfanumericoInvalido(string valor)
        {
            Regex alfanumericos = new Regex(@"^[a-zA-Z0-9]+$");

            return !alfanumericos.IsMatch(valor);
        }

        public static string FormatarParametro(string valor)
        {
            if (valor == null)
                return "NULL";

            return string.Format("\'{0}\'", valor);
        }

        public static string FormatarParametro(int? valor)
        {
            if (valor == null)
                return "NULL";

            return string.Format("{0}", valor);
        }
    }
}
