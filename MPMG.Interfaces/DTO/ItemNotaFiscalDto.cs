using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Interfaces.DTO
{
    public class ItemNotaFiscalDto
    {
        public string Sgdp { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public int IdItemNotaFiscal { get; set; }
        public string Produto { get; set; }
        public double Quantidade { get; set; }
        public double ValorUnitario { get; set; }
        public double ValorTotal { get; set; }   
    }
}
