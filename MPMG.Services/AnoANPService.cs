using System;
using System.Collections.Generic;
using System.Text;
using MPMG.Interfaces.DTO;
using MPMG.Repositories;
using MPMG.Repositories.Entidades;

namespace MPMG.Services
{
    public class AnoANPService
    {
        private readonly AnoANPRepo anoANPRepo;
        public AnoANPService()
        {
            anoANPRepo = new AnoANPRepo();
        }

        public List<AnoANPDto> ListarMesesPorSGDP(string valorSgdp)
        {
            return anoANPRepo.ListarAnoseMesesANPPorSgdp(valorSgdp).ConvertAll(new Converter<AnoANP, AnoANPDto>(ConverterEntidadeParaDto));
        }

        private AnoANPDto ConverterEntidadeParaDto(AnoANP entidade)
        {
            if (entidade == null)
                return null;

            return new AnoANPDto()
            {
                ano = entidade.ano,
                meses = entidade.meses.ConvertAll(new Converter<MesANP, int>(GetMesFromMesANP))
            };
        }

        private int GetMesFromMesANP(MesANP entidade)
        {
            return entidade.mes;
        }
    }
}
