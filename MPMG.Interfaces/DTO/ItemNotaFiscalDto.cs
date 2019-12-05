using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Interfaces.DTO
{
    public class ItemNotaFiscalDto
    {
        public string Sgdp { get; set; }
        public int NumeroNotaFiscal { get; set; }
        public int IdItemNotaFiscal { get; set; }
        public string Produto { get; set; }
        public int Quantidade { get; set; }
        public double ValorUnitario { get; set; }
        public double ValorTotal { get; set; }
        
    }
}
