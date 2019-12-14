
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
        private readonly TabelaANPRepo tabelaANPRepo;

        public TabelaUsuarioService()
        {
            tabelaRepositorio = new TabelaUsuarioRepo();
            tabelaANPRepo = new TabelaANPRepo();
            municipioRepositorio = new MunicipioRepositorio();
            municipioReferenteRepositorio = new MunicipioReferenteRepositorio();
            anpxNotaFiscalRepositorio = new AnpxNotaFiscalRepositorio();
            cupomFiscalRepositorio = new CupomFiscalRepo();
            outrasInfosRepositorio = new OutrasInformacoesRepositorio();
        }

        public void CadastrarTabela(
            string SGDP,
            List<int> AnosReferentes,
            string NomeMunicipioReferente,
            string NomeMunicipio,
            DateTime DataGeracao,
            string Titulo1,
            string Titulo2,
            string Titulo3,
            string AnalistaResponsavel)
        {
            if (string.IsNullOrWhiteSpace(SGDP))
            {
                throw new Exception("Valor inválido para o SGDP!");
            }

            if (string.IsNullOrWhiteSpace(NomeMunicipioReferente))
                NomeMunicipioReferente = NomeMunicipio;

            var idMunicipio          = this.BuscarMunicipio(NomeMunicipio);
            var idMunicipioReferente = this.BuscarMunicipioReferente(NomeMunicipioReferente);

            if(idMunicipio == 0 || idMunicipioReferente == 0)
            {
                throw new Exception("Os municípios informados são inválidos!");
            }

            tabelaRepositorio.CadastrarTabela(
                SGDP,
                idMunicipio,
                idMunicipioReferente,
                DataGeracao,
                Titulo1,
                Titulo2,
                Titulo3,
                AnalistaResponsavel
            );

            this.SalvarAnosReferentes(SGDP, AnosReferentes);
            this.SalvarMunicipioReferente(idMunicipio, idMunicipioReferente, AnosReferentes);
        }

        private void SalvarAnosReferentes(string SGDP, List<int> AnosReferentes)
        {
            foreach (var ano in AnosReferentes)
            {
                tabelaRepositorio.CadastrarAnoReferente(SGDP, ano);
            }
        }

        private void SalvarMunicipioReferente(int idMunicipio, int idMunicipioReferente, List<int> AnosReferentes)
        {
            foreach (var ano in AnosReferentes)
            {
                List<TabelaANP> tabelaDisponivel = tabelaANPRepo.BuscarMesesDisponiveisANP(idMunicipioReferente, ano);
                List<int> mesesANP = tabelaDisponivel.Select(i => i.mes).ToList();

                if(mesesANP == null || mesesANP.Count == 0)
                {
                    throw new Exception("Não foi possível buscar os meses na tabela da ANP.");
                }

                foreach(var mes in mesesANP)
                {
                    municipioReferenteRepositorio.InserirMunicipioReferente(
                        idMunicipio,
                        idMunicipioReferente,
                        ano,
                        mes);
                }
            }
        }

        private int BuscarMunicipio (string NomeMunicipio)
        {
            var municipio = municipioRepositorio.ObterMunicipio(NomeMunicipio);

            if (municipio == null) 
            {
                var municipioInserido = municipioRepositorio.InserirMunicipio(NomeMunicipio);
                
                if (municipioInserido == null)
                    throw new Exception("Ocorreu um erro ao salvar um novo município!");
                else
                    municipio = municipioRepositorio.ObterMunicipio(NomeMunicipio);
            }

            return municipio.Codigo;
        }

        private int BuscarMunicipioReferente(string NomeMunicipioReferente)
        {
            var municipio = municipioRepositorio.ObterMunicipio(NomeMunicipioReferente);
            return municipio.Codigo;
        }

        public TabelaUsuarioDto ObterTabela(string sgdp)
        {
            return ConverterEntidadeParaDto(tabelaRepositorio.ObterTabelaPorSgdp(sgdp));
        }

        public TabelaUsuarioDto ObterTabelaComDadosAnpxNotaFiscal(string sgdp)
        {
            var tabela = ConverterEntidadeParaDto(tabelaRepositorio.ObterTabelaPorSgdp(sgdp));

            if (tabela == null || (tabela.Municipio == null && tabela.MunicipioReferente == null))
                throw new Exception("Erro ao encontrar");

            int idMunicipio = tabela.MunicipioReferente?.Codigo ?? tabela.Municipio.Codigo;
            try
            {
                tabela.DadosAnpxNotaFiscal = ListarDadosAnpXNotaFiscalPorSgdp(tabela.SGDP, idMunicipio);

                tabela.DadosAnpxNotaFiscal.ForEach(dado =>
                    dado.CuponsFiscaisVinculados = cupomFiscalRepositorio.ObterCuponsVinculados(tabela.SGDP, dado.NumeroNotaFiscal));

            }
#pragma warning disable CS0168 // A variável "ex" está declarada, mas nunca é usada
            catch (Exception ex)
#pragma warning restore CS0168 // A variável "ex" está declarada, mas nunca é usada
            {
                tabela.DadosAnpxNotaFiscal = new List<AnpxNotaFiscalDto>();
            }

            return tabela;
        }

        public TabelaUsuarioDto ObterTabelaOutrasInformacoes(string sgdp)
        {
            var tabela = ConverterEntidadeParaDto(tabelaRepositorio.ObterTabelaPorSgdp(sgdp));

            if (tabela == null || (tabela.Municipio == null && tabela.MunicipioReferente == null))
                throw new Exception("Erro ao encontrar");

            int idMunicipio = tabela.MunicipioReferente?.Codigo ?? tabela.Municipio.Codigo;
            try
            {
                tabela.OutrasInformacoes = ListarDadosOutrasInformacoes(tabela.SGDP, idMunicipio);

                tabela.OutrasInformacoes.ForEach(dado =>
                    dado.CuponsFiscaisVinculados = cupomFiscalRepositorio.ObterCuponsVinculados(tabela.SGDP, dado.NumeroNotaFiscal));

            }
#pragma warning disable CS0168 // A variável "ex" está declarada, mas nunca é usada
            catch (Exception ex)
#pragma warning restore CS0168 // A variável "ex" está declarada, mas nunca é usada
            {
                tabela.OutrasInformacoes = new List<OutrasInformacoesDto>();
            }

            return tabela;
        }

        public List<TabelaUsuarioDto> ListarTabelas()
        {
            return ConverterListaEntidadeParaDto(tabelaRepositorio.ListarTabelas());
        }

        public List<string> ListarSgdpsTabelas()
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

        public List<AnpxNotaFiscalDto> ListarDadosAnpXNotaFiscalPorSgdp(string sgdp, int idMunicipio)
        {
            return ConverterListaEntidadeDadosAnpxNotaParaDto(anpxNotaFiscalRepositorio.ListarNotasFiscaisPorSgdp(sgdp, idMunicipio));
        }

        public List<OutrasInformacoesDto> ListarDadosOutrasInformacoes(string sgdp, int idMunicipio)
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
                ValorFam = entidade.ValorFam,
                ValorMaximoAtualizado = entidade.ValorMaximoAtualizado,
                ValorMedioAtualizado = entidade.ValorMedioAtualizado,
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
                ValorMaximoAtualizado = entidade.ValorMaximoAtualizado,
                ValorMedioAtualizado = entidade.ValorMedioAtualizado,
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
