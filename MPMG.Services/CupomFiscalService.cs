using MPMG.Interfaces.DTO;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPMG.Services
{
    public class CupomFiscalService
    {
        private readonly CupomFiscalRepo repo;
        public CupomFiscalService()
        {
            repo = new CupomFiscalRepo();
        }

        public void Cadastrar(int sGDP, int nrNotaFiscal, string cOO, string posto, DateTime data, string combustivel, int quantidade, double precoUnitario, double valorTotal, string cliente, int hodometro, string veiculo, string placaVeiculo)
        {
            repo.CadastrarCupomCompleto(
                sGDP,
                nrNotaFiscal,
                cOO,
                posto,
                data,
                combustivel,
                quantidade,
                precoUnitario,
                valorTotal,
                cliente,
                hodometro,
                veiculo,
                placaVeiculo);
        }

        public List<CupomFiscalDto> ListarCuponsFiscaisPorSgdp(string sgdp)
        {
            try
            {
                return ConverterListaEntidadeParaDto(repo.ListarCuponsPorSgdp(int.Parse(sgdp)));
            }
            catch (Exception ex)
            {
                return new List<CupomFiscalDto>();
            }
        }

        private CupomFiscalDto ConverterEntidadeParaDto(CupomFiscal entidade)
        {
            if (entidade == null)
                return null;

            return new CupomFiscalDto()
            {
                Cliente = entidade.Cliente,
                Coo = entidade.Coo,
                DataEmissao = entidade.DataEmissao,
                Hodometro = entidade.Hodometro,
                NumeroNotaFiscal = entidade.NumeroNotaFiscal,
                PlacaVeiculo = entidade.PlacaVeiculo,
                PostoReferente = entidade.PostoReferente,
                PrecoUnitario = entidade.PrecoUnitario,
                Produto = entidade.Produto,
                Quantidade = entidade.Quantidade,
                Sgdp = entidade.Sgdp,
                ValorTotal = entidade.ValorTotal,
                Veiculo = entidade.Veiculo
            };
        }

        private List<CupomFiscalDto> ConverterListaEntidadeParaDto(List<CupomFiscal> entidades)
        {
            if (entidades == null || !entidades.Any())
                return new List<CupomFiscalDto>();

            return entidades.Select(ConverterEntidadeParaDto).ToList();
        }
    }
}