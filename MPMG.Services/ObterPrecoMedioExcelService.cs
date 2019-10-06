using MPMG.Interfaces.DTO;
using MPMG.Repositories;
using MPMG.Util;
using static MPMG.Util.Enum.DeParaCombustivelMpmgAnp;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MPMG.Repositories.Entidades;

namespace MPMG.Services
{
    public class ObterPrecoMedioExcelService
    {
        private readonly CultureInfo Culture;
        private readonly DadosAnpRepositorio dadosAnpRepositorio;

        public ObterPrecoMedioExcelService()
        {
            Culture = CultureInfo.GetCultureInfo("pt-BR");
            dadosAnpRepositorio = new DadosAnpRepositorio();
        }

        //public DadosAnpDto ObterInformacoesAnp(string caminhoExcel, DadosNotaFiscalDto dadosNotaFiscal)
        //{
        //    if (string.IsNullOrWhiteSpace(caminhoExcel))
        //        return null;

        //    XSSFWorkbook workbookExcel;
        //    using (FileStream file = new FileStream(string.Format("{0}/{1}", Constantes.CAMINHO_DOWNLOAD_ARQUIVO, caminhoExcel),
        //        FileMode.Open, FileAccess.Read))
        //    {
        //        workbookExcel = new XSSFWorkbook(file);
        //    }

        //    ISheet sheet = workbookExcel.GetSheet(workbookExcel.GetSheetName(0));
        //    bool aposCabecalho = false;
        //    for (int row = 0; row <= sheet.LastRowNum; row++)
        //    {
        //        if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
        //        {
        //            if (aposCabecalho || ValidaSeLinhaEhCabecalho(sheet, row))
        //            {
        //                aposCabecalho = true;
        //            }

        //            if (aposCabecalho && ValidarLinhaBuscada(dadosNotaFiscal, sheet, row))
        //            {
        //                return new DadosAnpDto()
        //                {
        //                    Combustivel = sheet.GetRow(row).GetCell(1).StringCellValue,
        //                    Estado = sheet.GetRow(row).GetCell(3).StringCellValue,
        //                    Municipio = sheet.GetRow(row).GetCell(4).StringCellValue,
        //                    MesAnoInformacao = sheet.GetRow(row).GetCell(0).DateCellValue,
        //                    PrecoMedioRevenda = sheet.GetRow(row).GetCell(7).NumericCellValue
        //                };
        //            }
        //        }
        //    }

        //    return null;
        //}


        public void PreencherDadosNotaFiscalSuperFaturamento(string caminhoExcel, string caminhoExcelAnp)
        {
            if (string.IsNullOrWhiteSpace(caminhoExcel))
                return;

            XSSFWorkbook workbookExcel;
            List<DadosAnpDto> listaDadosAnp = new List<DadosAnpDto>();

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

                    //var informacoes = listaDadosAnp.FirstOrDefault(dados => dadosNotaFiscal.Estado.ToLower() == dados.Estado.ToLower() &&
                    //    dadosNotaFiscal.Municipio.ToLower() == dados.Municipio.ToLower() &&
                    //    DeParaCombustivelMpmgAnp.ConverterCombustivelMpmgAnp(dadosNotaFiscal.Combustivel) == dados.Combustivel &&
                    //    dadosNotaFiscal.DataNota.ToString("MMM/yy", Culture).ToLower() == dados.MesAnoInformacao.ToString("MMM/yy", Culture).ToLower());

                    //informacoes = informacoes ?? ObterInformacoesAnp(Constantes.NOME_ARQUIVO_ANP_PRECOS, dadosNotaFiscal);

                    var informacoes = dadosAnpRepositorio.ObterPorValoresNota(dadosNotaFiscal.DataNota.Month, dadosNotaFiscal.DataNota.Year,
                                                                              dadosNotaFiscal.Estado, dadosNotaFiscal.Municipio, 
                                                                              ConverterCombustivelMpmgAnp(dadosNotaFiscal.Combustivel));

                    var informacoesDto = ConverterDadosAnpParaDto(informacoes);
                    if (informacoesDto != null)
                    {
                        if (!VerificaSeInformacoesExistemNaLista(listaDadosAnp, informacoesDto))
                        {
                            listaDadosAnp.Add(informacoesDto);
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

        //private bool ValidarLinhaBuscada(DadosNotaFiscalDto dadosNotaFiscal, ISheet sheet, int row)
        //{
        //    return dadosNotaFiscal.Estado.ToLower() == sheet.GetRow(row).GetCell(3).StringCellValue.ToLower() &&
        //                               dadosNotaFiscal.Municipio.ToLower() == sheet.GetRow(row).GetCell(4).StringCellValue.ToLower() &&
        //                               DeParaCombustivelMpmgAnp.ConverterCombustivelMpmgAnp(dadosNotaFiscal.Combustivel).ToLower() ==
        //                               sheet.GetRow(row).GetCell(1).StringCellValue.ToLower() &&
        //                               dadosNotaFiscal.DataNota.ToString("MMM/yy", Culture).ToLower() ==
        //                               sheet.GetRow(row).GetCell(0).DateCellValue.ToString("MMM/yy", Culture).ToLower();
        //}

        //private static bool ValidaSeLinhaEhCabecalho(ISheet sheet, int row)
        //{
        //    return "MÊS" == sheet.GetRow(row).GetCell(0).StringCellValue &&
        //                            "PRODUTO" == sheet.GetRow(row).GetCell(1).StringCellValue &&
        //                            "REGIÃO" == sheet.GetRow(row).GetCell(2).StringCellValue &&
        //                            "ESTADO" == sheet.GetRow(row).GetCell(3).StringCellValue &&
        //                            "MUNICÍPIO" == sheet.GetRow(row).GetCell(4).StringCellValue;
        //}

        private bool VerificaSeInformacoesExistemNaLista(List<DadosAnpDto> listaDadosAnp, DadosAnpDto informacoes)
        {
            return listaDadosAnp.Any(dados => dados.Mes == informacoes.Mes &&
                                    dados.Ano == informacoes.Ano &&
                                    dados.Combustivel == informacoes.Combustivel &&
                                    dados.Municipio == informacoes.Municipio &&
                                    dados.Estado == informacoes.Estado);
        }

        private DadosAnpDto ConverterDadosAnpParaDto(DadosAnp entidade)
        {
            if (entidade == null)
                return null;

            return new DadosAnpDto()
            {
                Ano = entidade.Ano,
                Mes = entidade.Mes,
                Combustivel = entidade.Combustivel,
                Estado = entidade.Estado,
                Municipio = entidade.Municipio,
                PrecoMaximoRevenda = entidade.PrecoMaximoRevenda,
                PrecoMedioRevenda = entidade.PrecoMedioRevenda,
                PrecoMinimoRevenda = entidade.PrecoMinimoRevenda
            };
        }
    }
}
