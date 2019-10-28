using Dapper;
using MPMG.Repositories.Entidades;
using System;
using System.Data;

namespace MPMG.Repositories
{
    public class TabelaUsuarioRepo : RepositorioBase<TabelaUsuario>
    {
        private const string SQL_INSERIR_TABELA = @"
        INSERT INTO `TabelaUsuario` 
        (sgdp, id_municipio, id_municipio_referente, ano_referente, 
        dt_geracao, titulo_aba_1, titulo_aba_2, titulo_aba_3, analista_resp ) 
        VALUES 
        (@SGDP, @IdMunicipio, @IdMunicipioReferente, @AnoReferente, 
        @DataGeracao, @Titulo1, @Titulo2, @Titulo3, @AnalistResponsavel)";

        public bool CadastrarTabela(
            int SGDP, int AnoReferente,
            int IdMunicipio, int IdMunicipioReferente,
            DateTime DataGeracao, string Titulo1,
            string Titulo2, string Titulo3, string AnalistaResponsavel)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@SGDP", SGDP, DbType.Int32);
            parametros.Add("@AnoReferente", AnoReferente, DbType.Int32);
            parametros.Add("@IdMunicipio", IdMunicipio, DbType.Int32);
            parametros.Add("@IdMunicipioReferente", IdMunicipioReferente, DbType.Int32);
            parametros.Add("@DataGeracao", DataGeracao, DbType.DateTime);
            parametros.Add("@Titulo1", Titulo1, DbType.AnsiString);
            parametros.Add("@Titulo2", Titulo2, DbType.AnsiString);
            parametros.Add("@Titulo3", Titulo3, DbType.AnsiString);
            parametros.Add("@AnalistResponsavel", AnalistaResponsavel, DbType.AnsiString);

            return Execute(SQL_INSERIR_TABELA, parametros) > 0;
        }
    }
}
