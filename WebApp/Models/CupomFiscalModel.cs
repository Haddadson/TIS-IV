using MPMG.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class CupomFiscalModel
    {
        public string ValorSgdp { get; set; }

        public CupomFiscalInfoDto CuponsInfo { get; set; }
    }
}