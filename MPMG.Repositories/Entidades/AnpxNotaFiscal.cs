﻿using System;

namespace MPMG.Repositories.Entidades
{
    public class AnpxNotaFiscal
    {
        public DateTime DataGeracao { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public string Produto { get; set; }
        public double Quantidade { get; set; }
        public double ValorUnitario { get; set; }
        public double ValorTotalItem { get; set; }
        public double ValorTotalNota { get; set; }
        public int NumeroFolha { get; set; }
        public string ValorFam { get; set; }
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
        public int MesFam { get; set; }
        public int AnoFam { get; set; }
    }
}
