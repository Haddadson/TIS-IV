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
        public NotaFiscalService()
        {
            notaFiscalRepo = new NotaFiscalRepo();
            cupomFiscalRepo = new CupomFiscalRepo();
            tabelaANPRepo = new TabelaANPRepo();
        }
        public void addNotaFiscal(
            int nrNotaFiscal,
            int sGDP,
            double valorTotal,
            string chaveAcesso,
            DateTime dataEmissao,
            double precoMaximo,
            double precoMedio,
            DateTime dataConsultaANP,
            string veiculo,
            string placaVeiculo,
            string combustivel,
            int quantidade,
            double precoUnitario,
            int numeroFolha,
            int departamento, 
            List<string> cuponsSelecionados)
        {
            int mesFAM = dataEmissao.Month;
            int anoFAM = dataEmissao.Year;

            TabelaANP tabela = tabelaANPRepo.BuscarDadosANP(sGDP, combustivel, dataConsultaANP.Month, dataConsultaANP.Year);

            if (tabela != null)
            {
                precoMaximo = tabela.precoMaximo;
                precoMedio = tabela.precoMedio;
            }

            notaFiscalRepo.Cadastrar(
                nrNotaFiscal,
                sGDP,
                valorTotal,
                chaveAcesso,
                dataEmissao,
                precoMaximo,
                precoMedio,
                dataConsultaANP,
                veiculo,
                placaVeiculo,
                combustivel,
                quantidade,
                precoUnitario,
                numeroFolha,
                departamento,
                mesFAM,
                anoFAM);

            foreach (var cupom in cuponsSelecionados)
            {
                cupomFiscalRepo.CadastrarCupom(sGDP, cupom);
            }
        }
    }
}
