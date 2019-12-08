using System;

namespace WebApp.Models
{
    public class CupomFiscal
    {
        public int SGDP { get; set; }
        public int NrNotaFiscal { get; set; }
        public string COO { get; set; }
        public string Posto { get; set; }
        public DateTime Data { get; set; }
        public string Combustivel { get; set; }
        public int Quantidade { get; set; }
        public double PrecoUnitario { get; set; }
        public double ValorTotal { get; set; }
        public string Cliente { get; set; }
        public int Hodometro { get; set; }
        public string Veiculo { get; set; }
        public string PlacaVeiculo { get; set; }
        public int Horario { get; set; }
    }
}