
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
        private readonly AnpxNotaFiscalRepositorio anpxNotaFiscalRepositorio;
        private readonly CupomFiscalRepo cupomFiscalRepositorio;
        private readonly OutrasInformacoesRepositorio outrasInfosRepositorio;


        public TabelaUsuarioService()
        {
            tabelaRepositorio = new TabelaUsuarioRepo();
            municipioRepositorio = new MunicipioRepositorio();
            municipioReferenteRepositorio = new MunicipioReferenteRepositorio();
            anpxNotaFiscalRepositorio = new AnpxNotaFiscalRepositorio();
            cupomFiscalRepositorio = new CupomFiscalRepo();
            outrasInfosRepositorio = new OutrasInformacoesRepositorio();
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

                if (municipioInserido == null || municipioReferente == null)
                    throw new Exception("Ocorreu um erro interno");

                entidadeMunicipioRef = municipioReferenteRepositorio.InserirMunicipioReferente(municipioInserido.Codigo,
                    municipioReferente.Codigo,
                    AnoReferente);
            }

            int idMunicipio = 0;
            int idMunicipioReferente = 0;

            if (municipio == null && entidadeMunicipioRef == null)
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

        public TabelaUsuarioDto ObterTabelaComDadosAnpxNotaFiscal(string sgdp)
        {
            var tabela = ConverterEntidadeParaDto(tabelaRepositorio.ObterTabelaPorSgdp(int.Parse(sgdp)));

            if (tabela == null || (tabela.Municipio == null && tabela.MunicipioReferente == null))
                throw new Exception("Erro ao encontrar");

            int idMunicipio = tabela.MunicipioReferente?.Codigo ?? tabela.Municipio.Codigo;
            try
            {
                tabela.DadosAnpxNotaFiscal = ListarDadosAnpXNotaFiscalPorSgdp(tabela.SGDP, idMunicipio);

                tabela.DadosAnpxNotaFiscal.ForEach(dado =>
                    dado.CuponsFiscaisVinculados = cupomFiscalRepositorio.ObterCuponsVinculados(tabela.SGDP, int.Parse(dado.NumeroNotaFiscal)));

            }
            catch (Exception ex)
            {
                tabela.DadosAnpxNotaFiscal = new List<AnpxNotaFiscalDto>();
            }

            return tabela;
        }

        public TabelaUsuarioDto ObterTabelaOutrasInformacoes(string sgdp)
        {
            var tabela = ConverterEntidadeParaDto(tabelaRepositorio.ObterTabelaPorSgdp(int.Parse(sgdp)));

            if (tabela == null || (tabela.Municipio == null && tabela.MunicipioReferente == null))
                throw new Exception("Erro ao encontrar");

            int idMunicipio = tabela.MunicipioReferente?.Codigo ?? tabela.Municipio.Codigo;
            try
            {
                tabela.OutrasInformacoes = ListarDadosOutrasInformacoes(tabela.SGDP, idMunicipio);

                tabela.OutrasInformacoes.ForEach(dado =>
                    dado.CuponsFiscaisVinculados = cupomFiscalRepositorio.ObterCuponsVinculados(tabela.SGDP, int.Parse(dado.NumeroNotaFiscal)));

            }
            catch (Exception ex)
            {
                tabela.OutrasInformacoes = new List<OutrasInformacoesDto>();
            }

            return tabela;
        }

        public List<TabelaUsuarioDto> ListarTabelas()
        {
            return ConverterListaEntidadeParaDto(tabelaRepositorio.ListarTabelas());
        }

        public List<int> ListarSgdpsTabelas()
        {
            return tabelaRepositorio.ListarSgdpsTabelas().Select(item => item.SGDP).ToList();
        }

        public List<TabelaUsuarioDto> ListarTabelasComDadosAnpxNotaFiscal()
        {
            var tabelas = ConverterListaEntidadeParaDto(tabelaRepositorio.ListarTabelas());

            foreach (var tabelaUsuario in tabelas)
            {
                if (tabelaUsuario.Municipio == null && tabelaUsuario.MunicipioReferente == null)
                    continue;

                int idMunicipio = tabelaUsuario.MunicipioReferente?.Codigo ?? tabelaUsuario.Municipio.Codigo;

                tabelaUsuario.DadosAnpxNotaFiscal = ListarDadosAnpXNotaFiscalPorSgdp(tabelaUsuario.SGDP, idMunicipio);
            }

            return tabelas;
        }

        public List<AnpxNotaFiscalDto> ListarDadosAnpXNotaFiscalPorSgdp(int sgdp, int idMunicipio)
        {
            return ConverterListaEntidadeDadosAnpxNotaParaDto(anpxNotaFiscalRepositorio.ListarNotasFiscaisPorSgdp(sgdp, idMunicipio));
        }

        public List<OutrasInformacoesDto> ListarDadosOutrasInformacoes(int sgdp, int idMunicipio)
        {
            return ConverterListaEntidadeOutrasInfosParaDto(outrasInfosRepositorio.ListarNotasFiscaisPorSgdp(sgdp, idMunicipio));
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
                Municipio = new MunicipioDto(entidade.IdMunicipio, entidade.NomeMunicipio),
                MunicipioReferente = new MunicipioDto(entidade.IdMunicipioReferente, entidade.NomeMunicipioReferente),
            };
        }

        private List<TabelaUsuarioDto> ConverterListaEntidadeParaDto(List<TabelaUsuario> entidades)
        {
            if (entidades == null || !entidades.Any())
                return new List<TabelaUsuarioDto>();

            return entidades.Select(ConverterEntidadeParaDto).ToList();
        }

        private AnpxNotaFiscalDto ConverterDadosAnpXNotaFiscalParaDto(AnpxNotaFiscal entidade)
        {
            if (entidade == null)
                return null;

            return new AnpxNotaFiscalDto()
            {
                Produto = entidade.Produto,
                DataGeracao = entidade.DataGeracao,
                NumeroFolha = entidade.NumeroFolha,
                NumeroNotaFiscal = entidade.NumeroNotaFiscal,
                PrecoMaximoAnp = entidade.PrecoMaximoAnp,
                PrecoMedioAnp = entidade.PrecoMedioAnp,
                Quantidade = entidade.Quantidade,
                ValorFam = double.Parse(entidade.ValorFam.Replace('.', ',')),
                ValorMaximoAtualizado = CalcularValorAtualizado(entidade.ValorFam, entidade.ValorUnitario, entidade.PrecoMaximoAnp, entidade.Quantidade),
                ValorMedioAtualizado = CalcularValorAtualizado(entidade.ValorFam, entidade.ValorUnitario, entidade.PrecoMedioAnp, entidade.Quantidade),
                ValorTotalItem = entidade.ValorTotalItem,
                ValorTotalNota = entidade.ValorTotalNota,
                ValorUnitario = entidade.ValorUnitario,
                MesAnp = entidade.MesAnp,
                AnoAnp = entidade.AnoAnp,
                MesFam = entidade.MesFam,
                AnoFam = entidade.AnoFam,
                DiferencaMediaUnitaria = entidade.ValorUnitario - entidade.PrecoMedioAnp,
                DiferencaMediaTotal = (entidade.ValorUnitario - entidade.PrecoMedioAnp) * entidade.Quantidade,
                DiferencaMaximaUnitaria = entidade.ValorUnitario - entidade.PrecoMaximoAnp,
                DiferencaMaximaTotal = (entidade.ValorUnitario - entidade.PrecoMaximoAnp) * entidade.Quantidade
            };
        }

        private static double CalcularValorAtualizado(string ValorFam, double ValorUnitario, double PrecoAnp, double Quantidade)
        {
            return (double.Parse(ValorFam.Replace('.', ',')) * ((ValorUnitario - PrecoAnp) * Quantidade));
        }

        private List<OutrasInformacoesDto> ConverterListaEntidadeOutrasInfosParaDto(List<OutrasInformacoes> entidades)
        {
            if (entidades == null || !entidades.Any())
                return new List<OutrasInformacoesDto>();

            return entidades.Select(ConverterOutrasInformacoesParaDto).ToList();
        }

        private OutrasInformacoesDto ConverterOutrasInformacoesParaDto(OutrasInformacoes entidade)
        {
            if (entidade == null)
                return null;

            return new OutrasInformacoesDto()
            {
                Produto = entidade.Produto,
                NumeroNotaFiscal = entidade.NumeroNotaFiscal,
                PrecoMaximoAnp = entidade.PrecoMaximoAnp,
                PrecoMedioAnp = entidade.PrecoMedioAnp,
                Quantidade = entidade.Quantidade,
                ValorMaximoAtualizado = CalcularValorAtualizado(entidade.ValorFam, entidade.ValorUnitario, entidade.PrecoMaximoAnp, entidade.Quantidade),
                ValorMedioAtualizado = CalcularValorAtualizado(entidade.ValorFam, entidade.ValorUnitario, entidade.PrecoMedioAnp, entidade.Quantidade),
                ValorTotalItem = entidade.ValorTotalItem,
                ValorTotalNota = entidade.ValorTotalNota,
                ValorUnitario = entidade.ValorUnitario,
                MesAnp = entidade.MesAnp,
                AnoAnp = entidade.AnoAnp,
                MesFam = entidade.MesFam,
                AnoFam = entidade.AnoFam,
                NomeDepartamento = entidade.NomeDepartamento,
                PlacaVeiculo = entidade.PlacaVeiculo,
                Veiculo = entidade.Veiculo,
                DiferencaMediaUnitaria = entidade.ValorUnitario - entidade.PrecoMedioAnp,
                DiferencaMediaTotal = (entidade.ValorUnitario - entidade.PrecoMedioAnp) * entidade.Quantidade,
                DiferencaMaximaUnitaria = entidade.ValorUnitario - entidade.PrecoMaximoAnp,
                DiferencaMaximaTotal = (entidade.ValorUnitario - entidade.PrecoMaximoAnp) * entidade.Quantidade
            };
        }

        private List<AnpxNotaFiscalDto> ConverterListaEntidadeDadosAnpxNotaParaDto(List<AnpxNotaFiscal> entidades)
        {
            if (entidades == null || !entidades.Any())
                return new List<AnpxNotaFiscalDto>();

            return entidades.Select(ConverterDadosAnpXNotaFiscalParaDto).ToList();
        }
    }
}
