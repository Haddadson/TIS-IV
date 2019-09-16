namespace MPMG.Util.Enum
{
    public static class DeParaCombustivelMpmgAnp
    {
        public static string ConverterCombustivelMpmgAnp(string combustivelMpmg)
        {
            switch (combustivelMpmg)
            {
                case "Gasolina Comum":
                    return "GASOLINA COMUM";
                case "Oleo Diesel Comum":
                    return "ÓLEO DIESEL";
                case "Diesel S-10":
                    return "ÓLEO DIESEL S10";
                case "Etanol":
                    return "ETANOL HIDRATADO";
                default:
                    return string.Empty;
            }
        }
    }
}