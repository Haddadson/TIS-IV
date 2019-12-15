using Dapper;
using MPMG.Repositories.Entidades;
using MPMG.Repositories.Util;
using System.Collections.Generic;
using System.Data;

namespace MPMG.Repositories
{
    public class MunicipioRepositorio : RepositorioBase<Municipio>
    {

        private const string SQL_INSERIR_MUNICIPIO = @"
            INSERT INTO municipio (nome_municipio) VALUES (@NomeMunicipio)";

        private const string SQL_BUSCAR_MUNICIPIO = @"
            SELECT id_municipio AS Codigo, 
                   nome_municipio AS Nome 
            FROM municipio 
            WHERE nome_municipio = @NomeMunicipio";


        private const string SQL_OBTER_MUNICIPIO_ANP_POR_ANO_E_NOME = @"
            SELECT DISTINCT 
                T.id_municipio AS Codigo,
                M.nome_municipio AS Nome 
              FROM tabelaanp T 
              JOIN municipio M
                ON T.id_municipio = M.id_municipio
            WHERE T.ano IN @Ano
              AND M.nome_municipio = @NomeMunicipio";

        public Municipio ObterMunicipio(string nomeMunicipio)
        {
            
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NomeMunicipio", nomeMunicipio, DbType.AnsiString);

            return Obter(SQL_BUSCAR_MUNICIPIO, parametros);
        }

        public Municipio InserirMunicipio(string nomeMunicipio)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NomeMunicipio", nomeMunicipio, DbType.AnsiString);

            Execute(SQL_INSERIR_MUNICIPIO, parametros);

            return Obter(SQL_BUSCAR_MUNICIPIO, parametros);
        }

        public Municipio ObterMunicipioAnpPorNomeAno(List<int> anosReferentes, string nomeMunicipio)
        {
            DynamicParameters parametros = new DynamicParameters();

            string listaAnos = SqlUtil.FormatarListaParametrosInteiros(anosReferentes);
            string sql = SQL_OBTER_MUNICIPIO_ANP_POR_ANO_E_NOME.Replace("@Ano", listaAnos);

            parametros.Add("@NomeMunicipio", nomeMunicipio, DbType.AnsiString);

            return Obter(sql, parametros);
        }
    }
}
