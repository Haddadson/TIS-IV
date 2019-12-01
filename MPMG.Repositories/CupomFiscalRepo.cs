using Dapper;
using MPMG.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPMG.Services
{
    public class CupomFiscalRepo : RepositorioBase<CupomFiscal>
    {
        private const string SQL_INSERIR_CUPOM_FISCAL = @"
                INSERT INTO `cupomfiscal` (`coo`, `sgdp`, `id_nota_fiscal`)
                VALUES (@nr_cupom, @sGDP, (SELECT MAX(`id_nota_fiscal`) FROM `notafiscal`))";

        private const string SQL_INSERIR_CUPOM_FISCAL_COMPLETO = @"
                INSERT INTO `cupomfiscal` (`coo`, `sgdp`, `id_nota_fiscal`, `posto_referente`, `hodometro`, 
                `cliente`, `dt_emissao`, `quantidade`, `preco_unitario`, `vr_total`, `id_tipo_combustivel`)
                VALUES (@coo, @sGDP, (SELECT `id_nota_fiscal` FROM `notafiscal` WHERE `nr_nota_fiscal` = @nrNotaFiscal), 
                @posto,
                @hodometro,
                @cliente,
                @data,
                @quantidade,
                @precoUnitario,
                @valorTotal,
                @id_combustivel)";

        private const string SQL_LISTAR_CUPONS_FISCAIS_POR_NOTA = @"
            SELECT coo AS Coo
            FROM `cupomfiscal`
            WHERE sgdp = @Sgdp 
            AND nr_nota_fiscal = @NumeroNotaFiscal";

        public bool CadastrarCupom(int sGDP, string numeroCupom)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@nr_cupom", numeroCupom, DbType.AnsiString);
            parametros.Add("@sGDP", sGDP, DbType.Int32);

            return Execute(SQL_INSERIR_CUPOM_FISCAL, parametros) > 0;
        }

        public bool CadastrarCupomCompleto(int sGDP, int nrNotaFiscal, string cOO, string posto, DateTime data, string combustivel, int quantidade, double precoUnitario, double valorTotal, string cliente, int hodometro, string veiculo, string placaVeiculo)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@sGDP", sGDP, DbType.Int32); 
            parametros.Add("@nrNotaFiscal", nrNotaFiscal, DbType.Int32); 
            parametros.Add("@coo", cOO,  DbType.AnsiString); 
            parametros.Add("@posto",posto,  DbType.AnsiString); 
            parametros.Add("@data", data,  DbType.DateTime); 
            parametros.Add("@id_combustivel",combustivel, DbType.AnsiString); 
            parametros.Add("@quantidade",quantidade, DbType.Int32); 
            parametros.Add("@precoUnitario",precoUnitario, DbType.Double); 
            parametros.Add("@valorTotal", valorTotal, DbType.Double); 
            parametros.Add("@cliente", cliente, DbType.AnsiString); 
            parametros.Add("@hodometro", hodometro,  DbType.Int32); 
            parametros.Add("@veiculo", veiculo, DbType.AnsiString); 

            return Execute(SQL_INSERIR_CUPOM_FISCAL_COMPLETO, parametros) > 0;
        }

        public List<string> ObterCuponsVinculados(int Sgdp, int numeroNota)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NumeroNotaFiscal", numeroNota, DbType.Int32);
            parametros.Add("@Sgdp", Sgdp, DbType.Int32);

            return Listar(SQL_LISTAR_CUPONS_FISCAIS_POR_NOTA, parametros).Select(item => item.Coo).ToList();
        }
    }
}