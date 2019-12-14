using Dapper;
using MPMG.Repositories.Entidades;
using MPMG.Repositories.Util;
using System.Collections.Generic;
using System.Data;

namespace MPMG.Repositories
{
    public class MunicipioReferenteRepositorio : RepositorioBase<MunicipioReferente>
    {

        private const string SQL_INSERIR_MUNICIPIO_REFERENTE = @"
            INSERT INTO municipioreferente VALUES (@SGDP, @IdMunicipio, @IdMunicipioReferente, @Ano, @Mes)";

        private const string SQL_OBTER_MUNICIPIO_REFERENTE = @"
            SELECT B.id_municipio AS Codigo,
                   B.nome_municipio AS Nome,
                   C.id_municipio AS CodigoMunicipioReferente,
                   C.nome_municipio AS NomeMunicipioReferente,
                   A.ano AS Ano
            FROM municipioreferente A
            JOIN municipio B
            ON A.id_municipio = B.id_municipio
            JOIN municipio C
            ON A.id_municipio_referente = C.id_municipio 
            WHERE A.id_municipio = @IdMunicipio
	              AND A.ano = @Ano";

        private const string SQL_OBTER_MUNICIPIO_REFERENTE_POR_NOME = @"
            SELECT B.id_municipio AS Codigo,
                   B.nome_municipio AS Nome,
                   C.id_municipio AS CodigoMunicipioReferente,
                   C.nome_municipio AS NomeMunicipioReferente,
                   A.anoreferente AS Ano,
                   A.mesreferente AS Mes
            FROM municipioreferente A
            JOIN municipio B
            ON A.id_municipio = B.id_municipio
            JOIN municipio C
            ON A.id_municipio_referente = C.id_municipio 
            WHERE B.nome_municipio = @NomeMunicipio
	              AND A.anoreferente IN @Ano";

        public MunicipioReferente ObterMunicipioReferente(int idMunicipio, int ano)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@IdMunicipio", idMunicipio, DbType.Int32);
            parametros.Add("@Ano", ano.ToString(), DbType.AnsiStringFixedLength);

            return Obter(SQL_OBTER_MUNICIPIO_REFERENTE, parametros);
        }

        public MunicipioReferente ObterMunicipioReferentePorNome(string nomeMunicipio, List<int> anos)
        {
            DynamicParameters parametros = new DynamicParameters();

            string listaAnos = SqlUtil.FormatarListaParametrosInteiros(anos);
            string sql = SQL_OBTER_MUNICIPIO_REFERENTE_POR_NOME.Replace("@Ano", listaAnos);

            parametros.Add("@NomeMunicipio", nomeMunicipio, DbType.AnsiString);

            return Obter(sql, parametros);
        }

        public void InserirMunicipioReferente(string SGDP, int idMunicipio, int idMunicipioReferente, int ano, int mes)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@SGDP", SGDP, DbType.AnsiString);
            parametros.Add("@IdMunicipio", idMunicipio, DbType.Int32);
            parametros  .Add("@IdMunicipioReferente", idMunicipioReferente, DbType.Int32);
            parametros.Add("@Ano", ano.ToString(), DbType.AnsiStringFixedLength);
            parametros.Add("@Mes", mes.ToString(), DbType.AnsiString);

            Execute(SQL_INSERIR_MUNICIPIO_REFERENTE, parametros);
        }
    }
}
