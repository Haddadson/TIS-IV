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
        (`sgdp`, `id_dpto`, `mes_fam`, `ano_fam`, `dt_emissao`, `vrtotal_nota`,  
         `dt_consulta_anp`, `nro_folha`, `veiculo`, `placa_veiculo`, `nr_nota_fiscal`, `id_upload_fam`,
        `preco_medio_revenda`, `preco_maximo_revenda`)
        VALUES (
        @sGDP,
        @departamento,
        @mesFAM,
        @anoFAM,
        @dataEmissao,
        @valorTotal,
        @dataConsultaANP,
        @numeroFolha,
        @veiculo,
        @placaVeiculo,
        @nrNotaFiscal,
        (SELECT IFNULL(MAX(A.id_upload), 0) FROM uploadtabelafam A),
        '0.0',
        '0.0')";

        public bool Cadastrar(
            string nrNotaFiscal,
            string sGDP, 
            double valorTotal, 
            string chaveAcesso, 
            DateTime dataEmissao, 
            DateTime dataConsultaANP, 
            string veiculo, 
            string placaVeiculo, 
            int numeroFolha, 
            int departamento,
            int mesFAM,
            int anoFAM) 
        { 
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@nrNotaFiscal", nrNotaFiscal, DbType.AnsiString);
            parametros.Add("@sGDP", sGDP, DbType.AnsiString);
            parametros.Add("@valorTotal", valorTotal, DbType.Double);
            parametros.Add("@chaveAcesso", chaveAcesso, DbType.AnsiString);
            parametros.Add("@dataEmissao", dataEmissao, DbType.DateTime);
            parametros.Add("@dataConsultaANP", dataConsultaANP, DbType.DateTime);
            parametros.Add("@veiculo", veiculo, DbType.AnsiString);
            parametros.Add("@placaVeiculo", placaVeiculo, DbType.AnsiString);
            parametros.Add("@numeroFolha", numeroFolha, DbType.Int32);
            parametros.Add("@departamento", departamento, DbType.Int32);
            parametros.Add("@mesFAM", mesFAM, DbType.Int32);
            parametros.Add("@anoFAM", anoFAM, DbType.Int32);

            return Execute(SQL_INSERIR_NOTA_FISCAL, parametros) > 0;
        }
    }
}
