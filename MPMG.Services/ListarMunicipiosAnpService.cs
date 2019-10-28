using MPMG.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Services
{
    public class ListarMunicipiosAnpService
    {
        private DadosAnpRepositorio dadosAnpRepositorio;

        public ListarMunicipiosAnpService()
        {
            dadosAnpRepositorio = new DadosAnpRepositorio();
        }

        public List<string> ListarMunicipiosAnp()
        {
            return dadosAnpRepositorio.ListarMunicipiosAnp();
        }

    }
}
