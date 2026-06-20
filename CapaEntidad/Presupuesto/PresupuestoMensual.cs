using System;

namespace CapaEntidad.Presupuesto
{
    public class PresupuestoMensual
    {
        public int Anio { get; set; }

        public int Mes { get; set; }

        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        // Campo auxiliar del SP
        public string MesNombre { get; set; }

        public string PeriodoDescripcion
        {
            get
            {
                return MesNombre + " " + Anio.ToString();
            }
        }

        public string PeriodoCodigo
        {
            get
            {
                return Anio.ToString() + "-" + Mes.ToString("00");
            }
        }

        public bool EstaAbierto
        {
            get
            {
                return Estado != null &&
                       Estado.Trim().ToLower() == "abierto" &&
                       Activo;
            }
        }

        public bool EstaCerrado
        {
            get
            {
                return Estado != null &&
                       Estado.Trim().ToLower() == "cerrado";
            }
        }

        public bool EstaInactivo
        {
            get
            {
                return !Activo ||
                       (
                           Estado != null &&
                           Estado.Trim().ToLower() == "inactivo"
                       );
            }
        }

        public string EstadoDescripcion
        {
            get
            {
                if (EstaAbierto)
                {
                    return "Abierto";
                }

                if (EstaCerrado)
                {
                    return "Cerrado";
                }

                if (EstaInactivo)
                {
                    return "Inactivo";
                }

                return Estado;
            }
        }
    }
}