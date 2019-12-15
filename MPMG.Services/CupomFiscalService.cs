using MPMG.Interfaces.DTO;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPMG.Services
{
    public class CupomFiscalService
    {
        private readonly CupomFiscalRepo cupomFiscalRepo;

        public CupomFiscalService()
        {
            cupomFiscalRepo = new CupomFiscalRepo();
        }

        public void Cadastrar(string sGDP, string nrNotaFiscal, string cOO, string posto, DateTime data, string combustivel, int quantidade, double precoUnitario, double valorTotal, string cliente, int hodometro, string veiculo, string placaVeiculo)
        {
            var cupomSalvo = cupomFiscalRepo.buscarCupom(sGDP, cOO);
            if (cupomSalvo == null)
            {
                cupomFiscalRepo.CadastrarCupomCompleto(
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
            else
            {
                cupomFiscalRepo.EditarCupomCompleto(
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

        }

        public List<string> ListarCuponsDisponiveisPorSgdp(string sgdp)
        {
            try
            {
                return cupomFiscalRepo.ListarCuponsDisponiveisPorSgdp(sgdp).Select(item => item.Coo).ToList();
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        public List<CupomFiscalDto> ListarCuponsFiscaisPorSgdp(string sgdp)
        {
            try
            {
                return ConverterListaEntidadeParaDto(cupomFiscalRepo.ListarCuponsPorSgdp(sgdp));
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
                Coo = entidade.Coo,
                DataEmissao = entidade.DataEmissao,
                Hodometro = entidade.Hodometro,
                NumeroNotaFiscal = entidade.NumeroNotaFiscal,
                PlacaVeiculo = entidade.PlacaVeiculo,
                PrecoUnitario = entidade.PrecoUnitario,
                Produto = entidade.Produto,
                Quantidade = entidade.Quantidade,
                Sgdp = entidade.Sgdp,
                ValorTotal = entidade.ValorTotal,
                Veiculo = entidade.Veiculo
            };
        }

        public CupomFiscalInfoDto ObterInfoCuponsFiscais(string valorSgdp)
        {
            CupomFiscalInfoDto cuponsInfo = new CupomFiscalInfoDto();

            cuponsInfo.Clientes = cupomFiscalRepo.ListarClientes(valorSgdp);
            cuponsInfo.NotasFiscais = cupomFiscalRepo.ListarNotasFiscais(valorSgdp);
            cuponsInfo.Postos = cupomFiscalRepo.ListarPostos(valorSgdp);
                cuponsInfo.PrecosUnitarios = cupomFiscalRepo.ListarPrecos(valorSgdp);
            cuponsInfo.CuponsFiscais = cupomFiscalRepo.ListarCuponsFiscais(valorSgdp).Select(item => ConverterEntidadeParaDto(item)).ToList();

            return cuponsInfo;
        }

        private List<CupomFiscalDto> ConverterListaEntidadeParaDto(List<CupomFiscal> entidades)
        {
            if (entidades == null || !entidades.Any())
                return new List<CupomFiscalDto>();

            return entidades.Select(ConverterEntidadeParaDto).ToList();
        }
    }
}