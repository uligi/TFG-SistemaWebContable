using System;

namespace CapaEntidad.Seguridad
{
    public class ModuloSistema
    {
        public string CodigoModulo { get; set; }

        public string NombreModulo { get; set; }

        public string AreaSistema { get; set; }

        public string Controlador { get; set; }

        public string Accion { get; set; }

        public string Url { get; set; }

        public string Icono { get; set; }

        public int Orden { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public string EstadoTexto
        {
            get
            {
                return Activo ? "Activo" : "Inactivo";
            }
        }

        public string DescripcionModulo
        {
            get
            {
                return (CodigoModulo + " - " + NombreModulo).Trim();
            }
        }
    }
}