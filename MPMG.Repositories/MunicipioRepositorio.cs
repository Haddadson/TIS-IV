using Dapper;
using MPMG.Repositories.Entidades;
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
                M.id_municipio AS Codigo,
                M.nome_municipio AS Nome 
              FROM TabelaANP T 
              JOIN municipio M 
            WHERE T.ano = @Ano 
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

        public Municipio ObterMunicipioAnpPorNomeAno(int anoReferente, string nomeMunicipio)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Ano", anoReferente.ToString(), DbType.AnsiStringFixedLength);
            parametros.Add("@NomeMunicipio", nomeMunicipio, DbType.AnsiString);

            return Obter(SQL_OBTER_MUNICIPIO_ANP_POR_ANO_E_NOME, parametros);
        }
    }
}
