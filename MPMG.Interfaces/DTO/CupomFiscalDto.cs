using System;

namespace MPMG.Interfaces.DTO
{
    public class CupomFiscalDto
    {
        public int Sgdp { get; set; }
        public string Coo { get; set; }
        public int NumeroNotaFiscal { get; set; }
        public string PostoReferente { get; set; }
        public int Hodometro { get; set; }
        public string Cliente { get; set; }
        public DateTime DataEmissao { get; set; }
        public int Quantidade { get; set; }
        public double PrecoUnitario { get; set; }
        public double ValorTotal { get; set; }
        public string Produto { get; set; }
        public string Veiculo { get; set; }
        public string PlacaVeiculo { get; set; }
    }
}