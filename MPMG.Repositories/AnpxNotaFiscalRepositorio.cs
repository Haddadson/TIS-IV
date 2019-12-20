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
                A.mes_fam As MesFam,
                A.ano_fam As AnoFam,
                I.produto As Produto,
                I.quantidade AS Quantidade, 
                I.vunitario AS ValorUnitario,
                I.vrtotal AS ValorTotalItem,
                A.nro_folha AS NumeroFolha,
                B.valor_fam AS ValorFam,
                A.vrtotal_nota As ValorTotalNota,
                C.preco_medio_revenda AS PrecoMedioAnp,
                C.preco_maximo_revenda AS PrecoMaximoAnp,
                C.ano AS AnoAnp,
                C.mes AS MesAnp
            FROM notafiscal A
            join itemnotafiscal I
				on A.sgdp = I.sgdp
                AND A.nr_nota_fiscal = I.nr_nota_fiscal
            JOIN tabelafam B
                ON A.mes_fam = B.mes AND A.ano_fam = B.ano AND A.id_upload_fam = B.id_upload
            JOIN tabelaanp C
                ON MONTH(IFNULL(A.dt_consulta_anp, A.dt_emissao)) = C.mes 
                AND YEAR(IFNULL(A.dt_consulta_anp, A.dt_emissao)) = C.ano 
                AND I.produto = C.produto
                AND C.id_municipio = @IdMunicipio
            WHERE 
                A.sgdp = @Sgdp";

        public List<AnpxNotaFiscal> ListarNotasFiscaisPorSgdp(string sgdp, int idMunicipio)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@Sgdp", sgdp, DbType.AnsiString);
            parametros.Add("@IdMunicipio", idMunicipio, DbType.Int32);

            return Listar(SQL_LISTAR_NOTAS_FISCAIS_POR_SGDP, parametros);
        }
    }
}
