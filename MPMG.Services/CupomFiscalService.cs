using System;

namespace MPMG.Services
{
    public class CupomFiscalService
    {
        private readonly CupomFiscalRepo repo;
        public CupomFiscalService ()
        {
            repo = new CupomFiscalRepo ();
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
    }
}