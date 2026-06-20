using CapaDatos.Movimientos;
using CapaEntidad.Movimientos;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Movimientos
{
    public class CN_Gasto
    {
        private CD_Gasto objCapaDato = new CD_Gasto();

        public List<Gasto> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Gasto> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public Gasto Obtener(string numeroGasto)
        {
            numeroGasto = numeroGasto == null ? "" : numeroGasto.Trim();

            if (numeroGasto == "")
            {
                return null;
            }

            if (numeroGasto.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(numeroGasto);
        }

        public int Registrar(Gasto obj, out string Mensaje, out string NumeroGastoGenerado)
        {
            Mensaje = string.Empty;
            NumeroGastoGenerado = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out NumeroGastoGenerado);
        }

        public bool Editar(Gasto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, true);

            if (error != "")
            {
                Mensaje = error;
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(string numeroGasto, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroGasto = numeroGasto == null ? "" : numeroGasto.Trim();

            if (numeroGasto == "")
            {
                Mensaje = "Debe seleccionar un gasto válido.";
                return false;
            }

            if (numeroGasto.Length > 45)
            {
                Mensaje = "El número de gasto no puede superar los 45 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(numeroGasto, @"^[0-9A-Za-z.\-_]+$"))
            {
                Mensaje = "El número de gasto solo puede contener letras, números, puntos, guiones y guion bajo.";
                return false;
            }

            return objCapaDato.Inactivar(numeroGasto, out Mensaje);
        }

        private void PrepararDatos(Gasto obj)
        {
            obj.NumeroGasto = obj.NumeroGasto == null ? "" : obj.NumeroGasto.Trim();

            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            obj.TipoGastoNombre = obj.TipoGastoNombre == null ? "" : obj.TipoGastoNombre.Trim();

            obj.CodigoCuenta = obj.CodigoCuenta == null ? "" : obj.CodigoCuenta.Trim();
            obj.NombreCuenta = obj.NombreCuenta == null ? "" : obj.NombreCuenta.Trim();

            obj.IdentificacionProveedor = obj.IdentificacionProveedor == null ? "" : obj.IdentificacionProveedor.Trim();
            obj.NumeroDocumento = obj.NumeroDocumento == null ? "" : obj.NumeroDocumento.Trim();
            obj.ProveedorNombre = obj.ProveedorNombre == null ? "" : obj.ProveedorNombre.Trim();

            obj.ConceptoCuentaPorPagar = obj.ConceptoCuentaPorPagar == null ? "" : obj.ConceptoCuentaPorPagar.Trim();
            obj.EstadoCuentaPorPagar = obj.EstadoCuentaPorPagar == null ? "" : obj.EstadoCuentaPorPagar.Trim();

            obj.IdentificacionProveedorAbono = obj.IdentificacionProveedorAbono == null ? "" : obj.IdentificacionProveedorAbono.Trim();
            obj.NumeroDocumentoAbono = obj.NumeroDocumentoAbono == null ? "" : obj.NumeroDocumentoAbono.Trim();

            obj.NumeroComprobante = obj.NumeroComprobante == null ? "" : obj.NumeroComprobante.Trim();
            obj.NombreArchivoComprobante = obj.NombreArchivoComprobante == null ? "" : obj.NombreArchivoComprobante.Trim();
            obj.RutaComprobante = obj.RutaComprobante == null ? "" : obj.RutaComprobante.Trim();

            if (obj.FechaGasto == DateTime.MinValue)
            {
                obj.FechaGasto = DateTime.Now;
            }

            if (obj.Monto < 0)
            {
                obj.Monto = 0;
            }

            if (obj.MontoOriginal < 0)
            {
                obj.MontoOriginal = 0;
            }

            if (obj.SaldoActual < 0)
            {
                obj.SaldoActual = 0;
            }

            if (obj.MontoAbono < 0)
            {
                obj.MontoAbono = 0;
            }

            if (obj.NumeroAbonoCuentaPorPagar < 0)
            {
                obj.NumeroAbonoCuentaPorPagar = 0;
            }

            if (obj.IdentificacionProveedor == "")
            {
                obj.IdentificacionProveedor = "000000000";
            }

            if (obj.NumeroDocumento == "")
            {
                obj.NumeroDocumento = "CXP-GENERAL-000000";
            }

            if (obj.IdentificacionProveedorAbono == "")
            {
                obj.IdentificacionProveedorAbono = "000000000";
            }

            if (obj.NumeroDocumentoAbono == "")
            {
                obj.NumeroDocumentoAbono = "CXP-GENERAL-000000";
            }
        }

        private string Validar(Gasto obj, bool esEdicion)
        {
            if (esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.NumeroGasto))
                {
                    return "Debe seleccionar un gasto válido.";
                }

                if (obj.NumeroGasto.Length > 45)
                {
                    return "El número de gasto no puede superar los 45 caracteres.";
                }

                if (!Regex.IsMatch(obj.NumeroGasto, @"^[0-9A-Za-z.\-_]+$"))
                {
                    return "El número de gasto solo puede contener letras, números, puntos, guiones y guion bajo.";
                }
            }

            if (obj.FechaGasto == DateTime.MinValue)
            {
                return "Debe ingresar la fecha del gasto.";
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                return "Debe ingresar una descripción.";
            }

            if (obj.Descripcion.Length > 150)
            {
                return "La descripción no puede superar los 150 caracteres.";
            }

            if (!Regex.IsMatch(obj.Descripcion, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "La descripción solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.IdTipoGasto <= 0)
            {
                return "Debe seleccionar un tipo de gasto.";
            }

            if (string.IsNullOrWhiteSpace(obj.CodigoCuenta))
            {
                return "Debe seleccionar una cuenta contable.";
            }

            if (obj.CodigoCuenta.Length > 45)
            {
                return "El código de cuenta no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.CodigoCuenta, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El código de cuenta solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (obj.Monto <= 0 && obj.Activo)
            {
                return "El monto del gasto debe ser mayor a cero.";
            }

            if (obj.IdentificacionProveedor.Length > 45)
            {
                return "La identificación del proveedor no puede superar los 45 caracteres.";
            }

            if (obj.NumeroDocumento.Length > 45)
            {
                return "El número de documento no puede superar los 45 caracteres.";
            }

            if (obj.IdentificacionProveedorAbono.Length > 45)
            {
                return "La identificación del proveedor del abono no puede superar los 45 caracteres.";
            }

            if (obj.NumeroDocumentoAbono.Length > 45)
            {
                return "El número de documento del abono no puede superar los 45 caracteres.";
            }

            if (obj.NumeroAbonoCuentaPorPagar < 0)
            {
                return "El número de abono no puede ser negativo.";
            }

            if (obj.NumeroComprobante.Length > 45)
            {
                return "El número de comprobante no puede superar los 45 caracteres.";
            }

            if (obj.NombreArchivoComprobante.Length > 150)
            {
                return "El nombre del archivo comprobante no puede superar los 150 caracteres.";
            }

            if (obj.RutaComprobante.Length > 300)
            {
                return "La ruta del comprobante no puede superar los 300 caracteres.";
            }

            if (obj.NumeroComprobante != "" &&
                !Regex.IsMatch(obj.NumeroComprobante, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El número de comprobante solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (obj.NombreArchivoComprobante != "" &&
                !Regex.IsMatch(obj.NombreArchivoComprobante, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&().]+$"))
            {
                return "El nombre del archivo comprobante contiene caracteres no permitidos.";
            }

            if (obj.RutaComprobante != "" &&
                !Regex.IsMatch(obj.RutaComprobante, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,:#\-_/\\&().]+$"))
            {
                return "La ruta del comprobante contiene caracteres no permitidos.";
            }

            return "";
        }
    }
}