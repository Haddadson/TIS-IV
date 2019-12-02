namespace MPMG.Repositories.Entidades
{
    public class TabelaANP
    {
        public int municipio{ get; set; }
        public int mes { get; set; }
        public int ano { get; set; }
        public string produto { get; set; }
        public double precoMedio { get; set; }
        public double precoMaximo { get; set; }
    }
}