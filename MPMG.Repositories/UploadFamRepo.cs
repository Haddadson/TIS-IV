using MPMG.Repositories.Entidades;

namespace MPMG.Repositories
{
    public class UploadFamRepo : RepositorioBase<UploadAnp>
    {
        private const string SQL_OBTER_ULTIMO_UPLOAD = @"
            SELECT 
                IFNULL(MAX(id_upload), 0) AS Id,
                dt_upload AS DataUpload
            FROM `uploadtabelafam`
            GROUP BY dt_upload";

        private const string SQL_INSERT_UPLOAD = @"
            INSERT INTO `uploadtabelafam` VALUES ((SELECT IFNULL(MAX(A.id_upload), 0) + 1 FROM `uploadtabelafam` A), NOW())";

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
