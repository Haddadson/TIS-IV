using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Drawing;
using System.IO;

namespace MPMG.Util.Excel
{
    public class ManipuladorExcelTabelas
    {
        private IWorkbook WorkBook { get; set; }
        private ISheet Sheet1 { get; set; }
        private ISheet Sheet2 { get; set; }
        private ISheet Sheet3 { get; set; }
        private XSSFCellStyle EstiloCelulaTitulo { get; set; }
        private XSSFCellStyle EstiloCelulaCabecalho { get; set; }
        private IFont FonteCabecalho { get; set; }
        private XSSFCellStyle EstiloCelulaCorpoTexto { get; set; }
        private XSSFCellStyle EstiloCelulaCorpoValorNumerico { get; set; }
        private XSSFCellStyle EstiloCelulaCorpoValorInteiro { get; set; }
        private XSSFCellStyle EstiloCelulaCorpoData { get; set; }
        private IFont FonteCelulaCorpo { get; set; }

        public ManipuladorExcelTabelas()
        {
            WorkBook = new XSSFWorkbook();
            Sheet1 = WorkBook.CreateSheet("NFs X ANP");
            Sheet2 = WorkBook.CreateSheet("Cupons Fiscais");
            Sheet3 = WorkBook.CreateSheet("Outras informações");
            CriarEstilosDaPlanilha();
        }

        #region Criação de estilos

        private void CriarEstilosDaPlanilha()
        {
            CriarFonteCabecalho();
            CriarFonteCorpo();
            CriarEstiloCabecalho();
            CriarEstiloTitulo();
            CriarEstilosCorpo();
        }

        private void CriarFonteCabecalho()
        {
            FonteCabecalho = CriarFonte();
            FonteCabecalho.Color = IndexedColors.Black.Index;
            FonteCabecalho.IsBold = true;
        }

        private IFont CriarFonte()
        {
            IFont fonte = WorkBook.CreateFont();
            fonte.FontHeightInPoints = 12;
            fonte.FontName = "Calibri";
            return fonte;
        }

        private void CriarFonteCorpo()
        {
            FonteCelulaCorpo = CriarFonte();
            FonteCelulaCorpo.FontHeightInPoints = 11;
            FonteCelulaCorpo.Color = HSSFColor.Black.Index;
        }

        private void CriarEstiloCabecalho()
        {
            EstiloCelulaCabecalho = (XSSFCellStyle)WorkBook.CreateCellStyle();
            EstiloCelulaCabecalho.Alignment = HorizontalAlignment.Center;
            EstiloCelulaCabecalho.VerticalAlignment = VerticalAlignment.Center;
            EstiloCelulaCabecalho.SetFont(FonteCabecalho);
            EstiloCelulaCabecalho.WrapText = true;
            EstiloCelulaCabecalho.BorderBottom = BorderStyle.Thin;
            EstiloCelulaCabecalho.BorderTop = BorderStyle.Thin;
            EstiloCelulaCabecalho.BorderLeft = BorderStyle.Thin;
            EstiloCelulaCabecalho.BorderRight = BorderStyle.Thin;
        }

        private void CriarEstiloTitulo()
        {
            EstiloCelulaTitulo = (XSSFCellStyle)WorkBook.CreateCellStyle();
            EstiloCelulaTitulo.Alignment = HorizontalAlignment.Center;
            EstiloCelulaTitulo.VerticalAlignment = VerticalAlignment.Center;
            EstiloCelulaTitulo.SetFont(FonteCabecalho);
            EstiloCelulaTitulo.WrapText = true;
        }

        private void CriarEstilosCorpo()
        {
            CriarEstiloCelulaCorpoNumerica();
            CriarEstiloCelulaCorpoValorInteiro();
            CriarEstiloCelulaCorpoTexto();
            CriarEstiloCelulaCorpoData();
        }

        private void CriarEstiloCelulaCorpoNumerica()
        {
            EstiloCelulaCorpoValorNumerico = CriarEstiloCelulaCorpo();
            EstiloCelulaCorpoValorNumerico.Alignment = HorizontalAlignment.Right;
            IDataFormat dataFormat = WorkBook.CreateDataFormat();
            string formato = ObterFormatoCelulaNumerica(2);
            EstiloCelulaCorpoValorNumerico.DataFormat = dataFormat.GetFormat(formato);
        }

        private XSSFCellStyle CriarEstiloCelulaCorpo()
        {
            var estilo = (XSSFCellStyle)WorkBook.CreateCellStyle();
            estilo.VerticalAlignment = VerticalAlignment.Center;
            estilo.SetFillForegroundColor(new XSSFColor(Color.Transparent));
            estilo.FillPattern = FillPattern.NoFill;
            estilo.SetFont(FonteCelulaCorpo);
            estilo.BorderBottom = BorderStyle.Thin;
            estilo.BorderTop = BorderStyle.Thin;
            estilo.BorderLeft = BorderStyle.Thin;
            estilo.BorderRight = BorderStyle.Thin;
            return estilo;
        }

        private string ObterFormatoCelulaNumerica(int numeroCasasDecimais)
        {
            string formato = "#,##0";

            if (numeroCasasDecimais != 0)
            {
                string formatoCasasDecimais = "0".PadRight(numeroCasasDecimais, '0');
                formato = "#,##0." + formatoCasasDecimais;
            }

            return formato;
        }

        private void CriarEstiloCelulaCorpoValorInteiro()
        {
            EstiloCelulaCorpoValorInteiro = CriarEstiloCelulaCorpo();
            EstiloCelulaCorpoValorInteiro.Alignment = HorizontalAlignment.Right;
            IDataFormat dataFormat = WorkBook.CreateDataFormat();
            string formato = ObterFormatoCelulaNumerica(0);
            EstiloCelulaCorpoValorInteiro.DataFormat = dataFormat.GetFormat(formato);
        }

        private void CriarEstiloCelulaCorpoTexto()
        {
            EstiloCelulaCorpoTexto = CriarEstiloCelulaCorpo();
            EstiloCelulaCorpoTexto.Alignment = HorizontalAlignment.Left;
            IDataFormat dataFormat = WorkBook.CreateDataFormat();
            EstiloCelulaCorpoTexto.DataFormat = dataFormat.GetFormat("@");
            EstiloCelulaCorpoTexto.WrapText = true;
        }

        private void CriarEstiloCelulaCorpoData()
        {
            EstiloCelulaCorpoData = CriarEstiloCelulaCorpo();
            EstiloCelulaCorpoData.Alignment = HorizontalAlignment.Left;
            IDataFormat dataFormat = WorkBook.CreateDataFormat();
            EstiloCelulaCorpoData.DataFormat = dataFormat.GetFormat("dd/MM/yyyy");
        }

        #endregion

        #region Obtenção de célula estilizada

        protected ICell ObterCelulaParaCabecalho(int numeroLinha, int numeroColuna, string nomeAba, bool borda, IndexedColors cor = null, XSSFColor corXssf = null)
        {
            ICell celula = ObterCelula(numeroLinha, numeroColuna, nomeAba);

            var estiloTemporario = (XSSFCellStyle)WorkBook.CreateCellStyle();
            estiloTemporario = (XSSFCellStyle)EstiloCelulaCabecalho.Clone();

            if (!borda)
            {
                estiloTemporario.BorderBottom = BorderStyle.None;
                estiloTemporario.BorderTop = BorderStyle.None;
                estiloTemporario.BorderLeft = BorderStyle.None;
                estiloTemporario.BorderRight = BorderStyle.None;
            }

            if (cor != null)
            {
                estiloTemporario.FillForegroundColor = cor.Index;
                estiloTemporario.FillPattern = FillPattern.SolidForeground;
            }
            else if (corXssf != null)
            {
                estiloTemporario.SetFillForegroundColor(corXssf);
                estiloTemporario.FillPattern = FillPattern.SolidForeground;
            }

            celula.CellStyle = estiloTemporario;
            return celula;
        }

        protected ICell ObterCelulaParaTitulo(int numeroLinha, int numeroColuna, bool borda, string nomeAba, IndexedColors cor = null)
        {
            ICell celula = ObterCelula(numeroLinha, numeroColuna, nomeAba);

            var estiloTemporario = (XSSFCellStyle)WorkBook.CreateCellStyle();
            estiloTemporario = (XSSFCellStyle)EstiloCelulaTitulo.Clone();

            if (!borda)
            {
                estiloTemporario.BorderBottom = BorderStyle.None;
                estiloTemporario.BorderTop = BorderStyle.None;
                estiloTemporario.BorderLeft = BorderStyle.None;
                estiloTemporario.BorderRight = BorderStyle.None;
            }

            if (cor != null)
            {
                estiloTemporario.FillForegroundColor = cor.Index;
                estiloTemporario.FillPattern = FillPattern.SolidForeground;
            }

            celula.CellStyle = estiloTemporario;
            return celula;
        }

        protected ICell ObterCelula(int numeroLinha, int numeroColuna, string nomeAba)
        {
            IRow linha = ObterLinha(numeroLinha, nomeAba);
            return linha.GetCell(numeroColuna) ?? linha.CreateCell(numeroColuna);
        }

        protected IRow ObterLinha(int numeroLinha, string nomeAba)
        {

            ISheet Sheet = SelecionarAba(nomeAba);

            return Sheet.GetRow(numeroLinha) ?? Sheet.CreateRow(numeroLinha);
        }

        protected ICell ObterCelulaTexto(int numeroLinha, int numeroColuna, string nomeAba, bool borda, IndexedColors cor = null)
        {
            ICell celula = ObterCelula(numeroLinha, numeroColuna, nomeAba);

            var estiloTemporario = (XSSFCellStyle)WorkBook.CreateCellStyle();
            estiloTemporario = (XSSFCellStyle)EstiloCelulaCorpoTexto.Clone();

            if (!borda)
            {
                estiloTemporario.BorderBottom = BorderStyle.None;
                estiloTemporario.BorderTop = BorderStyle.None;
                estiloTemporario.BorderLeft = BorderStyle.None;
                estiloTemporario.BorderRight = BorderStyle.None;
            }

            if (cor != null)
            {
                estiloTemporario.FillForegroundColor = cor.Index;
                estiloTemporario.FillPattern = FillPattern.SolidForeground;
            }

            celula.CellStyle = EstiloCelulaCorpoTexto;
            return celula;
        }

        protected ICell ObterCelulaValorNumerico(int numeroLinha, int numeroColuna, string nomeAba, bool borda, IndexedColors cor = null)
        {
            ICell celula = ObterCelula(numeroLinha, numeroColuna, nomeAba);

            var estiloTemporario = (XSSFCellStyle)WorkBook.CreateCellStyle();
            estiloTemporario = (XSSFCellStyle)EstiloCelulaCorpoValorNumerico.Clone();

            if (!borda)
            {
                estiloTemporario.BorderBottom = BorderStyle.None;
                estiloTemporario.BorderTop = BorderStyle.None;
                estiloTemporario.BorderLeft = BorderStyle.None;
                estiloTemporario.BorderRight = BorderStyle.None;
            }

            if (cor != null)
            {
                estiloTemporario.FillForegroundColor = cor.Index;
                estiloTemporario.FillPattern = FillPattern.SolidForeground;
            }

            celula.CellStyle = estiloTemporario;
            return celula;
        }

        protected ICell ObterCelulaValorInteiro(int numeroLinha, int numeroColuna, string nomeAba, bool borda, IndexedColors cor = null)
        {
            ICell celula = ObterCelula(numeroLinha, numeroColuna, nomeAba);

            var estiloTemporario = (XSSFCellStyle)WorkBook.CreateCellStyle();
            estiloTemporario = (XSSFCellStyle)EstiloCelulaCorpoValorInteiro.Clone();

            if (!borda)
            {
                estiloTemporario.BorderBottom = BorderStyle.None;
                estiloTemporario.BorderTop = BorderStyle.None;
                estiloTemporario.BorderLeft = BorderStyle.None;
                estiloTemporario.BorderRight = BorderStyle.None;
            }

            if (cor != null)
            {
                estiloTemporario.FillForegroundColor = cor.Index;
                estiloTemporario.FillPattern = FillPattern.SolidForeground;
            }

            celula.CellStyle = EstiloCelulaCorpoValorInteiro;
            return celula;
        }

        protected ICell ObterCelulaData(int numeroLinha, int numeroColuna, string nomeAba, bool borda, IndexedColors cor = null)
        {
            ICell celula = ObterCelula(numeroLinha, numeroColuna, nomeAba);

            var estiloTemporario = (XSSFCellStyle)WorkBook.CreateCellStyle();
            estiloTemporario = (XSSFCellStyle)EstiloCelulaCorpoData.Clone();

            if (!borda)
            {
                estiloTemporario.BorderBottom = BorderStyle.None;
                estiloTemporario.BorderTop = BorderStyle.None;
                estiloTemporario.BorderLeft = BorderStyle.None;
                estiloTemporario.BorderRight = BorderStyle.None;
            }

            if (cor != null)
            {
                estiloTemporario.FillForegroundColor = cor.Index;
                estiloTemporario.FillPattern = FillPattern.SolidForeground;
            }

            celula.CellStyle = estiloTemporario;
            return celula;
        }

        #endregion

        #region Métodos de preenchimento de células

        public void PreencherCelulaCabecalho(int linha, int coluna, string valor, string nomeAba, bool borda, IndexedColors cor = null, XSSFColor corXssf = null)
        {
            ICell celulaCorrente = ObterCelulaParaCabecalho(linha, coluna, nomeAba, borda, cor, corXssf);
            celulaCorrente.SetCellValue(valor);
        }

        public void PreencherCelulaTitulo(int linha, int coluna, string valor, string nomeAba, bool borda, IndexedColors cor = null)
        {
            ICell celulaCorrente = ObterCelulaParaTitulo(linha, coluna, borda, nomeAba, cor);
            celulaCorrente.SetCellValue(valor);
        }

        public void PreencherCelulaTexto(int linha, int coluna, string valor, string nomeAba, bool borda, IndexedColors cor = null)
        {
            ICell celulaCorrente = ObterCelulaTexto(linha, coluna, nomeAba, borda, cor);
            celulaCorrente.SetCellValue(valor);
        }

        public void PreencherCelulaNumerica(int linha, int coluna, double? valor, string nomeAba, bool borda, IndexedColors cor = null)
        {
            ICell celulaCorrente = ObterCelulaValorNumerico(linha, coluna, nomeAba, borda, cor);
            if (valor.HasValue)
                celulaCorrente.SetCellValue(valor.Value);

        }

        public void PreencherCelulaValorInteiro(int linha, int coluna, int? valor, string nomeAba, bool borda, IndexedColors cor = null)
        {
            ICell celulaCorrente = ObterCelulaValorInteiro(linha, coluna, nomeAba, borda, cor);
            if (valor.HasValue)
                celulaCorrente.SetCellValue(valor.Value);
        }

        public void PreencherCelulaData(int linha, int coluna, DateTime? valor, string nomeAba, bool borda, IndexedColors cor = null)
        {
            ICell celulaCorrente = ObterCelulaData(linha, coluna, nomeAba, borda, cor);
            if (valor.HasValue)
                celulaCorrente.SetCellValue(valor.Value);
        }

        #endregion

        public void DefinirAlturaDaLinha(int linha, int tamanho, string nomeAba)
        {
            ObterLinha(linha, nomeAba).HeightInPoints = tamanho;
        }

        public void DefinirLarguraDaColuna(int coluna, int tamanho, string nomeAba)
        {
            ISheet Sheet = SelecionarAba(nomeAba);
            Sheet.SetColumnWidth(coluna, (tamanho * 256) + (int)Math.Round(0.71 * 256));
        }

        public void CriarCelulaMerge(int primeiraLinha, int ultimaLinha, int primeiraColuna, int ultimaColuna,
            string nomeAba)
        {
            ISheet Sheet = SelecionarAba(nomeAba);
            var cra = new NPOI.SS.Util.CellRangeAddress(primeiraLinha, ultimaLinha, primeiraColuna, ultimaColuna);

            Sheet.AddMergedRegion(cra);
        }

        public MemoryStream ObterVetorDeBytes()
        {
            MemoryStream arquivoExcelStream = new MemoryStream();
            WorkBook.Write(arquivoExcelStream);
            return arquivoExcelStream;
        }

        public XSSFCellStyle CriarEstilo()
        {
            return (XSSFCellStyle)WorkBook.CreateCellStyle();
        }

        private ISheet SelecionarAba(string nome)
        {
            switch (nome)
            {
                case "NFs X ANP":
                    return Sheet1;
                case "Cupons Fiscais":
                    return Sheet2;
                case "Outras informações":
                    return Sheet3;
            }

            return Sheet1;
        }

    }
}
