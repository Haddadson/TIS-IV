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

            if (ListaTabelaAnpxNota == null)
                ListaTabelaAnpxNota = new List<AnpxNotaFiscalModelDto>();

            if (ListaCuponsFiscais == null)
                ListaCuponsFiscais = new List<CupomFiscalDto>();

            if (ListaOutrasInformacoes == null)
                ListaOutrasInformacoes = new List<OutrasInformacoesModelDto>();

            if (DadosTabela != null)
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
            PreencherCabecalhoDaTabelaAba1(dadosTabela);
            PreencherGridAba1(listaTabelaAnpxNota);

            PreencherCabecalhoDaTabelaAba2(dadosTabela);
            PreencherGridAba2(listaCuponsFiscais);

            PreencherCabecalhoDaTabelaAba3(dadosTabela);
            PreencherGridAba3(listaOutrasInformacoes);
        }

        #region Aba 1

        private void PreencherCabecalhoDaTabelaAba1(DadosTabelaDto dadosTabela)
        {
            int coluna = POSICAO_PRIMEIRA_COLUNA;

            ManipuladorPlanilha.CriarCelulaMerge(0, 0, 0, 17, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTitulo(0, 0, dadosTabela.Titulo1, ABA_1, false);

            ManipuladorPlanilha.PreencherCelulaCabecalho(2, 0, "SGDP", ABA_1, false);
            ManipuladorPlanilha.PreencherCelulaTexto(2, 1, dadosTabela.Sgdp, ABA_1, false);

            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 3, 4, ABA_1);
            ManipuladorPlanilha.PreencherCelulaCabecalho(2, 3, "Município(s):", ABA_1, false);
            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 5, 9, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTexto(2, 5, dadosTabela.Municipio, ABA_1, false);

            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 11, 12, ABA_1);
            ManipuladorPlanilha.PreencherCelulaCabecalho(2, 11, "Município Ref. ANP:", ABA_1, false);
            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 13, 16, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTexto(2, 13, dadosTabela.MunicipioReferente, ABA_1, false);

            ManipuladorPlanilha.PreencherCelulaCabecalho(4, 0, "Analista", ABA_1, false);
            ManipuladorPlanilha.CriarCelulaMerge(4, 4, 1, 4, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTexto(4, 1, dadosTabela.AnalistaResponsavel, ABA_1, false);

            ManipuladorPlanilha.CriarCelulaMerge(4, 4, 7, 9, ABA_1);
            ManipuladorPlanilha.PreencherCelulaCabecalho(4, 7, "Data de geração da Tabela:", ABA_1, false);
            ManipuladorPlanilha.CriarCelulaMerge(4, 4, 10, 11, ABA_1);
            ManipuladorPlanilha.PreencherCelulaData(4, 10, dadosTabela.DataGeracao, ABA_1, false);

            ManipuladorPlanilha.CriarCelulaMerge(4, 4, 13, 14, ABA_1);
            ManipuladorPlanilha.PreencherCelulaCabecalho(4, 13, "Ano(s) referente(s):", ABA_1, false);
            ManipuladorPlanilha.CriarCelulaMerge(4, 4, 15, 16, ABA_1);
            ManipuladorPlanilha.PreencherCelulaTexto(4, 15, dadosTabela.AnosReferentes, ABA_1, false);

            PreencherCelulaDoCabecalho(6, coluna++, 15, "DATA", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 25, "NOTA FISCAL", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 20, "COMBUSTÍVEL", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 11, "QTDE.", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 18, "VALOR UNIT.", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 20, "VALOR TOTAL", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 10, "VALOR TOTAL DA NOTA", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 10, "NUM. FOLHA", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 11, $"FAM {dadosTabela.MesFam}/{dadosTabela.AnoFam}", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "PREÇO MED ANP", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "DIFERENÇA MED UNIT.", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "DIFERENÇA MED TOTAL", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 15, "VALOR MED ATUALIZADO", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "PREÇO MAX ANP", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "DIFERENÇA MAX UNIT.", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "DIFERENÇA MAX TOTAL", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 14, "VALOR MAX ATUALIZADO", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 12, "CUPONS FISCAIS VINCULADOS", ABA_1);
            PreencherCelulaDoCabecalho(6, coluna++, 25, "MES/ANO PROCURADOS NA ANP", ABA_1);

            ManipuladorPlanilha.DefinirAlturaDaLinha(6, 45, ABA_1);
        }

        private void PreencherGridAba1(List<AnpxNotaFiscalModelDto> listaTabelaAnpxNota)
        {
            int linha = 7;
            listaTabelaAnpxNota.ForEach(item => PreencherLinhaDoGridAba1(linha++, POSICAO_PRIMEIRA_COLUNA, item));
        }

        private void PreencherLinhaDoGridAba1(int linha, int coluna, AnpxNotaFiscalModelDto item)
        {
            ManipuladorPlanilha.PreencherCelulaData(linha, coluna++, item.DataGeracao, ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.NumeroNotaFiscal, ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.Produto, ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaNumerica(linha, coluna++, item.Quantidade, ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaNumerica(linha, coluna++, item.ValorUnitario, ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaNumerica(linha, coluna++, item.ValorTotalItem, ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaNumerica(linha, coluna++, item.ValorTotalNota, ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaValorInteiro(linha, coluna++, item.NumeroFolha, ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.ValorFam.ToString("0.0000000"), ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.PrecoMedioAnp.ToString("0.00"), ABA_1, true);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.DiferencaMediaUnitaria.Replace(',', '.'), ABA_1, true);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.DiferencaMediaTotal.Replace(',', '.'), ABA_1, true,
                double.Parse(item.DiferencaMediaUnitaria.Replace(',', '.').Replace('-', '0')) > 0 ? IndexedColors.LightYellow : null);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.ValorMedioAtualizado.Replace(',', '.'), ABA_1, true,
                double.Parse(item.DiferencaMediaUnitaria.Replace(',', '.').Replace('-', '0')) > 0 ? IndexedColors.LightYellow : null);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.PrecoMaximoAnp.ToString("0.00"), ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.DiferencaMaximaUnitaria.Replace(',', '.'), ABA_1, true);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.DiferencaMaximaTotal.Replace(',', '.'), ABA_1, true,
                double.Parse(item.DiferencaMaximaUnitaria.Replace(',', '.').Replace('-', '0')) > 0 ? IndexedColors.Red : null);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.ValorMaximoAtualizado.Replace(',', '.'), ABA_1, true,
                double.Parse(item.DiferencaMaximaUnitaria.Replace(',', '.').Replace('-', '0')) > 0 ? IndexedColors.Red : null);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, string.Join(";", item.CuponsFiscaisVinculados), ABA_1, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, string.Format("{0}/{1}", item.MesAnp, item.AnoAnp), ABA_1, true);

            ManipuladorPlanilha.DefinirAlturaDaLinha(linha, 15, ABA_1);
        }

        #endregion

        #region Aba 2

        private void PreencherCabecalhoDaTabelaAba2(DadosTabelaDto dadosTabela)
        {
            int coluna = POSICAO_PRIMEIRA_COLUNA;

            ManipuladorPlanilha.CriarCelulaMerge(0, 0, 0, 10, ABA_2);
            ManipuladorPlanilha.PreencherCelulaTitulo(0, 0, dadosTabela.Titulo2, ABA_2, false);

            ManipuladorPlanilha.PreencherCelulaCabecalho(2, 0, "SGDP", ABA_2, false);
            ManipuladorPlanilha.PreencherCelulaTexto(2, 1, dadosTabela.Sgdp, ABA_2, false);

            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 3, 4, ABA_2);
            ManipuladorPlanilha.PreencherCelulaCabecalho(2, 3, "Município(s):", ABA_2, false);
            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 5, 8, ABA_2);
            ManipuladorPlanilha.PreencherCelulaTexto(2, 5, dadosTabela.Municipio, ABA_2, false);

            PreencherCelulaDoCabecalho(5, coluna++, 25, "NUM. NF REFERENTE", ABA_2);
            PreencherCelulaDoCabecalho(5, coluna++, 15, "DATA", ABA_2);
            PreencherCelulaDoCabecalho(5, coluna++, 20, "HORÁRIO", ABA_2);
            PreencherCelulaDoCabecalho(5, coluna++, 25, "COMBUSTÍVEL", ABA_2);
            PreencherCelulaDoCabecalho(5, coluna++, 15, "COO", ABA_2);
            PreencherCelulaDoCabecalho(5, coluna++, 15, "QUANT.", ABA_2);
            PreencherCelulaDoCabecalho(5, coluna++, 20, "PREÇO UNIT.", ABA_2);
            PreencherCelulaDoCabecalho(5, coluna++, 15, "VALOR TOTAL", ABA_2);
            PreencherCelulaDoCabecalho(5, coluna++, 15, "VEÍCULO", ABA_2);
            PreencherCelulaDoCabecalho(5, coluna++, 15, "PLACA VEÍCULO", ABA_2);
            PreencherCelulaDoCabecalho(5, coluna++, 25, "HODÔMETRO", ABA_2);

            ManipuladorPlanilha.DefinirAlturaDaLinha(5, 45, ABA_2);
        }

        private void PreencherGridAba2(List<CupomFiscalDto> listaCupons)
        {
            int linha = 6;
            listaCupons.ForEach(item => PreencherLinhaDoGridAba2(linha++, POSICAO_PRIMEIRA_COLUNA, item));
        }

        private void PreencherLinhaDoGridAba2(int linha, int coluna, CupomFiscalDto item)
        {
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.NumeroNotaFiscal, ABA_2, true);
            ManipuladorPlanilha.PreencherCelulaData(linha, coluna++, item.DataEmissao.Date, ABA_2, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.DataEmissao.ToShortTimeString(), ABA_2, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.Coo, ABA_2, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.Produto, ABA_2, true);
            ManipuladorPlanilha.PreencherCelulaNumerica(linha, coluna++, item.Quantidade, ABA_2, true);
            ManipuladorPlanilha.PreencherCelulaNumerica(linha, coluna++, item.PrecoUnitario, ABA_2, true);
            ManipuladorPlanilha.PreencherCelulaNumerica(linha, coluna++, item.ValorTotal, ABA_2, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.Veiculo, ABA_2, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.PlacaVeiculo, ABA_2, true);
            ManipuladorPlanilha.PreencherCelulaNumerica(linha, coluna++, item.Hodometro, ABA_2, true);

            ManipuladorPlanilha.DefinirAlturaDaLinha(linha, 15, ABA_2);
        }

        #endregion

        #region Aba 3

        private void PreencherCabecalhoDaTabelaAba3(DadosTabelaDto dadosTabela)
        {
            int coluna = POSICAO_PRIMEIRA_COLUNA;

            ManipuladorPlanilha.CriarCelulaMerge(0, 0, 0, 10, ABA_3);
            ManipuladorPlanilha.PreencherCelulaTitulo(0, 0, dadosTabela.Titulo3, ABA_3, false);

            ManipuladorPlanilha.PreencherCelulaCabecalho(2, 0, "SGDP", ABA_3, false);
            ManipuladorPlanilha.PreencherCelulaTexto(2, 1, dadosTabela.Sgdp, ABA_3, false);

            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 4, 5, ABA_3);
            ManipuladorPlanilha.PreencherCelulaCabecalho(2, 4, "Município(s):", ABA_3, false);
            ManipuladorPlanilha.CriarCelulaMerge(2, 2, 6, 9, ABA_3);
            ManipuladorPlanilha.PreencherCelulaTexto(2, 6, dadosTabela.Municipio, ABA_3, false);

            PreencherCelulaDoCabecalho(5, coluna++, 20, "NUM. NF REFERENTE", ABA_3);
            PreencherCelulaDoCabecalho(5, coluna++, 25, "COOs VINCULADOS", ABA_3);
            PreencherCelulaDoCabecalho(5, coluna++, 25, "DEPARTAMENTO", ABA_3);
            PreencherCelulaDoCabecalho(5, coluna++, 15, "VEÍCULO", ABA_3);
            PreencherCelulaDoCabecalho(5, coluna++, 15, "PLACA VEÍCULO", ABA_3);
            PreencherCelulaDoCabecalho(5, coluna++, 20, "COMBUSTÍVEL", ABA_3);
            PreencherCelulaDoCabecalho(5, coluna++, 25, "VALOR TOTAL CUPOM FISCAL", ABA_3);
            PreencherCelulaDoCabecalho(5, coluna++, 25, "DIFERENÇA  MED TOTAL NF", ABA_3);
            PreencherCelulaDoCabecalho(5, coluna++, 25, "VALOR MED ATUALIZADO NF", ABA_3);
            PreencherCelulaDoCabecalho(5, coluna++, 25, "DIFERENÇA MAX TOTAL NF", ABA_3);
            PreencherCelulaDoCabecalho(5, coluna++, 25, "VALOR MAX ATUALIZADO NF", ABA_3);

            ManipuladorPlanilha.DefinirAlturaDaLinha(5, 45, ABA_3);
        }

        private void PreencherGridAba3(List<OutrasInformacoesModelDto> listaOutrasInfos)
        {
            int linha = 6;
            listaOutrasInfos.ForEach(item => PreencherLinhaDoGridAba3(linha++, POSICAO_PRIMEIRA_COLUNA, item));
        }

        private void PreencherLinhaDoGridAba3(int linha, int coluna, OutrasInformacoesModelDto item)
        {
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.NumeroNotaFiscal.ToString(), ABA_3, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, string.Join(";", item.CuponsFiscaisVinculados), ABA_3, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.NomeDepartamento, ABA_3, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.Veiculo, ABA_3, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.PlacaVeiculo, ABA_3, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.Produto, ABA_3, true);
            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.ValorTotalNota.ToString("0.00"), ABA_3, true);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.DiferencaMediaTotal.Replace('.', ','), ABA_3, true,
                double.Parse(item.DiferencaMediaTotal.Replace(',', '.').Replace('-', '0')) > 0 ? IndexedColors.LightYellow : null);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.ValorMedioAtualizado.Replace(',', '.'), ABA_3, true,
                double.Parse(item.DiferencaMediaTotal.Replace(',', '.').Replace('-', '0')) > 0 ? IndexedColors.LightYellow : null);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.DiferencaMaximaTotal.Replace(',', '.'), ABA_3, true,
                double.Parse(item.DiferencaMaximaTotal.Replace(',', '.').Replace('-', '0')) > 0 ? IndexedColors.Red : null);

            ManipuladorPlanilha.PreencherCelulaTexto(linha, coluna++, item.ValorMaximoAtualizado.Replace(',', '.'), ABA_3, true,
                double.Parse(item.DiferencaMaximaTotal.Replace(',', '.').Replace('-', '0')) > 0 ? IndexedColors.Red : null);

            ManipuladorPlanilha.DefinirAlturaDaLinha(linha, 15, ABA_3);
        }

        #endregion


        private void PreencherCelulaDoCabecalho(int linha, int coluna, int tamanho, string conteudo, string nomeAba)
        {
            ManipuladorPlanilha.PreencherCelulaCabecalho(linha, coluna, conteudo, nomeAba, true, null, new XSSFColor(new byte[] { 195, 218, 242 }));
            ManipuladorPlanilha.DefinirLarguraDaColuna(coluna, tamanho, nomeAba);
        }

        private MemoryStream ObterArquivoExcel()
        {
            return ManipuladorPlanilha.ObterVetorDeBytes();
        }

    }
}
