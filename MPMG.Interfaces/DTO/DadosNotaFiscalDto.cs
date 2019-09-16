using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Interfaces.DTO
{
    public class DadosNotaFiscalDto
    {
        public DateTime DataNota { get; set; }
        public double IdNota { get; set; }
        public string Combustivel { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
    }
}