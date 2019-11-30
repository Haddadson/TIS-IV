using Dapper;
using MPMG.Repositories.Entidades;
using MPMG.Repositories.Util;
using MPMG.Util.Enum;
using System.Data;
using System.Linq;
using System.Text;

namespace MPMG.Repositories
{
    public class TabelaFamRepo : RepositorioBase<TabelaFam>
    {
        private const string SQL_BUSCAR_DADOS_FAM_POR_MES_ANO = @"
            SELECT  `mes` AS Mes,
                    `ano` AS Ano,
                    `id_upload` AS IdUpload,
                    `valor_fam` AS ValorFam
             FROM `tabelafam`
            WHERE `mes` = @mes
              AND `ano` = @ano";

        private const string SQL_INSERT_FAM_LOTE = @"
            INSERT INTO `tabelafam`
            (mes, ano, id_upload, valor_fam)
            VALUES";

        private const string SQL_DELETAR_REGISTROS_EXISTENTES = "DELETE FROM `tabelafam`";

        public TabelaFam BuscarDadosFam(int mes, int ano)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@mes", mes, DbType.Int32);
            parametros.Add("@ano", ano, DbType.Int32);

            return Obter(SQL_BUSCAR_DADOS_FAM_POR_MES_ANO, parametros);
        }

        public void InserirLoteFam(DataRow linha, int idUploadFam)
        {
            if (linha == null)
                return;

            StringBuilder stringBuilder = new StringBuilder();

            var sql = stringBuilder.AppendLine(SQL_INSERT_FAM_LOTE);

            for (int i = 1; i < linha.ItemArray.Count(); i++)
            {
                if (string.IsNullOrWhiteSpace(linha.ItemArray[i].ToString()))
                    continue;

                sql.Append("(")
                   .Append($"{SqlUtil.FormatarParametro(Meses.ObterMesPorNomeReduzido(linha.Table.Columns[i].ColumnName.ToLower())?.Numero)}, ")
                   .Append($"{SqlUtil.FormatarParametro(linha.ItemArray[0].ToString())}, ")
                   .Append($"{SqlUtil.FormatarParametro(idUploadFam)}, ")
                   .Append($"{SqlUtil.FormatarParametro(linha.ItemArray[i].ToString())}")
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