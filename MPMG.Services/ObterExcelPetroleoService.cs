using MPMG.Repositories;
using MPMG.Util;
using System;
using System.IO;

namespace MPMG.Services
{
    public class ObterExcelPetroleoService
    {

        private ObterArquivoRepositorio repositorioObterArquivo;

        public ObterExcelPetroleoService()
        {
            repositorioObterArquivo = new ObterArquivoRepositorio();
        }

        public byte[] ObterExcelPrecosCombustivel()
        {
            return repositorioObterArquivo.ObterArquivoPorUrl(Constantes.URL_AGENCIA_NACIONAL_PETROLEO_PRECOS_ESTADO);
        }

        public void ObterExcelPrecosCombustivelESalvar()
        {
            if(!File.Exists(string.Format("{0}/{1}", Constantes.CAMINHO_DOWNLOAD_ARQUIVO, Constantes.NOME_ARQUIVO_ANP_PRECOS)))
                repositorioObterArquivo.ObterArquivoPorUrlESalvar(Constantes.URL_AGENCIA_NACIONAL_PETROLEO_PRECOS_MUNICIPIOS_ATUAL);
        }
    }
}