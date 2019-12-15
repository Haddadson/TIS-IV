using ExcelDataReader;
using MPMG.Repositories;
using System;
using System.Data;
using System.IO;

namespace MPMG.Services
{
    public class PlanilhaFamService
    {
        private readonly UploadFamRepo uploadFamRepositorio;
        private readonly TabelaFamRepo tabelaFamRepositorio;

        public PlanilhaFamService()
        {
            uploadFamRepositorio = new UploadFamRepo();
            tabelaFamRepositorio = new TabelaFamRepo();
        }

        public bool AtualizarDadosTabelaFam(byte[] arquivoFam, string extensaoArquivo)
        {
            MemoryStream stream = new MemoryStream(arquivoFam);
            IExcelDataReader excelReader;

            if (extensaoArquivo.ToLower().EndsWith("xls"))
                //1. Reading from a binary Excel file ('97-2003 format; *.xls) 
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = excelReader.AsDataSet();
            DataTable tabelas = result.Tables[0];

            int indiceLinhaInicial = 0;
            int indiceLinhaFinal = 0;

            foreach (DataRow linha in tabelas.Rows)
            {
                if (linha?.ItemArray?.GetValue(0)?.ToString() == "Anos\\Meses" && linha?.ItemArray?.GetValue(1)?.ToString() == "jan")
                {
                    break;
                }
                indiceLinhaInicial++;
            }

            for (int a = 0; a < indiceLinhaInicial; a++)
            {
                var dataRow = tabelas.Rows[a];
                dataRow.Delete();
            }

            tabelas.AcceptChanges();

            if (tabelas.Rows[0]?.ItemArray?.GetValue(0)?.ToString() != "Anos\\Meses" && tabelas.Rows[0]?.ItemArray?.GetValue(1)?.ToString() != "jan")
                throw new Exception("Erro ao ler planilha FAM");

            foreach (DataRow linha in tabelas.Rows)
            {
                if (string.IsNullOrWhiteSpace(linha?.ItemArray?.GetValue(0)?.ToString()) &&
                    string.IsNullOrWhiteSpace(linha?.ItemArray?.GetValue(1)?.ToString()) &&
                    string.IsNullOrWhiteSpace(linha?.ItemArray?.GetValue(2)?.ToString()) &&
                    string.IsNullOrWhiteSpace(linha?.ItemArray?.GetValue(3)?.ToString()) &&
                    string.IsNullOrWhiteSpace(linha?.ItemArray?.GetValue(4)?.ToString()) &&
                    string.IsNullOrWhiteSpace(linha?.ItemArray?.GetValue(5)?.ToString()) &&
                    string.IsNullOrWhiteSpace(linha?.ItemArray?.GetValue(6)?.ToString()) &&
                    string.IsNullOrWhiteSpace(linha?.ItemArray?.GetValue(7)?.ToString()))
                {
                    break;
                }
                indiceLinhaFinal++;
            }

            for (int a = indiceLinhaFinal; a < tabelas.Rows.Count; a++)
            {
                var dataRow = tabelas.Rows[a];
                dataRow.Delete();
            }

            tabelas.AcceptChanges();

            int contador = 0;
            int indiceUltimaColuna = 0;
            bool ultimaIteracaoColuna = false;

            foreach (DataColumn coluna in tabelas.Columns)
            {
                if (!ultimaIteracaoColuna)
                {
                    if (!string.IsNullOrWhiteSpace(tabelas.Rows[0]?.ItemArray?.GetValue(contador)?.ToString()))
                    {
                        if (tabelas.Rows[0]?.ItemArray?.GetValue(contador)?.ToString() == "dez")
                        {
                            indiceUltimaColuna = contador;
                            ultimaIteracaoColuna = true;
                        }
                        if (indiceUltimaColuna <= contador)
                        {
                            coluna.ColumnName = tabelas.Rows[0]?.ItemArray?.GetValue(contador)?.ToString();
                            tabelas.AcceptChanges();
                        }
                    }
                    contador++;
                }
            }

            tabelas.Rows[0].Delete();

            for (int i = indiceUltimaColuna + 1; i < tabelas.Columns.Count; i++)
            {
                tabelas.Columns.RemoveAt(i);
            }

            tabelas.AcceptChanges();

            uploadFamRepositorio.InserirNovaData();
            var idUploadFam = (uploadFamRepositorio.ObterUltimoUpload()?.Id ?? 0) + 1;

            //tabelaFamRepositorio.DeletarTodosRegistros();

            int contErros = 0;

            for (int cont = 0; cont < tabelas.Rows.Count; cont++)
            {
                try
                {
                    tabelaFamRepositorio.InserirLoteFam(tabelas.Rows[cont], idUploadFam);
                }
                catch (Exception ex)
                {
                    contErros++;
                }
            }

            stream.Close();

            return contErros == 0;
        }
    }
}
