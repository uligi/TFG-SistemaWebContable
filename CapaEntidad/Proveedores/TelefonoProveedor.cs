using System;

namespace CapaEntidad.Proveedores
{
    public class TelefonoProveedor
    {
        public string IdentificacionProveedor { get; set; }

        public string RazonSocial { get; set; }

        public string NumeroTelefono { get; set; }

        // Se usa para editar cuando cambia el número,
        // porque la llave primaria es compuesta:
        // IdentificacionProveedor + NumeroTelefono
        public string NumeroTelefonoAnterior { get; set; }

        public int IdTipoTelefono { get; set; }
        public string TipoTelefonoNombre { get; set; }

        public bool EsPrincipal { get; set; }

        public bool Activo { get; set; }
    }
}