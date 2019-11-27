using Dapper;
using MPMG.Repositories.Entidades;
using MPMG.Repositories.Util;
using MPMG.Services;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MPMG.Repositories
{
    public class TabelaANPRepo : RepositorioBase<TabelaANP>
    {
        private const string SQL_BUSCAR_DADOS_ANP= @"
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
            (mes, ano, produto, id_upload_anp, preco_medio_revenda, preco_maximo_revenda, preco_minimo_revenda)
            VALUES";
        
        public TabelaANP BuscarDadosANP(int SGDP, string produto, int mes, int ano)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@sgdp", SGDP, DbType.Int32);
            parametros.Add("@produto", produto, DbType.AnsiString);
            parametros.Add("@mes", mes, DbType.Int32);
            parametros.Add("@ano", ano, DbType.Int32);

            return Obter(SQL_BUSCAR_DADOS_ANP, parametros);
        }

        public void InserirLoteAnp(List<DataRow> linhas)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (linhas.Any(linha => linha.ItemArray.Any(item => SqlUtil.ParametroAlfanumericoInvalido(item.ToString()))))
                return;

            var sql = stringBuilder.Append(SQL_INSERT_ANP_LOTE);

            //TODO


            Execute(sql.ToString(), null);
        }
    }
}