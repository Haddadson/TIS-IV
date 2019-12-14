using Dapper;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MPMG.Repositories
{
    public class MesesANPRepo : RepositorioBase<MesANP>
    {
        private const string SQL_LISTAR_MESES_DISPONIVEIS_ANP = @"
                SELECT DISTINCT mesreferente AS mes, anoreferente AS ano
	              FROM `municipioreferente` M
	              JOIN `tabelausuario` TA
   	                ON M.id_municipio_referente = TA.id_municipio_referente
                 WHERE TA.sgdp = @SGDP 
                   AND M.anoreferente = @Ano
                 ORDER BY ano ASC, LENGTH(mesreferente), mesreferente ASC
		
        ";

        public List<MesANP> ListarMesesANPPorSGDP(string sgdp, int ano)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@SGDP", sgdp, DbType.AnsiString);
            parametros.Add("@Ano", ano, DbType.Int32);

            return Listar(SQL_LISTAR_MESES_DISPONIVEIS_ANP, parametros);
        }

    }
}
