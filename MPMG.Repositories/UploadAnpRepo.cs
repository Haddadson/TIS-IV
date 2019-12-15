using MPMG.Repositories.Entidades;

namespace MPMG.Repositories
{
    public class UploadAnpRepo : RepositorioBase<UploadAnp>
    {
        private const string SQL_OBTER_ULTIMO_UPLOAD = @"
            SELECT 
                IFNULL(MAX(id_upload_anp), 0) AS Id,
                data AS DataUpload
            FROM `uploadanp`
            GROUP BY data";

        private const string SQL_INSERT_UPLOAD = @"
            INSERT INTO `uploadanp` VALUES ((SELECT IFNULL(MAX(A.id_upload_anp), 0) + 1 FROM `uploadanp` A), NOW())";

        public void InserirNovaData()
        {
            Execute(SQL_INSERT_UPLOAD, null);
        }

        public UploadAnp ObterUltimoUpload()
        {
            return Obter(SQL_OBTER_ULTIMO_UPLOAD, null);
        }

    }
}
