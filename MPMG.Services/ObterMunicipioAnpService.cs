using MPMG.Interfaces.DTO;
using MPMG.Repositories;
using MPMG.Repositories.Entidades;
using System.Collections.Generic;

namespace MPMG.Services
{
    public class ObterMunicipioAnpService
    {
        private readonly MunicipioRepositorio municipioRepositorio;
        private readonly MunicipioReferenteRepositorio municipioReferenteRepositorio;
        private readonly DadosAnpRepositorio dadosAnpRepositorio;


        public ObterMunicipioAnpService()
        {
            municipioRepositorio = new MunicipioRepositorio();
            municipioReferenteRepositorio = new MunicipioReferenteRepositorio();
            dadosAnpRepositorio = new DadosAnpRepositorio();

        }

        public MunicipioDto ObterMunicipioAnpPorNomeAno(List<int> anosReferentes, string nomeMunicipio)
        {
            return ConverterMunicipioParaDto(municipioRepositorio.ObterMunicipioAnpPorNomeAno(anosReferentes, nomeMunicipio));
        }

        public List<string> ListarMunicipiosAnpPorAno(List<int> anosReferentes)
        {
            return dadosAnpRepositorio.ListarMunicipiosAnpPorAno(anosReferentes);
        }

        private MunicipioDto ConverterMunicipioParaDto(Municipio entidade)
        {
            if (entidade == null)
                return null;

            return new MunicipioDto(entidade.Codigo, entidade.Nome);
        }

    }
}
