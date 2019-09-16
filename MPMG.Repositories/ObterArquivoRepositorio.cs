using MPMG.Util;
using System;
using System.Net;

namespace MPMG.Repositories
{
    public class ObterArquivoRepositorio
    {
        public byte[] ObterArquivoPorUrl(string urlDownload)
        {
            byte[] arquivoAgencia;
            using (var client = new WebClient())
            {
                arquivoAgencia = client.DownloadData(urlDownload);
            }
            return arquivoAgencia;
        }

        public void ObterArquivoPorUrlESalvar(string urlDownload)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(urlDownload, string.Format("{0}/{1}", Constantes.CAMINHO_DOWNLOAD_ARQUIVO, Constantes.NOME_ARQUIVO_ANP_PRECOS));
            }
        }
    }
}