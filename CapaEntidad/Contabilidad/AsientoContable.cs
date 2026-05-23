using System;

namespace CapaEntidad.Contabilidad
{
    public class AsientoContable
    {
        public int IdAsientoContable { get; set; }

        public int IdPeriodoContable { get; set; }

        public int Anio { get; set; }
        public int Mes { get; set; }
        public string EstadoPeriodo { get; set; }

        public string NumeroAsiento { get; set; }
        public DateTime FechaAsiento { get; set; }

        public string TipoAsiento { get; set; }
        public string Concepto { get; set; }

        public decimal TotalDebe { get; set; }
        public decimal TotalHaber { get; set; }

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
                return Anio.ToString() + " - " + NombreMes;
            }
        }

        public bool EstaCuadrado
        {
            get
            {
                return TotalDebe == TotalHaber && TotalDebe > 0;
            }
        }

        public decimal Diferencia
        {
            get
            {
                return TotalDebe - TotalHaber;
            }
        }
    }
}