using Dapper;
using MPMG.Repositories.Entidades;
using System.Collections.Generic;
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

        private const string SQL_BUSCAR_PRECOS = @"
            SELECT DISTINCT vunitario AS ValorUnitario
              FROM  itemnotafiscal 
             WHERE SGDP = @SGDP
               AND vunitario IS NOT NULL
        ";

        public bool Cadastrar(
            string nrNotaFiscal, 
            string sgdp, 
            string produto,
            double quantidade,
            double valorTotal, 
            double valorUnitario)
        { 
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NumeroNotaFiscal", nrNotaFiscal, DbType.AnsiString);
            parametros.Add("@Sgdp", sgdp, DbType.AnsiString);
            parametros.Add("@valorTotal", valorTotal, DbType.Double);
            parametros.Add("@valorUnitario", valorUnitario, DbType.Double);
            parametros.Add("@quantidade", quantidade, DbType.Double);
            parametros.Add("@produto", produto, DbType.AnsiString);

            return Execute(SQL_INSERIR_ITEM_NOTA_FISCAL, parametros) > 0;
        }

        public List<ItemNotaFiscal> buscarPrecos(string valorSgdp)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@SGDP", valorSgdp, DbType.AnsiString);

            return Listar(SQL_BUSCAR_PRECOS, parametros);
        }
    }
}
