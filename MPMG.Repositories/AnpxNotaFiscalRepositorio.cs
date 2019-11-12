using Dapper;
using MPMG.Repositories.Entidades;
using System.Collections.Generic;
using System.Data;

namespace MPMG.Repositories
{
    public class AnpxNotaFiscalRepositorio : RepositorioBase<AnpxNotaFiscal>
    {
        private const string SQL_LISTAR_NOTAS_FISCAIS_POR_SGDP = @"
            SELECT 
                A.dt_emissao AS DataGeracao,
                A.nr_nota_fiscal AS NumeroNotaFiscal,
                A.id_tipo_combustivel AS Combustivel,
                A.quantidade AS Quantidade, 
                A.preco_unitario AS ValorUnitario,
                A.vrtotal AS ValorTotal,
                A.nro_folha AS NumeroFolha,
                B.valor_fam AS ValorFam,
                C.preco_medio_revenda AS PrecoMedioRevenda,
                C.preco_maximo_revenda AS PrecoMaximoRevenda,
                C.ano AS AnoAnp,
                C.mes AS MesAnp
            FROM notafiscal A
            JOIN tabelafam B
                ON A.mes_fam = B.mes AND A.ano_fam = B.ano
            JOIN tabelaanp C
                ON MONTH(A.dt_consulta_anp) = C.mes AND YEAR(A.dt_consulta_anp) = C.ano AND C.id_municipio = @IdMunicipio
            WHERE 
                A.sgdp = @Sgdp";

        public List<AnpxNotaFiscal> ListarNotasFiscaisPorSgdp(int sgdp, int idMunicipio)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@Sgdp", sgdp, DbType.Int32);
            parametros.Add("@IdMunicipio", idMunicipio, DbType.Int32);

            return Listar(SQL_LISTAR_NOTAS_FISCAIS_POR_SGDP, parametros);
        }
    }
}
