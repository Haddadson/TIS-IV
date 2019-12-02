using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Interfaces.DTO
{
    public class AnpxNotaFiscalModelDto
    {
        public AnpxNotaFiscalModelDto()
        {
            CuponsFiscaisVinculados = new List<string>();
        }

        public DateTime DataGeracao { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public string Produto { get; set; }
        public double Quantidade { get; set; }
        public double ValorUnitario { get; set; }
        public double ValorTotalItem { get; set; }
        public double ValorTotalNota { get; set; }
        public int NumeroFolha { get; set; }
        public double ValorFam { get; set; }
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
        public List<string> CuponsFiscaisVinculados { get; set; }
    }
}
