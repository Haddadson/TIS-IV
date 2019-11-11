
using MPMG.Interfaces.DTO;
using MPMG.Repositories;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

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
            String IdMunicipioReferente,
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

            int codigoMunicipio = MunicipiosRepo.buscarOuCriarMunicipio(IdMunicipio);
            int codigoMunicipioReferente;
            if (IdMunicipioReferente == null)
            {
                codigoMunicipioReferente = codigoMunicipio;
            } else
            {
                codigoMunicipioReferente = MunicipiosRepo.buscarOuCriarMunicipio(IdMunicipioReferente);
            }

            TabelaRepo.CadastrarTabela(
                SGDP,
                AnoReferente,
                codigoMunicipioReferente,
                codigoMunicipio,
                DataGeracao,
                Titulo1,
                Titulo2,
                Titulo3,
                AnalistaResponsavel
           );
        }

        public List<TabelaUsuarioDto> ListarTabelas()
        {
            return ConverterListaEntidadeParaDto(TabelaRepo.ListarTabelas());

        }

        private TabelaUsuarioDto ConverterEntidadeParaDto(TabelaUsuario entidade)
        {
            if (entidade == null)
                return null;

            return new TabelaUsuarioDto()
            {
                AnalistaResponsavel = entidade.AnalistaResponsavel,
                AnoReferente = entidade.AnoReferente,
                DataGeracao = entidade.DataGeracao,
                SGDP = entidade.SGDP,
                Titulo1 = entidade.Titulo1,
                Titulo2 = entidade.Titulo2,
                Titulo3 = entidade.Titulo3,
                Municipio = new Interfaces.DTO.Municipio(entidade.IdMunicipio, entidade.NomeMunicipio),  
                MunicipioReferente = new Interfaces.DTO.Municipio(entidade.IdMunicipioReferente, entidade.NomeMunicipioReferente),  
            };
        }

        private List<TabelaUsuarioDto> ConverterListaEntidadeParaDto(List<TabelaUsuario> entidades)
        {
            if (entidades == null || !entidades.Any())
                return new List<TabelaUsuarioDto>();

            return entidades.Select(ConverterEntidadeParaDto).ToList();
        }

    }
}
