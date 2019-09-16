using MPMG.Interfaces.DTO;
using MPMG.Util;
using MPMG.Util.Enum;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MPMG.Services
{
    public class ObterPrecoMedioExcelService
    {
        private readonly CultureInfo Culture;

        public ObterPrecoMedioExcelService()
        {
            Culture = CultureInfo.GetCultureInfo("pt-BR");
        }

        public DadosPlanilhaAnpDto ObterInformacoesAnp(string caminhoExcel, DadosNotaFiscalDto dadosNotaFiscal)
        {
            if (string.IsNullOrWhiteSpace(caminhoExcel))
                return null;

            XSSFWorkbook workbookExcel;
            using (FileStream file = new FileStream(string.Format("{0}/{1}", Constantes.CAMINHO_DOWNLOAD_ARQUIVO, caminhoExcel),
                FileMode.Open, FileAccess.Read))
            {
                workbookExcel = new XSSFWorkbook(file);
            }

            ISheet sheet = workbookExcel.GetSheet(workbookExcel.GetSheetName(0));
            bool aposCabecalho = false;
            for (int row = 0; row <= sheet.LastRowNum; row++)
            {
                if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                {
                    if (aposCabecalho || ValidaSeLinhaEhCabecalho(sheet, row))
                    {
                        aposCabecalho = true;
                    }

                    if (aposCabecalho && ValidarLinhaBuscada(dadosNotaFiscal, sheet, row))
                    {
                        return new DadosPlanilhaAnpDto()
                        {
                            Combustivel = sheet.GetRow(row).GetCell(1).StringCellValue,
                            Estado = sheet.GetRow(row).GetCell(3).StringCellValue,
                            Municipio = sheet.GetRow(row).GetCell(4).StringCellValue,
                            MesAnoInformacao = sheet.GetRow(row).GetCell(0).DateCellValue,
                            PrecoMedioRevenda = sheet.GetRow(row).GetCell(7).NumericCellValue
                        };
                    }
                }
            }

            return null;
        }


        public void PreencherDadosNotaFiscalSuperFaturamento(string caminhoExcel, string caminhoExcelAnp)
        {
            if (string.IsNullOrWhiteSpace(caminhoExcel))
                return;

            XSSFWorkbook workbookExcel;
            List<DadosPlanilhaAnpDto> listaDadosAnp = new List<DadosPlanilhaAnpDto>();
            using (FileStream file = new FileStream(string.Format("{0}/{1}", Constantes.CAMINHO_DOWNLOAD_ARQUIVO, caminhoExcel),
                FileMode.Open, FileAccess.ReadWrite))
            {
                workbookExcel = new XSSFWorkbook(file);
            }

            ISheet sheet = workbookExcel.GetSheet("Superfaturamento");
            for (int row = 2; row <= sheet.LastRowNum - 2; row++)
            {
                if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                {

                    if (sheet.GetRow(row).GetCell(0).CellType == CellType.String &&
                        sheet.GetRow(row).GetCell(0).StringCellValue == "TOTAL")
                    {
                        workbookExcel.Close();
                        return;
                    }

                    DadosNotaFiscalDto dadosNotaFiscal = new DadosNotaFiscalDto()
                    {
                        Combustivel = sheet.GetRow(row).GetCell(2).StringCellValue,
                        DataNota = sheet.GetRow(row).GetCell(0).DateCellValue,
                        Estado = "MINAS GERAIS", //TODO: Obter estado
                        Municipio = "FORMIGA", //TODO: Obter municipio
                        IdNota = sheet.GetRow(row).GetCell(1).NumericCellValue
                    };

                    var informacoes = listaDadosAnp.FirstOrDefault(dados => dadosNotaFiscal.Estado.ToLower() == dados.Estado.ToLower() &&
                        dadosNotaFiscal.Municipio.ToLower() == dados.Municipio.ToLower() &&
                        DeParaCombustivelMpmgAnp.ConverterCombustivelMpmgAnp(dadosNotaFiscal.Combustivel) == dados.Combustivel &&
                        dadosNotaFiscal.DataNota.ToString("MMM/yy", Culture).ToLower() == dados.MesAnoInformacao.ToString("MMM/yy", Culture).ToLower());

                    informacoes = informacoes ?? ObterInformacoesAnp(Constantes.NOME_ARQUIVO_ANP_PRECOS, dadosNotaFiscal);

                    if (informacoes != null)
                    {
                        if (!VerificaSeInformacoesExistemNaLista(listaDadosAnp, informacoes))
                        {
                            listaDadosAnp.Add(informacoes);
                        }

                        var celulaPreco = sheet.GetRow(row).GetCell(7);
                        celulaPreco.SetCellValue(informacoes.PrecoMedioRevenda);

                        using (FileStream fs = new FileStream(string.Format("{0}/{1}", Constantes.CAMINHO_DOWNLOAD_ARQUIVO, caminhoExcel),
                            FileMode.Create, FileAccess.Write))
                        {
                            workbookExcel.Write(fs);
                        }
                    }

                }
            }
            workbookExcel.Close();
        }

        private bool ValidarLinhaBuscada(DadosNotaFiscalDto dadosNotaFiscal, ISheet sheet, int row)
        {
            return dadosNotaFiscal.Estado.ToLower() == sheet.GetRow(row).GetCell(3).StringCellValue.ToLower() &&
                                       dadosNotaFiscal.Municipio.ToLower() == sheet.GetRow(row).GetCell(4).StringCellValue.ToLower() &&
                                       DeParaCombustivelMpmgAnp.ConverterCombustivelMpmgAnp(dadosNotaFiscal.Combustivel).ToLower() ==
                                       sheet.GetRow(row).GetCell(1).StringCellValue.ToLower() &&
                                       dadosNotaFiscal.DataNota.ToString("MMM/yy", Culture).ToLower() ==
                                       sheet.GetRow(row).GetCell(0).DateCellValue.ToString("MMM/yy", Culture).ToLower();
        }

        private static bool ValidaSeLinhaEhCabecalho(ISheet sheet, int row)
        {
            return "MÊS" == sheet.GetRow(row).GetCell(0).StringCellValue &&
                                    "PRODUTO" == sheet.GetRow(row).GetCell(1).StringCellValue &&
                                    "REGIÃO" == sheet.GetRow(row).GetCell(2).StringCellValue &&
                                    "ESTADO" == sheet.GetRow(row).GetCell(3).StringCellValue &&
                                    "MUNICÍPIO" == sheet.GetRow(row).GetCell(4).StringCellValue;
        }

        private static bool VerificaSeInformacoesExistemNaLista(List<DadosPlanilhaAnpDto> listaDadosAnp, DadosPlanilhaAnpDto informacoes)
        {
            return listaDadosAnp.Any(dados => dados.MesAnoInformacao == informacoes.MesAnoInformacao &&
                                    dados.Municipio == informacoes.Municipio &&
                                    dados.Estado == informacoes.Estado);
        }
    }
}
