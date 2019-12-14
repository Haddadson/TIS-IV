using System.Collections.Generic;

namespace MPMG.Interfaces.DTO
{
    public class OutrasInformacoesModelDto
    {
        public string NumeroNotaFiscal { get; set; }
        public string Produto { get; set; }
        public double Quantidade { get; set; }
        public double ValorUnitario { get; set; }
        public double ValorTotalItem { get; set; }
        public double ValorTotalNota { get; set; }
        public double PrecoMedioAnp { get; set; }
        public string DiferencaMediaUnitaria { get; set; }
        public string DiferencaMediaTotal { get; set; }
        public string ValorMedioAtualizado { get; set; }
        public double PrecoMaximoAnp { get; set; }
        public string DiferencaMaximaUnitaria { get; set; }
        public string DiferencaMaximaTotal { get; set; }
        public string ValorMaximoAtualizado { get; set; }
        public int MesAnp { get; set; }
        public int AnoAnp { get; set; }
        public int MesFam { get; set; }
        public int AnoFam { get; set; }
        public string NomeDepartamento { get; set; }
        public string Veiculo { get; set; }
        public string PlacaVeiculo { get; set; }
        public List<string> CuponsFiscaisVinculados { get; set; }
    }
}
