using Dapper;
using MPMG.Repositories.Entidades;
using System;
using System.Data;

namespace MPMG.Repositories
{
    public class NotaFiscalRepo : RepositorioBase<NotaFiscal>
    {
        private const string SQL_INSERIR_NOTA_FISCAL = @"
        INSERT INTO `notafiscal`
        (`sgdp`, `id_dpto`, `mes_fam`, `ano_fam`, `dt_emissao`, `vrtotal`, `preco_medio_revenda`, `preco_maximo_revenda`, 
            `dt_consulta_anp`, `id_tipo_combustivel`,  `quantidade`, `preco_unitario`, `nro_folha`, `veiculo`, `placa_veiculo`, `nr_nota_fiscal`)
        VALUES (
        @sGDP,
        @departamento,
        @mesFAM,
        @anoFAM,
        @dataEmissao,
        @valorTotal,
        @precoMedio,
        @precoMaximo,
        @dataConsultaANP,
        @combustivel,
        @quantidade,
        @precoUnitario,
        @numeroFolha,
        @veiculo,
        @placaVeiculo,
        @nrNotaFiscal)";

        public bool Cadastrar(
            int nrNotaFiscal, 
            int sGDP, 
            double valorTotal, 
            string chaveAcesso, 
            DateTime dataEmissao, 
            double precoMaximo, 
            double precoMedio, 
            DateTime dataConsultaANP, 
            string veiculo, 
            string placaVeiculo, 
            string combustivel, 
            int quantidade, 
            double precoUnitario, 
            int numeroFolha, 
            int departamento,
            int mesFAM,
            int anoFAM) 
        { 
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@nrNotaFiscal", nrNotaFiscal, DbType.Int32);
            parametros.Add("@sGDP", sGDP, DbType.Int32);
            parametros.Add("@valorTotal", valorTotal, DbType.Double);
            parametros.Add("@chaveAcesso", chaveAcesso, DbType.AnsiString);
            parametros.Add("@dataEmissao", dataEmissao, DbType.DateTime);
            parametros.Add("@precoMaximo", precoMaximo, DbType.Double);
            parametros.Add("@precoMedio", precoMedio, DbType.Double);
            parametros.Add("@dataConsultaANP", dataConsultaANP, DbType.DateTime);
            parametros.Add("@veiculo", veiculo, DbType.AnsiString);
            parametros.Add("@placaVeiculo", placaVeiculo, DbType.AnsiString);
            parametros.Add("@combustivel", combustivel, DbType.AnsiString);
            parametros.Add("@quantidade", quantidade, DbType.Int32);
            parametros.Add("@precoUnitario", precoUnitario, DbType.Double);
            parametros.Add("@numeroFolha", numeroFolha, DbType.Int32);
            parametros.Add("@departamento", departamento, DbType.Int32);
            parametros.Add("@mesFAM", mesFAM, DbType.Int32);
            parametros.Add("@anoFAM", anoFAM, DbType.Int32);

            return Execute(SQL_INSERIR_NOTA_FISCAL, parametros) > 0;
        }
    }
}
