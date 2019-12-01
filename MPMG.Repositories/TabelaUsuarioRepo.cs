using Dapper;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
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

        private const string SQL_LISTAR_TABELAS = @"
            SELECT  
                sgdp AS SGDP, 
                A.id_municipio AS IdMunicipio,
                B.nome_municipio AS NomeMunicipio,
                id_municipio_referente AS IdMunicipioReferente, 
                C.nome_municipio AS NomeMunicipioReferente,
                ano_referente AS AnoReferente, 
                dt_geracao AS DataGeracao, 
                titulo_aba_1 AS Titulo1, 
                titulo_aba_2 AS Titulo2, 
                titulo_aba_3 AS Titulo3, 
                analista_resp AS AnalistaResponsavel
            FROM `TabelaUsuario` A
            LEFT JOIN `municipio` B
            on A.id_municipio = B.id_municipio
            LEFT JOIN `municipio` C
            on A.id_municipio_referente = C.id_municipio";

        private const string SQL_LISTAR_SGDPS_TABELAS = @"
            SELECT  
                sgdp AS SGDP
            FROM `TabelaUsuario` A";

        private const string SQL_OBTER_TABELA_POR_SGDP = @"
            SELECT  
                sgdp AS SGDP, 
                A.id_municipio AS IdMunicipio,
                B.nome_municipio AS NomeMunicipio,
                id_municipio_referente AS IdMunicipioReferente, 
                C.nome_municipio AS NomeMunicipioReferente,
                ano_referente AS AnoReferente, 
                dt_geracao AS DataGeracao, 
                titulo_aba_1 AS Titulo1, 
                titulo_aba_2 AS Titulo2, 
                titulo_aba_3 AS Titulo3, 
                analista_resp AS AnalistaResponsavel
            FROM `TabelaUsuario` A
            LEFT JOIN `municipio` B
            on A.id_municipio = B.id_municipio
            LEFT JOIN `municipio` C
            on A.id_municipio_referente = C.id_municipio
            WHERE sgdp = @Sgdp";

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

        public List<TabelaUsuario> ListarTabelas()
        {
            return Listar(SQL_LISTAR_TABELAS, null);
        }

        public List<TabelaUsuario> ListarSgdpsTabelas()
        {
            return Listar(SQL_LISTAR_SGDPS_TABELAS, null);
        }

        public TabelaUsuario ObterTabelaPorSgdp(int sgdp)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@SGDP", sgdp, DbType.Int32);

            return Obter(SQL_OBTER_TABELA_POR_SGDP, parametros);
        }
    }
}
