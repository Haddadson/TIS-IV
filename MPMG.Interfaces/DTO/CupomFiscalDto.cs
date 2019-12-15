using System;

namespace MPMG.Interfaces.DTO
{
    public class CupomFiscalDto
    {
        public string Sgdp { get; set; }
        public string Coo { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public int Hodometro { get; set; }
        public DateTime DataEmissao { get; set; }
        public double Quantidade { get; set; }
        public double PrecoUnitario { get; set; }
        public double ValorTotal { get; set; }
        public string Produto { get; set; }
        public string Veiculo { get; set; }
        public string PlacaVeiculo { get; set; }
    }
}