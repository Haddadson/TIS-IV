using MPMG.Interfaces.DTO;
using MPMG.Util.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MPMG.Services
{
    public class ExportacaoExcelService
    {
        private readonly ManipuladorExcelTabelas ManipuladorPlanilha = new ManipuladorExcelTabelas();
        private const int POSICAO_LINHA_CABECALHO = 0;
        private const int POSICAO_PRIMEIRA_COLUNA = 0;
        private const string ABA_1 = "NFs X ANP";
        private const string ABA_2 = "Cupons Fiscais";
        private const string ABA_3 = "Outras informações";


        public byte[] ExportarDadosParaExcel(DadosTabelaDto DadosTabela,
                                                   List<AnpxNotaFiscalModelDto> ListaTabelaAnpxNota,
                                                   List<CupomFiscalDto> ListaCuponsFiscais,
                                                   List<OutrasInformacoesModelDto> ListaOutrasInformacoes)
        {
            MemoryStream stream;

            if (DadosTabela != null && ListaCuponsFiscais.Any() && ListaTabelaAnpxNota.Any() && ListaOutrasInformacoes.Any())
            {
                CriarExcel(DadosTabela, ListaTabelaAnpxNota, ListaCuponsFiscais, ListaOutrasInformacoes);
                stream = ObterArquivoExcel();

            }
            else return null;

            return stream.ToArray();

        }

        private void CriarExcel(DadosTabelaDto dadosTabela,
                                List<AnpxNotaFiscalModelDto> listaTabelaAnpxNota,
                                List<CupomFiscalDto> listaCuponsFiscais,
                                List<OutrasInformacoesModelDto> listaOutrasInformacoes)
        {
            PreencherCabecalhoDaTabelaAba1(dadosTabela, listaTabelaAnpxNota);
            //PreencherGrid();
        }

        private void PreencherCabecalhoDaTabelaAba1(DadosTabelaDto dadosTabela,
                                List<AnpxNotaFiscalModelDto> listaTabelaAnpxNota)
        {
            int coluna = POSICAO_PRIMEIRA_COLUNA;

            ManipuladorPlanilha.CriarCelulaMerge(0, 0, 0, 17, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTitulo(0, 0, dadosTabela.Titulo1, ABA_1);
            
            ManipuladorPlanilha.PreencherCelulaCabecalho(2, 0, "SGDP", ABA_1);
            ManipuladorPlanilha.PreencherCelulaTexto(2, 1, dadosTabela.Sgdp, ABA_1);

            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 3, 4, ABA_1);
            ManipuladorPlanilha.PreencherCelulaCabecalho(2, 3, "Município(s):", ABA_1);
            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 5, 9, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTexto(2, 5, dadosTabela.Municipio, ABA_1);

            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 11, 12, ABA_1);
            ManipuladorPlanilha.PreencherCelulaCabecalho(2, 11, "Município Ref. ANP:", ABA_1);
            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 13, 16, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTexto(2, 13, dadosTabela.MunicipioReferente, ABA_1);

            ManipuladorPlanilha.PreencherCelulaCabecalho(4, 0, "Analista", ABA_1);
            ManipuladorPlanilha.CriarCelulaMerge(4, 4, 1, 4, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTexto(4, 1, dadosTabela.AnalistaResponsavel, ABA_1);

            ManipuladorPlanilha.CriarCelulaMerge(4, 4, 7, 9, ABA_1);
            ManipuladorPlanilha.PreencherCelulaCabecalho(4, 7, "Data de geração da Tabela:", ABA_1);
            ManipuladorPlanilha.CriarCelulaMerge(4, 4, 10, 11, ABA_1);
            ManipuladorPlanilha.PreencherCelulaData(4, 10, dadosTabela.DataGeracao, ABA_1);

            ManipuladorPlanilha.CriarCelulaMerge(4, 4, 13, 14, ABA_1);
            ManipuladorPlanilha.PreencherCelulaCabecalho(4, 13, "Ano(s) referente(s):", ABA_1);
            ManipuladorPlanilha.CriarCelulaMerge(4, 4, 15, 16, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTexto(4, 15, dadosTabela.AnosReferentes, ABA_1);

            PreencherCelulaDoCabecalho(6, coluna++, 15, "DATA", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 25, "NOTA FISCAL", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 15, "COMBUSTÍVEL", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 11, "QTDE.", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 18, "VALOR UNIT.", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 20, "VALOR TOTAL", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 10, "VALOR TOTAL DA NOTA", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 10, "NUM. FOLHA", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 11, $"FAM ANO/MES", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "PREÇO MED ANP", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "DIFERENÇA MED UNIT.", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "DIFERENÇA MED TOTAL", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "VALOR MED ATUALIZADO", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "PREÇO MAX ANP", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "DIFERENÇA MAX UNIT.", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "DIFERENÇA MAX TOTAL", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "VALOR MAX ATUALIZADO", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "CUPONS FISCAIS VINCULADOS", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "MES/ANO PROCURADOS NA ANP", ABA_1);
        }
        
        private void PreencherCelulaDoCabecalho(int linha, int coluna, int tamanho, string conteudo, string nomeAba)
        {
            var estilo = ManipuladorPlanilha.CriarEstilo();

            estilo.SetFillForegroundColor(new XSSFColor(new byte[] { 195, 218, 242 }));
            estilo.FillPattern = FillPattern.SolidForeground;

            ManipuladorPlanilha.PreencherCelulaCabecalho(linha, coluna, conteudo, nomeAba);
            ManipuladorPlanilha.DefinirLarguraDaColuna(coluna, tamanho, nomeAba);
        }

        private MemoryStream ObterArquivoExcel()
        {
            return ManipuladorPlanilha.ObterVetorDeBytes();
        }

    }
}
