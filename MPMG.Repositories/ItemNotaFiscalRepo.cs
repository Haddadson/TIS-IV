using Dapper;
using MPMG.Repositories.Entidades;
using System.Data;

namespace MPMG.Repositories
{
    public class ItemNotaFiscalRepo : RepositorioBase<ItemNotaFiscal>
    {
        private const string SQL_INSERIR_ITEM_NOTA_FISCAL = @"
        INSERT INTO `itemnotafiscal` (sgdp, nr_nota_fiscal, id_item_nota_fiscal, produto, quantidade, vunitario, vrtotal)
        VALUES (
        @Sgdp,
        @NumeroNotaFiscal,
        (SELECT IFNULL(MAX(A.id_item_nota_fiscal)+1, 0) FROM itemnotafiscal A WHERE A.sgdp = @Sgdp AND A.nr_nota_fiscal = @NumeroNotaFiscal),
        @produto,
        @quantidade,
        @valorUnitario,
        @valorTotal)";

        public bool Cadastrar(
            int nrNotaFiscal, 
            int sgdp, 
            string produto,
            int quantidade,
            double valorTotal, 
            double valorUnitario)
        { 
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NumeroNotaFiscal", nrNotaFiscal, DbType.Int32);
            parametros.Add("@Sgdp", sgdp, DbType.Int32);
            parametros.Add("@valorTotal", valorTotal, DbType.Double);
            parametros.Add("@valorUnitario", valorUnitario, DbType.Double);
            parametros.Add("@quantidade", quantidade, DbType.Int32);
            parametros.Add("@produto", produto, DbType.AnsiString);

            return Execute(SQL_INSERIR_ITEM_NOTA_FISCAL, parametros) > 0;
        }
    }
}
