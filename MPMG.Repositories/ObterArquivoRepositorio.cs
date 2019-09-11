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
    }
}