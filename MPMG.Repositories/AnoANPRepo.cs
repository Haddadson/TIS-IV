using Dapper;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MPMG.Repositories
{
    public class AnoANPRepo : RepositorioBase<AnoANP>
    {
        private readonly MesesANPRepo mesesANPRepo;
        public AnoANPRepo()
        {
            mesesANPRepo = new MesesANPRepo();
        }

        private const string SQL_LISTAR_ANOS_DISPONIVEIS_ANP = @"
            SELECT DISTINCT anoreferente AS ano
	          FROM `municipioreferente` M
	          JOIN `tabelausuario` TA
   	            ON M.id_municipio_referente = TA.id_municipio_referente
             WHERE TA.sgdp = @SGDP
        ";

        public List<AnoANP> ListarAnoseMesesANPPorSgdp(string sgdp)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@SGDP", sgdp, DbType.AnsiString);

            List<AnoANP> anosANP = Listar(SQL_LISTAR_ANOS_DISPONIVEIS_ANP, parametros);

            foreach (var anoANP in anosANP)
            {
                anoANP.meses = mesesANPRepo.ListarMesesANPPorSGDP(sgdp, anoANP.ano);
            }
            return anosANP;
        }
    }
}
