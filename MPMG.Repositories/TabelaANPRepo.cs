using Dapper;
using MPMG.Repositories.Entidades;
using MPMG.Repositories.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MPMG.Repositories
{
    public class TabelaANPRepo : RepositorioBase<TabelaANP>
    {
        private const string SQL_BUSCAR_DADOS_ANP = @"
            SELECT `id_municipio` AS municipio,
                    `mes`,
                    `ano`,
                    `produto`,
                    `preco_medio_revenda` AS precoMedio,
                    `preco_maximo_revenda` AS precoMaximo
             FROM `tabelaanp`
            WHERE `id_municipio` = (SELECT `id_municipio_referente` FROM `tabelausuario` WHERE `sgdp` = @sgdp)
              AND `mes` = @mes
              AND `ano` = @ano
              AND `produto` = @produto";

        private const string SQL_INSERT_ANP_LOTE = @"
            INSERT INTO `tabelaanp`
            (mes, ano, produto, id_upload_anp, preco_medio_revenda, preco_maximo_revenda, id_municipio)
            VALUES";

        private const string SQL_DELETAR_REGISTROS_EXISTENTES = "DELETE FROM `tabelaanp`";

        public TabelaANP BuscarDadosANP(int SGDP, string produto, int mes, int ano)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@sgdp", SGDP, DbType.Int32);
            parametros.Add("@produto", produto, DbType.AnsiString);
            parametros.Add("@mes", mes, DbType.Int32);
            parametros.Add("@ano", ano, DbType.Int32);

            return Obter(SQL_BUSCAR_DADOS_ANP, parametros);
        }

        public void InserirLoteAnp(List<DataRow> linhas, int idUploadAnp)
        {
            if (!linhas.Any())
                return;

            StringBuilder stringBuilder = new StringBuilder();

            var sql = stringBuilder.AppendLine(SQL_INSERT_ANP_LOTE);

            foreach (var linha in linhas)
            {
                DateTime data = DateTime.ParseExact(linha["MÊS"].ToString().Split(' ')[0], "dd/MM/yyyy", new CultureInfo("pt-br"));

                sql.Append("(")
                   .Append($"{SqlUtil.FormatarParametro(data.Month.ToString())}, ")
                   .Append($"{SqlUtil.FormatarParametro(data.Year.ToString())}, ")
                   .Append($"{SqlUtil.FormatarParametro(linha["PRODUTO"].ToString())}, ")
                   .Append($"{SqlUtil.FormatarParametro(idUploadAnp)}, ")
                   .Append($"{SqlUtil.FormatarParametro(linha["PREÇO MÉDIO REVENDA"].ToString().Replace(',', '.'))}, ")
                   .Append($"{SqlUtil.FormatarParametro(linha["PREÇO MÁXIMO REVENDA"].ToString().Replace(',', '.'))}, ")
                   .Append($"{FormatarMunicipioParaConsulta(linha["MUNICÍPIO"].ToString())}")
                   .AppendLine("), ");
            }

            string sqlString = sql.ToString();

            sqlString = sqlString.Substring(0, sqlString.Length - 4);

            Execute(sqlString, null);
        }

        public void DeletarTodosRegistros()
        {
            Execute(SQL_DELETAR_REGISTROS_EXISTENTES, null);
        }

        private string FormatarMunicipioParaConsulta(string municipio)
        {
            return $"(SELECT A.id_municipio FROM `municipio` A WHERE A.nome_municipio = '{municipio}')";
        }
    }
}