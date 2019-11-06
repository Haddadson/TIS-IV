using Dapper;
using MPMG.Repositories;
using System.Data;

namespace MPMG.Services
{
    public class CupomFiscalRepo : RepositorioBase<CupomFiscal>
    {
        private const string SQL_INSERIR_NOTA_FISCAL = @"
                INSERT INTO `cupomfiscal` (`coo`, `sgdp`, `id_nota_fiscal`)
                VALUES (@nr_cupom, @sGDP, (SELECT MAX(`id_nota_fiscal`) FROM `notafiscal`))";
        public bool CadastrarCupom(int sGDP, string numeroCupom)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@nr_cupom", numeroCupom, DbType.AnsiString);
            parametros.Add("@sGDP", sGDP, DbType.Int32);

            return Execute(SQL_INSERIR_NOTA_FISCAL, parametros) > 0;
        }
    }
}