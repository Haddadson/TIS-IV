using Dapper;
using MPMG.Repositories;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPMG.Services
{
    public class CupomFiscalRepo : RepositorioBase<CupomFiscal>
    {
        private const string SQL_INSERIR_CUPOM_FISCAL = @"
                INSERT INTO `cupomfiscal` (`coo`, `sgdp`, `nr_nota_fiscal`)
                VALUES (@nr_cupom, @sGDP, @numNotaFiscal)";

        private const string SQL_INSERIR_CUPOM_FISCAL_COMPLETO = @"
                INSERT INTO `cupomfiscal` (`coo`, `sgdp`, `nr_nota_fiscal`, `posto_referente`, `hodometro`, 
                `cliente`, `dt_emissao`, `quantidade`, `preco_unitario`, `vrtotal`, `produto`, `veiculo`, `placa_veiculo`)
                VALUES (@coo, @sGDP, (SELECT `nr_nota_fiscal` FROM `notafiscal` WHERE `nr_nota_fiscal` = @nrNotaFiscal), 
                @posto,
                @hodometro,
                @cliente,
                @data,
                @quantidade,
                @precoUnitario,
                @valorTotal,
                @produto,
                @veiculo,
                @placaVeiculo)";

        private const string SQL_LISTAR_CUPONS_FISCAIS_POR_SGDP = @"
            SELECT
                coo AS Coo,
                sgdp AS Sgdp,
                nr_nota_fiscal AS NumeroNotaFiscal,
                posto_referente AS PostoReferente,
                hodometro AS Hodometro,
                cliente AS Cliente,
                dt_emissao AS DataEmissao,
                quantidade AS Quantidade,
                preco_unitario AS PrecoUnitario,
                vrtotal AS ValorTotal,
                produto AS Produto,
                veiculo AS Veiculo,
                placa_veiculo AS PlacaVeiculo
            FROM `cupomfiscal`
            WHERE sgdp = @Sgdp";

        private const string SQL_LISTAR_CUPONS_FISCAIS_POR_NOTA = @"
            SELECT 
                coo AS Coo
            FROM `cupomfiscal`
            WHERE sgdp = @Sgdp 
            AND nr_nota_fiscal = @NumeroNotaFiscal";

        public bool CadastrarCupom(int sGDP, string numeroCupom, int numeroNota)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@nr_cupom", numeroCupom, DbType.AnsiString);
            parametros.Add("@sGDP", sGDP, DbType.Int32);
            parametros.Add("@numNotaFiscal", numeroCupom, DbType.Int32);

            return Execute(SQL_INSERIR_CUPOM_FISCAL, parametros) > 0;
        }

        public bool CadastrarCupomCompleto(int sGDP, int nrNotaFiscal, string cOO, 
            string posto, DateTime data, 
            string combustivel, int quantidade, double precoUnitario, 
            double valorTotal, string cliente, int hodometro, string veiculo, string placaVeiculo)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@sGDP", sGDP, DbType.Int32); 
            parametros.Add("@nrNotaFiscal", nrNotaFiscal, DbType.Int32); 
            parametros.Add("@coo", cOO,  DbType.AnsiString); 
            parametros.Add("@posto",posto,  DbType.AnsiString); 
            parametros.Add("@data", data,  DbType.DateTime); 
            parametros.Add("@produto",combustivel, DbType.AnsiString); 
            parametros.Add("@quantidade",quantidade, DbType.Int32); 
            parametros.Add("@precoUnitario",precoUnitario, DbType.Double); 
            parametros.Add("@valorTotal", valorTotal, DbType.Double); 
            parametros.Add("@cliente", cliente, DbType.AnsiString); 
            parametros.Add("@hodometro", hodometro,  DbType.Int32); 
            parametros.Add("@veiculo", veiculo, DbType.AnsiString); 
            parametros.Add("@placaVeiculo", veiculo, DbType.AnsiString); 

            return Execute(SQL_INSERIR_CUPOM_FISCAL_COMPLETO, parametros) > 0;
        }

        public List<string> ObterCuponsVinculados(int Sgdp, int numeroNota)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NumeroNotaFiscal", numeroNota, DbType.Int32);
            parametros.Add("@Sgdp", Sgdp, DbType.Int32);

            return Listar(SQL_LISTAR_CUPONS_FISCAIS_POR_NOTA, parametros).Select(item => item.Coo).ToList();
        }

        public List<CupomFiscal> ListarCuponsPorSgdp(int Sgdp)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Sgdp", Sgdp, DbType.Int32);

            return Listar(SQL_LISTAR_CUPONS_FISCAIS_POR_SGDP, parametros);
        }
    }
}