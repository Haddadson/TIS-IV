using Dapper;
using MPMG.Repositories.Entidades;
using System;
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

        public int BuscarOuCriarMunicipio(string nomeMunicipio)
        {
            int id = -1;
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NomeMunicipio", nomeMunicipio, DbType.String);

            Municipio municipio = Obter(SQL_BUSCAR_MUNICIPIO, parametros);

            if (municipio == null)
            {
                DynamicParameters parametrosInsert = new DynamicParameters();

                parametrosInsert.Add("@NomeMunicipio", nomeMunicipio, DbType.String);

                Execute(SQL_INSERIR_MUNICIPIO, parametros);

                municipio = Obter(SQL_BUSCAR_MUNICIPIO, parametros);
            }

            id = municipio?.Codigo ?? -1;

            if (id == -1)
            {
                throw new Exception("Não foi possível buscar ou inserir o município.");
            }

            return id;
        }
    }
}
