using System;

namespace CapaEntidad.Seguridad
{
    public class HistorialCambioConsulta
    {
        public int IdHistorialCambio { get; set; }

        public string Identificacion { get; set; }

        public string NombreUsuario { get; set; }

        public string NombreEmpleado { get; set; }

        public string NombreTabla { get; set; }

        public int IdTipoDeObservacion { get; set; }

        public string TipoDeObservacion { get; set; }

        public DateTime FechaHoraCambio { get; set; }

        public string Detalle { get; set; }

        public string ValorAnterior { get; set; }

        public string ValorNuevo { get; set; }

        public string IdRegistro { get; set; }

        public bool Activo { get; set; }

        public string FechaHoraCambioTexto
        {
            get
            {
                return FechaHoraCambio.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public string EmpleadoDescripcion
        {
            get
            {
                return (Identificacion + " - " + NombreEmpleado).Trim();
            }
        }

        public string EstadoTexto
        {
            get
            {
                return Activo ? "Activo" : "Inactivo";
            }
        }
    }
}