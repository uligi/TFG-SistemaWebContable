using CapaDatos.Consultas;
using CapaEntidad.Consultas;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Consultas
{
    public class CN_Consultas
    {
        private CD_Consultas objCapaDato = new CD_Consultas();

        public List<ConsultaFactura> ConsultarFacturas(string filtro, DateTime? fechaInicio, DateTime? fechaFin, string estado)
        {
            filtro = PrepararFiltro(filtro);
            estado = PrepararTexto(estado);

            if (!ValidarTextoBusqueda(filtro))
            {
                return new List<ConsultaFactura>();
            }

            if (!ValidarEstado(estado))
            {
                return new List<ConsultaFactura>();
            }

            if (!ValidarRangoFechasOpcional(fechaInicio, fechaFin))
            {
                return new List<ConsultaFactura>();
            }

            return objCapaDato.ConsultarFacturas(filtro, fechaInicio, fechaFin, estado);
        }

        public List<ConsultaDetalleFactura> ConsultarDetalleFactura(string numeroFactura)
        {
            numeroFactura = PrepararCodigo(numeroFactura);

            if (numeroFactura == "")
            {
                return new List<ConsultaDetalleFactura>();
            }

            if (numeroFactura.Length > 45)
            {
                return new List<ConsultaDetalleFactura>();
            }

            if (!Regex.IsMatch(numeroFactura, @"^[0-9A-Za-z.\-_]+$"))
            {
                return new List<ConsultaDetalleFactura>();
            }

            return objCapaDato.ConsultarDetalleFactura(numeroFactura);
        }

        public List<ConsultaCuentaPorCobrar> ConsultarCuentasPorCobrar(string filtro, string estado, string estadoCredito)
        {
            filtro = PrepararFiltro(filtro);
            estado = PrepararTexto(estado);
            estadoCredito = PrepararTexto(estadoCredito);

            if (!ValidarTextoBusqueda(filtro))
            {
                return new List<ConsultaCuentaPorCobrar>();
            }

            if (!ValidarEstado(estado))
            {
                return new List<ConsultaCuentaPorCobrar>();
            }

            if (!ValidarEstadoCredito(estadoCredito))
            {
                return new List<ConsultaCuentaPorCobrar>();
            }

            return objCapaDato.ConsultarCuentasPorCobrar(filtro, estado, estadoCredito);
        }

        public List<ConsultaCuentaPorPagar> ConsultarCuentasPorPagar(string filtro, string estado, string estadoCredito)
        {
            filtro = PrepararFiltro(filtro);
            estado = PrepararTexto(estado);
            estadoCredito = PrepararTexto(estadoCredito);

            if (!ValidarTextoBusqueda(filtro))
            {
                return new List<ConsultaCuentaPorPagar>();
            }

            if (!ValidarEstado(estado))
            {
                return new List<ConsultaCuentaPorPagar>();
            }

            if (!ValidarEstadoCredito(estadoCredito))
            {
                return new List<ConsultaCuentaPorPagar>();
            }

            return objCapaDato.ConsultarCuentasPorPagar(filtro, estado, estadoCredito);
        }

        public List<ConsultaProducto> ConsultarProductos(string filtro, bool soloBajoStock, bool soloSinStock, bool soloActivos)
        {
            filtro = PrepararFiltro(filtro);

            if (!ValidarTextoBusqueda(filtro))
            {
                return new List<ConsultaProducto>();
            }

            if (soloSinStock)
            {
                soloBajoStock = false;
            }

            return objCapaDato.ConsultarProductos(filtro, soloBajoStock, soloSinStock, soloActivos);
        }

        public List<ConsultaCliente> ConsultarClientes(string filtro, bool soloConSaldo, bool soloActivos)
        {
            filtro = PrepararFiltro(filtro);

            if (!ValidarTextoBusqueda(filtro))
            {
                return new List<ConsultaCliente>();
            }

            return objCapaDato.ConsultarClientes(filtro, soloConSaldo, soloActivos);
        }

        public List<ConsultaProveedor> ConsultarProveedores(string filtro, bool soloConSaldo, bool soloActivos)
        {
            filtro = PrepararFiltro(filtro);

            if (!ValidarTextoBusqueda(filtro))
            {
                return new List<ConsultaProveedor>();
            }

            return objCapaDato.ConsultarProveedores(filtro, soloConSaldo, soloActivos);
        }

        public List<ConsultaMovimientoContable> ConsultarMovimientosContables(string filtro, DateTime? fechaInicio, DateTime? fechaFin, string tipoMovimiento)
        {
            filtro = PrepararFiltro(filtro);
            tipoMovimiento = PrepararTexto(tipoMovimiento);

            if (!ValidarTextoBusqueda(filtro))
            {
                return new List<ConsultaMovimientoContable>();
            }

            if (!ValidarTipoMovimiento(tipoMovimiento))
            {
                return new List<ConsultaMovimientoContable>();
            }

            if (!ValidarRangoFechasOpcional(fechaInicio, fechaFin))
            {
                return new List<ConsultaMovimientoContable>();
            }

            return objCapaDato.ConsultarMovimientosContables(filtro, fechaInicio, fechaFin, tipoMovimiento);
        }

        public bool ValidarFechasTexto(string fechaInicioTexto, string fechaFinTexto, out DateTime? fechaInicio, out DateTime? fechaFin, out string mensaje)
        {
            fechaInicio = null;
            fechaFin = null;
            mensaje = string.Empty;

            fechaInicioTexto = PrepararTexto(fechaInicioTexto);
            fechaFinTexto = PrepararTexto(fechaFinTexto);

            if (fechaInicioTexto != "")
            {
                DateTime fechaTemp;

                if (!DateTime.TryParse(fechaInicioTexto, out fechaTemp))
                {
                    mensaje = "La fecha de inicio no tiene un formato válido.";
                    return false;
                }

                if (fechaTemp.Year < 2000 || fechaTemp.Year > 2100)
                {
                    mensaje = "La fecha de inicio no es válida.";
                    return false;
                }

                fechaInicio = fechaTemp.Date;
            }

            if (fechaFinTexto != "")
            {
                DateTime fechaTemp;

                if (!DateTime.TryParse(fechaFinTexto, out fechaTemp))
                {
                    mensaje = "La fecha final no tiene un formato válido.";
                    return false;
                }

                if (fechaTemp.Year < 2000 || fechaTemp.Year > 2100)
                {
                    mensaje = "La fecha final no es válida.";
                    return false;
                }

                fechaFin = fechaTemp.Date;
            }

            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio.Value > fechaFin.Value)
            {
                mensaje = "La fecha de inicio no puede ser mayor que la fecha final.";
                return false;
            }

            return true;
        }

        private string PrepararFiltro(string texto)
        {
            texto = texto == null ? "" : texto.Trim();

            if (texto.Length > 100)
            {
                texto = texto.Substring(0, 100);
            }

            return texto;
        }

        private string PrepararTexto(string texto)
        {
            return texto == null ? "" : texto.Trim();
        }

        private string PrepararCodigo(string texto)
        {
            return texto == null ? "" : texto.Trim();
        }

        private bool ValidarTextoBusqueda(string texto)
        {
            if (texto == "")
            {
                return true;
            }

            if (texto.Length > 100)
            {
                return false;
            }

            return Regex.IsMatch(texto, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$");
        }

        private bool ValidarEstado(string estado)
        {
            if (estado == "")
            {
                return true;
            }

            if (estado.Length > 45)
            {
                return false;
            }

            return Regex.IsMatch(estado, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$");
        }

        private bool ValidarEstadoCredito(string estadoCredito)
        {
            if (estadoCredito == "")
            {
                return true;
            }

            if (estadoCredito == "Pendiente" ||
                estadoCredito == "Por vencer" ||
                estadoCredito == "Vencida" ||
                estadoCredito == "Pagada" ||
                estadoCredito == "Anulada")
            {
                return true;
            }

            return false;
        }

        private bool ValidarTipoMovimiento(string tipoMovimiento)
        {
            if (tipoMovimiento == "")
            {
                return true;
            }

            if (tipoMovimiento == "Ingreso" || tipoMovimiento == "Gasto")
            {
                return true;
            }

            return false;
        }

        private bool ValidarRangoFechasOpcional(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (fechaInicio.HasValue)
            {
                if (fechaInicio.Value.Year < 2000 || fechaInicio.Value.Year > 2100)
                {
                    return false;
                }
            }

            if (fechaFin.HasValue)
            {
                if (fechaFin.Value.Year < 2000 || fechaFin.Value.Year > 2100)
                {
                    return false;
                }
            }

            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio.Value > fechaFin.Value)
            {
                return false;
            }

            return true;
        }
    }
}