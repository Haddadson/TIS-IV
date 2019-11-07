
namespace MPMG.Interfaces.DTO
{
    public class Municipio
    {
        public Municipio()
        {

        }

        public Municipio(int codigo, string nome)
        {
            Codigo = codigo;
            Nome = nome;
        }

        public int Codigo { get; set; }
        public string Nome { get; set; }
    }
}
