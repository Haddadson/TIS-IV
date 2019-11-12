using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Repositories.Entidades
{
    public class AnpxNotaFiscal
    {
        public DateTime DataGeracao { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public string Combustivel { get; set; }
        public double Quantidade { get; set; }
        public double ValorUnitario { get; set; }
        public double ValorTotal { get; set; }
        public int NumeroFolha { get; set; }
        public double ValorFam { get; set; }
        public double PrecoMedioAnp { get; set; }
        public double DiferencaMediaUnitaria { get; set; }
        public double DiferencaMediaTotal { get; set; }
        public double ValorMedioAtualizado { get; set; }
        public double PrecoMaximoAnp { get; set; }
        public double DiferencaMaximaUnitaria { get; set; }
        public double DiferencaMaximaTotal { get; set; }
        public double ValorMaximoAtualizado { get; set; }
        public int MesAnp { get; set; }
        public int AnoAnp { get; set; }
    }
}
