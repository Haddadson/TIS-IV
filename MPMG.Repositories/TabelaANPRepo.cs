using Dapper;
using MPMG.Repositories.Entidades;
using MPMG.Services;
using System.Data;

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
        public TabelaANP BuscarDadosANP(int SGDP, string produto, int mes, int ano)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@sgdp", SGDP, DbType.Int32);
            parametros.Add("@produto", produto, DbType.AnsiString);
            parametros.Add("@mes", mes, DbType.Int32);
            parametros.Add("@ano", ano, DbType.Int32);

            return Obter(SQL_BUSCAR_DADOS_ANP, parametros);
        }
    }
}