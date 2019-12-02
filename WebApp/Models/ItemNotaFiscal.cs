using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ItemNotaFiscal
    {
        public string Combustivel { get; set; }
        public double PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public double ValorTotal { get; set; }
    }
}