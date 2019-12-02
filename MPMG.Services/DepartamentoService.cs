using MPMG.Repositories;
using MPMG.Repositories.Entidades;
using System.Collections.Generic;

namespace MPMG.Services
{
    public class DepartamentoService
    {
        private readonly DepartamentoRepo dptoRepo;

        public DepartamentoService()
        {
            dptoRepo = new DepartamentoRepo();
        }

        public List<object> ListarDepartamentos()
        {
            return dptoRepo.ObterDepartamento();
        }
    }
}
