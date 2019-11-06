using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class NotaFiscal
    {
        public int NrNotaFiscal { get; set; }
        public int SGDP { get; set; }        
        public double ValorTotal { get; set; }
        public string ChaveAcesso { get; set; }
        public string DataEmissao { get; set; }
        public double PrecoMaximo { get; set; }
        public double PrecoMedio { get; set; }
        public string DataConsultaANP { get; set; }
        public string Veiculo { get; set; }
        public string PlacaVeiculo { get; set; }
        public string Combustivel { get; set; }
        public int Quantidade { get; set; }
        public double PrecoUnitario { get; set; }        
        public int NumeroFolha { get; set; } 
        public int Departamento { get; set; }
        public List<string> CuponsSelecionados { get; set; }
    }
}