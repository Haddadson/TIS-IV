using Dapper;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MPMG.Repositories
{
    public class DepartamentoRepo : RepositorioBase<Departamento>
    {

        private const string SQL_BUSCAR_MUNICIPIO = @"
            SELECT id_dpto AS Id, 
                   nome_dpto AS Nome 
            FROM departamento 
            ORDER BY 1";

        
        public List<object> ObterDepartamento()
        {
            DynamicParameters parametros = new DynamicParameters();
            return Listar(SQL_BUSCAR_MUNICIPIO, parametros);
        }
    }
}
