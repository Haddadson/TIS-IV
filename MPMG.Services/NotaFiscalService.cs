using MPMG.Interfaces.DTO;
using MPMG.Repositories;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Services
{
    public class NotaFiscalService
    {
        private readonly NotaFiscalRepo notaFiscalRepo;
        private readonly CupomFiscalRepo cupomFiscalRepo;
        private readonly TabelaANPRepo tabelaANPRepo;
        private readonly ItemNotaFiscalRepo itemNotaFiscalRepo;


        public NotaFiscalService()
        {
            notaFiscalRepo = new NotaFiscalRepo();
            cupomFiscalRepo = new CupomFiscalRepo();
            tabelaANPRepo = new TabelaANPRepo();
            itemNotaFiscalRepo = new ItemNotaFiscalRepo();
        }
        public void CadastrarNotaFiscal(
            string nrNotaFiscal,
            string sGDP,
            double valorTotal,
            string chaveAcesso,
            DateTime dataEmissao,
            DateTime dataConsultaANP,
            string veiculo,
            string placaVeiculo,
            int numeroFolha,
            int departamento,
            List<string> cuponsSelecionados,
            List<ItemNotaFiscalDto> ItensNotaFiscal)
        {
            int mesFAM = dataEmissao.Month;
            int anoFAM = dataEmissao.Year;

            notaFiscalRepo.Cadastrar(nrNotaFiscal, sGDP, valorTotal,
                chaveAcesso, dataEmissao, dataConsultaANP,
                veiculo, placaVeiculo, numeroFolha,
                departamento, mesFAM, anoFAM);
 
            foreach (var item in ItensNotaFiscal)
            {
                if(item != null)
                {

                    //TabelaANP tabela = tabelaANPRepo.BuscarDadosANP(sGDP, item.Produto, dataConsultaANP.Month, dataConsultaANP.Year);

                    //if (tabela != null)
                    //{
                    //    precoMaximo = tabela.precoMaximo;
                    //    precoMedio = tabela.precoMedio;
                    //}

                    itemNotaFiscalRepo.Cadastrar(nrNotaFiscal, item.Sgdp, item.Produto, 
                        item.Quantidade, item.ValorTotal, item.ValorUnitario);
                }
            }

            foreach (var cupom in cuponsSelecionados)
            {
                var cupomSalvo = cupomFiscalRepo.buscarCupomDisponivel(sGDP, cupom);
                if (cupomSalvo == null)
                {
                    cupomFiscalRepo.CadastrarCupom(sGDP, cupom, nrNotaFiscal); // Insere um novo Cupom.
                } else 
                {
                    cupomFiscalRepo.EditarCupom(sGDP, cupom, nrNotaFiscal); // Adicionamos o Número da Nota Fiscal ao Cupom já cadastrado.
                }
            }
        }
    }
}
