using System;

namespace CapaEntidad.Contabilidad
{
    public class PeriodoContable
    {
        public int Anio { get; set; }

        public int Mes { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string Estado { get; set; }

        public bool Activo { get; set; }

        public string NombreMes
        {
            get
            {
                switch (Mes)
                {
                    case 1: return "Enero";
                    case 2: return "Febrero";
                    case 3: return "Marzo";
                    case 4: return "Abril";
                    case 5: return "Mayo";
                    case 6: return "Junio";
                    case 7: return "Julio";
                    case 8: return "Agosto";
                    case 9: return "Septiembre";
                    case 10: return "Octubre";
                    case 11: return "Noviembre";
                    case 12: return "Diciembre";
                    default: return "";
                }
            }
        }

        public string Periodo
        {
            get
            {
                return NombreMes + " " + Anio.ToString();
            }
        }

        public bool EstaAbierto
        {
            get
            {
                return Estado != null && Estado.Trim().ToLower() == "abierto";
            }
        }

        public bool EstaCerrado
        {
            get
            {
                return Estado != null && Estado.Trim().ToLower() == "cerrado";
            }
        }
    }
}