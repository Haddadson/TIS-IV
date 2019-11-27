using ExcelDataReader;
using MPMG.Interfaces.DTO;
using MPMG.Repositories;
using MPMG.Repositories.Entidades;
using MPMG.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MPMG.Services
{
    public class PlanilhaAnpService
    {
        private readonly CultureInfo Culture;
        private readonly DadosAnpRepositorio dadosAnpRepositorio;

        public PlanilhaAnpService()
        {
            Culture = CultureInfo.GetCultureInfo("pt-BR");
            dadosAnpRepositorio = new DadosAnpRepositorio();
        }

        public void PopularBancoComDadosAnp(string caminhoExcelAnp)
        {

            string file = string.Format("{0}/{1}", Constantes.CAMINHO_DOWNLOAD_ARQUIVO, caminhoExcelAnp);

            FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader;
            
            if (file.ToLower().EndsWith("xls"))
                //1. Reading from a binary Excel file ('97-2003 format; *.xls) 
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = excelReader.AsDataSet();
            DataTable tabelas = result.Tables[0];
            int i = 0;

            foreach (DataRow linha in tabelas.Rows)
            {
                if (linha?.ItemArray?.GetValue(0)?.ToString() == "MÊS" && linha?.ItemArray?.GetValue(1)?.ToString() == "PRODUTO")
                {
                    break;
                }
                i++;
            }

            for (int a = 0; a < i; a++)
            {
                var dataRow = tabelas.Rows[a];
                dataRow.Delete();
            }

            tabelas.AcceptChanges();

            int contador = 0;

            if (tabelas.Rows[0]?.ItemArray?.GetValue(0)?.ToString() != "MÊS" && tabelas.Rows[0]?.ItemArray?.GetValue(1)?.ToString() != "PRODUTO")
                throw new Exception("Erro ao ler planilha da ANP");


            foreach (DataColumn coluna in tabelas.Columns)
            {
                if (!string.IsNullOrWhiteSpace(tabelas.Rows[0]?.ItemArray?.GetValue(contador)?.ToString()))
                {
                    coluna.ColumnName = tabelas.Rows[0]?.ItemArray?.GetValue(contador)?.ToString();
                    tabelas.AcceptChanges();
                }
                contador++;
            }

            tabelas.Rows[0].Delete();
            tabelas.AcceptChanges();

            DataView dv = new DataView(tabelas)
            {
                RowFilter = " REGIÃO = 'SUDESTE' AND ESTADO = 'MINAS GERAIS'"
            };


            var colunasInsert = new List<DataRow>();

            for (int cont = 0; cont < dv.Table.Rows.Count; cont++)
            {
                if(colunasInsert.Count < 100 && (cont + 100 < dv.Table.Rows.Count))
                {
                    if (ValidarLinha(dv, cont))
                        colunasInsert.Add(dv.Table.Rows[cont]);
                    else
                        continue;
                }
                else
                {
                    //TODO: chamar repositorio
                    colunasInsert.Clear();
                }
            }


            stream.Close();

        }

        private static bool ValidarLinha(DataView dv, int cont)
        {
            return !string.IsNullOrWhiteSpace(dv.Table.Rows[cont]["MÊS"].ToString()) &&
                   !string.IsNullOrWhiteSpace(dv.Table.Rows[cont]["PRODUTO"].ToString()) &&
                   !string.IsNullOrWhiteSpace(dv.Table.Rows[cont]["PREÇO MÉDIO REVENDA"].ToString()) &&
                   !string.IsNullOrWhiteSpace(dv.Table.Rows[cont]["PREÇO MÍNIMO REVENDA"].ToString()) &&
                   !string.IsNullOrWhiteSpace(dv.Table.Rows[cont]["PREÇO MÁXIMO REVENDA"].ToString()) &&
                   !string.IsNullOrWhiteSpace(dv.Table.Rows[cont]["MUNICÍPIO"].ToString());
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
