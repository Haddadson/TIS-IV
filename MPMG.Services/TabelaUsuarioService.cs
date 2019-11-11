
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
        private readonly TabelaUsuarioRepo tabelaRepositorio;
        private readonly MunicipioRepositorio municipioRepositorio;
        private readonly MunicipioReferenteRepositorio municipioReferenteRepositorio;

        public TabelaUsuarioService()
        {
            tabelaRepositorio = new TabelaUsuarioRepo();
            municipioRepositorio = new MunicipioRepositorio();
            municipioReferenteRepositorio = new MunicipioReferenteRepositorio();
        }

        public void CadastrarTabela(
            int SGDP,
            int AnoReferente,
            string NomeMunicipioReferente,
            string NomeMunicipio,
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

            var municipio = municipioRepositorio.ObterMunicipio(NomeMunicipio);
            MunicipioReferente entidadeMunicipioRef = null;

            if (municipio == null)
            {
                if (string.IsNullOrWhiteSpace(NomeMunicipioReferente))
                    throw new Exception("O município referente não foi informado");

                var municipioInserido = municipioRepositorio.InserirMunicipio(NomeMunicipio);
                var municipioReferente = municipioRepositorio.ObterMunicipio(NomeMunicipioReferente);

                if(municipioInserido == null || municipioReferente == null)
                    throw new Exception("Ocorreu um erro interno");

                entidadeMunicipioRef = municipioReferenteRepositorio.InserirMunicipioReferente(municipioInserido.Codigo, 
                    municipioReferente.Codigo, 
                    AnoReferente);
            }

            int idMunicipio = 0;
            int idMunicipioReferente = 0;

            if(municipio == null && entidadeMunicipioRef == null)
                throw new Exception("Ocorreu um erro interno");

            if (municipio != null)
            {
                idMunicipio = municipio.Codigo;
                idMunicipioReferente = municipio.Codigo;
            }
            else
            {
                idMunicipio = entidadeMunicipioRef.Codigo;
                idMunicipioReferente = entidadeMunicipioRef.CodigoMunicipioReferente;
            }

            tabelaRepositorio.CadastrarTabela(
                SGDP,
                AnoReferente,
                idMunicipioReferente,
                idMunicipio,
                DataGeracao,
                Titulo1,
                Titulo2,
                Titulo3,
                AnalistaResponsavel
           );
        }

        public TabelaUsuarioDto ObterTabela(string sgdp)
        {
            return ConverterEntidadeParaDto(tabelaRepositorio.ObterTabelaPorSgdp(int.Parse(sgdp)));
        }

        public List<TabelaUsuarioDto> ListarTabelas()
        {
            return ConverterListaEntidadeParaDto(tabelaRepositorio.ListarTabelas());
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
                Municipio = new Interfaces.DTO.MunicipioDto(entidade.IdMunicipio, entidade.NomeMunicipio),  
                MunicipioReferente = new Interfaces.DTO.MunicipioDto(entidade.IdMunicipioReferente, entidade.NomeMunicipioReferente),  
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
