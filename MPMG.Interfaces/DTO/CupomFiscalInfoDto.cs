using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Interfaces.DTO
{
    public class CupomFiscalInfoDto
    {
        public List<double> PrecosUnitarios { get; set; }
        public List<string> Clientes { get; set; }
        public List<string> Postos { get; set; }
        public List<string> NotasFiscais { get; set; }
    }
}
