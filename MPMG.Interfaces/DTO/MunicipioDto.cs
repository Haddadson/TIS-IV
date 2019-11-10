
namespace MPMG.Interfaces.DTO
{
    public class MunicipioDto
    {
        public MunicipioDto()
        {

        }

        public MunicipioDto(int codigo, string nome)
        {
            Codigo = codigo;
            Nome = nome;
        }

        public int Codigo { get; set; }
        public string Nome { get; set; }
    }
}
