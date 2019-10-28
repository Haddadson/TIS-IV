using Dapper;
using MPMG.Repositories.Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPMG.Repositories
{
    public class TabelaUsuarioRepo : RepositorioBase<DadosAnp>
    {
        private const string SQL_INSERIR_TABELA = @"
            INSERT INTO (sgpd, ano_referente, id_municipio, id_municipio_referente, mes, ano, dt_geracao, ano_referente, titulo_1, titulo_2, titulo_3, analista_resp ) 
            VALUES (@SGPD, @AnoReferente, @IdMunicipio, @IdMunicipioReferente, @MesANP, @AnoANP, @DataGeracao, @Titulo1, @Titulo2, @Titulo3, @AnalistResponsavel)";

        public DadosAnp cadastrarTabela(int mes, int ano, string estado, string municipio, string produto)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@SGPD", mes, DbType.Int32);
            parametros.Add("@AnoReferente", mes, DbType.Int32);
            parametros.Add("@IdMunicipio", mes, DbType.Int32);
            parametros.Add("@IdMunicipioReferente", mes, DbType.Int32);
            parametros.Add("@AnoANP", ano, DbType.Int32);
            parametros.Add("@DataGeracao", mes, DbType.Int32);
            parametros.Add("@Titulo1", mes, DbType.Int32);
            parametros.Add("@Titulo2", mes, DbType.Int32);
            parametros.Add("@Titulo3", mes, DbType.Int32);
            parametros.Add("@AnalistaResponsavel", mes, DbType.Int32);

            return Obter(SQL_INSERIR_TABELA, parametros);
        }
    }
}
