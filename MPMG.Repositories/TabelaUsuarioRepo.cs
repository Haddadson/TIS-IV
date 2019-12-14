using Dapper;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPMG.Repositories
{
    public class TabelaUsuarioRepo : RepositorioBase<TabelaUsuario>
    {
        private const string SQL_INSERIR_TABELA = @"
            INSERT INTO `TabelaUsuario` 
            (sgdp, id_municipio, id_municipio_referente,
            dt_geracao, titulo_aba_1, titulo_aba_2, titulo_aba_3, analista_resp ) 
            VALUES 
            (@SGDP, @IdMunicipio, @IdMunicipioReferente, 
            @DataGeracao, @Titulo1, @Titulo2, @Titulo3, @AnalistResponsavel)";

        private const string SQL_LISTAR_TABELAS = @"
            SELECT  
                sgdp AS SGDP, 
                A.id_municipio AS IdMunicipio,
                B.nome_municipio AS NomeMunicipio,
                id_municipio_referente AS IdMunicipioReferente, 
                C.nome_municipio AS NomeMunicipioReferente,
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
                A.id_municipio_referente AS IdMunicipioReferente, 
                D.nome_municipio AS NomeMunicipioReferente,
				dt_geracao AS DataGeracao, 
                C.anoreferente AS AnoReferente,
                C.mesreferente AS MesReferente,
                titulo_aba_1 AS Titulo1, 
                titulo_aba_2 AS Titulo2, 
                titulo_aba_3 AS Titulo3, 
                analista_resp AS AnalistaResponsavel
            FROM `TabelaUsuario` A
            INNER JOIN `municipio` B
            on A.id_municipio = B.id_municipio
            LEFT JOIN `municipioreferente` C 
            ON A.id_municipio = C.id_municipio AND
               A.id_municipio_referente = C.id_municipio_referente
            LEFT JOIN `municipio` D
            on A.id_municipio_referente = D.id_municipio
            WHERE A.sgdp = @Sgdp";

        private const string SQL_INSERIR_ANOS_POR_TABELA = @"
            INSERT INTO `anosportabela`
            VALUES (@SGDP, @AnoReferente)            
        ";
        private const string SQL_OBTER_ANOS_REFERENTES_POR_SGDP = @"
            SELECT AnoReferente FROM anosportabela WHERE SGDP = @Sgdp";

        public bool CadastrarTabela(
            string SGDP,
            int IdMunicipio, int IdMunicipioReferente,
            DateTime DataGeracao, string Titulo1,
            string Titulo2, string Titulo3, string AnalistaResponsavel)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@SGDP", SGDP, DbType.AnsiString);
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

        public List<int> ListarAnosReferentesPorSgdp(string sgdp)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Sgdp", sgdp, DbType.AnsiString);

            return Query<int>(SQL_OBTER_ANOS_REFERENTES_POR_SGDP, parametros).ToList();
        }
        public TabelaUsuario ObterTabelaPorSgdp(string sgdp)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@SGDP", sgdp, DbType.AnsiString);

            return Obter(SQL_OBTER_TABELA_POR_SGDP, parametros);
        }

        public bool CadastrarAnoReferente (string sgdp, int anoReferente)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@SGDP", sgdp, DbType.AnsiString);
            parametros.Add("@AnoReferente", anoReferente, DbType.Int32);

            return Execute(SQL_INSERIR_ANOS_POR_TABELA, parametros) > 0;
        }
    }
}
