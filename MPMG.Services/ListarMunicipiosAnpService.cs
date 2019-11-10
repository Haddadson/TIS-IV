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

        public List<string> ListarMunicipiosAnpPorAno(int anoReferente)
        {
            return dadosAnpRepositorio.ListarMunicipiosAnpPorAno(anoReferente);
        }

    }
}
