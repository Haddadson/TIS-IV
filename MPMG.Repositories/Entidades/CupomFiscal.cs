using System;

namespace MPMG.Repositories.Entidades
{
    public class CupomFiscal
    {
        public string Sgdp { get; set; }
        public string Coo { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public string PostoReferente { get; set; }
        public double Hodometro { get; set; }
        public string Cliente { get; set; }
        public DateTime DataEmissao { get; set; }
        public double Quantidade { get; set; }
        public double PrecoUnitario { get; set; }
        public double ValorTotal { get; set; }
        public string Produto { get; set; }
        public string Veiculo { get; set; }
        public string PlacaVeiculo { get; set; }
    }
}