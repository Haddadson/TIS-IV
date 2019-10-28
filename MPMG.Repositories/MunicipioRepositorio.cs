using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MPMG.Repositories.Entidades;

namespace MPMG.Repositories
{
    public class MunicipioRepositorio : RepositorioBase<Municipio>
    {

        private const string SQL_INSERIR_MUNICIPIO = @"
            INSERT INTO municipio (nome_municipio) VALUES (@IdMunicipio)
        ";

        private const string SQL_BUSCAR_MUNICIPIO = @"
            SELECT id_municipio AS Codigo, nome_municipio AS Nome FROM municipio WHERE nome_municipio = @IdMunicipio
        ";

        public int buscarOuCriarMunicipio(string idMunicipio)
        {
            int id = -1;
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@IdMunicipio", idMunicipio, DbType.String);
            
            Municipio municipio = Obter(SQL_BUSCAR_MUNICIPIO, parametros);

            if (municipio == null)
            {
                DynamicParameters parametrosInsert = new DynamicParameters();

                parametrosInsert.Add("@IdMunicipio", idMunicipio, DbType.String);

                Execute(SQL_INSERIR_MUNICIPIO, parametros);

                municipio = Obter(SQL_BUSCAR_MUNICIPIO, parametros);
            }
                
            id = municipio.Codigo;
            
            if (id == -1)
            {
                throw new Exception("Não foi possível buscar ou inserir o município.");
            }

            return id;
        }
    }
}
