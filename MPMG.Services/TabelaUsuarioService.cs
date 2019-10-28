
using MPMG.Repositories;
using System;

namespace MPMG.Services
{
    public class TabelaUsuarioService
    {
        private readonly TabelaUsuarioRepo TabelaRepo;
        private readonly MunicipioRepositorio MunicipiosRepo;
        public TabelaUsuarioService()
        {
            TabelaRepo = new TabelaUsuarioRepo();
            MunicipiosRepo = new MunicipioRepositorio();
        }

        public void CadastrarTabela(
            int SGDP,
            int AnoReferente,
            int IdMunicipioReferente,
            String IdMunicipio,
            DateTime DataGeracao,
            string Titulo1,
            string Titulo2,
            string Titulo3,
            string AnalistaResponsavel)
        {

            if (SGDP <= 0)
            {
                throw new Exception("Valor inválido para o SGDP!");
            }

            if (IdMunicipioReferente <= 0)
            {
                throw new Exception("Não foi informado o código do município referente!");
            }

            int codigoMunicipio = MunicipiosRepo.buscarOuCriarMunicipio(IdMunicipio);


            TabelaRepo.CadastrarTabela(
                SGDP,
                AnoReferente,
                IdMunicipioReferente,
                codigoMunicipio,
                DataGeracao,
                Titulo1,
                Titulo2,
                Titulo3,
                AnalistaResponsavel
           );
        }
    }
}
