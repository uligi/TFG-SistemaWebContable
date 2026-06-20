using System;

namespace CapaEntidad.Proveedores
{
    public class CorreoProveedor
    {
        public string IdentificacionProveedor { get; set; }

        public string RazonSocial { get; set; }

        public string DireccionCorreo { get; set; }

        // Se usa para editar cuando cambia el correo,
        // porque la llave primaria es compuesta:
        // IdentificacionProveedor + DireccionCorreo
        public string DireccionCorreoAnterior { get; set; }

        public int IdTipoCorreo { get; set; }
        public string TipoCorreoNombre { get; set; }

        public bool EsPrincipal { get; set; }

        public bool Activo { get; set; }
    }
}