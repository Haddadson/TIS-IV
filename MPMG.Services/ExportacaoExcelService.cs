using MPMG.Interfaces.DTO;
using MPMG.Util.Excel;
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


        public MemoryStream ExportarDadosParaExcel(DadosTabelaDto DadosTabela,
                                                   List<AnpxNotaFiscalModelDto> ListaTabelaAnpxNota,
                                                   List<CupomFiscalDto> ListaCuponsFiscais,
                                                   List<OutrasInformacoesModelDto> ListaOutrasInformacoes)
        {
            MemoryStream stream;

            if(DadosAnpDto != null && ListaCuponsFiscais.Any() && ListaTabelaAnpxNota.Any() && ListaOutrasInformacoes.Any())
            {
                CriarExcel(requisicao.ListaDocumentosParaExportacao);
                stream = ObterArquivoExcel();

            }


            return resultado;

        }

        private void CriarExcel(DadosTabelaDto DadosTabela,
                                List<AnpxNotaFiscalModelDto> ListaTabelaAnpxNota,
                                List<CupomFiscalDto> ListaCuponsFiscais,
                                List<OutrasInformacoesModelDto> ListaOutrasInformacoes)
        {
            PreencherCabecalhoDaTabelaAba1();
            PreencherGrid();
        }

        private void PreencherCabecalhoDaTabelaAba1()
        {
            int coluna = POSICAO_PRIMEIRA_COLUNA;

            ManipuladorPlanilha.CriarCelulaMerge(0, 0, 0, 17, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTitulo(0,0, "")


            PreencherCelulaDoCabecalho(coluna++, 15, "Visualizado");
            PreencherCelulaDoCabecalho(coluna++, 25, "Documento");
            PreencherCelulaDoCabecalho(coluna++, 15, "Origem");
            PreencherCelulaDoCabecalho(coluna++, 11, "Contrato");
            PreencherCelulaDoCabecalho(coluna++, 18, "CNPJ");
            PreencherCelulaDoCabecalho(coluna++, 20, "Cliente");
            PreencherCelulaDoCabecalho(coluna++, 10, "Emissão");
            PreencherCelulaDoCabecalho(coluna++, 10, "Publicação");
            PreencherCelulaDoCabecalho(coluna++, 11, "Vencimento");
            PreencherCelulaDoCabecalho(coluna++, 12, "Valor(R$)");
            PreencherCelulaDoCabecalho(coluna, 20, "Observação");

            ManipuladorPlanilha.DefinirAlturaDaLinha(POSICAO_LINHA_CABECALHO, 45, ABA_1);
        }
        private void PreencherCelulaDoCabecalho(int coluna, int tamanho, string conteudo)
        {
            ManipuladorPlanilha.PreencherCelulaCabecalho(POSICAO_LINHA_CABECALHO, coluna, conteudo);
            ManipuladorPlanilha.DefinirLarguraDaColuna(coluna, tamanho);
        }

    }
}
