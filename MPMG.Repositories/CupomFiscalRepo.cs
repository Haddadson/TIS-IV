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

        private const string SQL_LISTAR_CUPONS_DISPONIVEIS_POR_SGDP = @"
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
            WHERE sgdp = @Sgdp
              AND nr_nota_fiscal IS NULL";

        private const string SQL_BUSCAR_CUPOM_DISPONIVEL = @"
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
            WHERE sgdp = @Sgdp
              AND coo = @Coo
              AND nr_nota_fiscal IS NULL";

        private const string SQL_EDITAR_CUPOM = @"
            UPDATE `cupomfiscal`
               SET nr_nota_fiscal = @NrNotaFiscal
             WHERE sgdp = @SGDP
              AND coo = @Coo";

        public bool CadastrarCupom(string sGDP, string numeroCupom, string numeroNota)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@nr_cupom", numeroCupom, DbType.AnsiString);
            parametros.Add("@sGDP", sGDP, DbType.AnsiString);
            parametros.Add("@numNotaFiscal", numeroCupom, DbType.AnsiString);

            return Execute(SQL_INSERIR_CUPOM_FISCAL, parametros) > 0;
        }

        public bool CadastrarCupomCompleto(string sGDP, string nrNotaFiscal, string cOO, 
            string posto, DateTime data, 
            string combustivel, int quantidade, double precoUnitario, 
            double valorTotal, string cliente, int hodometro, string veiculo, string placaVeiculo)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@sGDP", sGDP, DbType.AnsiString); 
            parametros.Add("@nrNotaFiscal", nrNotaFiscal, DbType.AnsiString); 
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

        public List<string> ObterCuponsVinculados(string Sgdp, string numeroNota)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NumeroNotaFiscal", numeroNota, DbType.AnsiString);
            parametros.Add("@Sgdp", Sgdp, DbType.AnsiString);

            return Listar(SQL_LISTAR_CUPONS_FISCAIS_POR_NOTA, parametros).Select(item => item.Coo).ToList();
        }

        public List<CupomFiscal> ListarCuponsPorSgdp(string Sgdp)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Sgdp", Sgdp, DbType.AnsiString);

            return Listar(SQL_LISTAR_CUPONS_FISCAIS_POR_SGDP, parametros);
        }

        public List<CupomFiscal> ListarCuponsDisponiveisPorSgdp (string Sgdp)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Sgdp", Sgdp, DbType.AnsiString);

            return Listar(SQL_LISTAR_CUPONS_DISPONIVEIS_POR_SGDP, parametros);
        }

        public CupomFiscal buscarCupomDisponivel (string sGDP, string numeroCupom)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Sgdp", sGDP, DbType.AnsiString);
            parametros.Add("@Coo", numeroCupom, DbType.AnsiString);

            return Obter(SQL_BUSCAR_CUPOM_DISPONIVEL, parametros);
        }

        public void EditarCupom(string sgdp, string numeroCupom, string numeroNota)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Sgdp", sgdp, DbType.AnsiString);
            parametros.Add("@Coo", numeroCupom, DbType.AnsiString);
            parametros.Add("@NrNotaFiscal", numeroNota, DbType.AnsiString);

            Execute(SQL_EDITAR_CUPOM, parametros);
        }
    }
}