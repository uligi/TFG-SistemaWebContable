using CapaEntidad.Contabilidad;
using CapaEntidad.Cuentas;
using CapaEntidad.CuentasPorPagar;
using CapaEntidad.Movimientos;
using System;



namespace CapaNegocio.Contabilidad
{
    public class CN_AsientoAutomatico
    {
        private readonly CN_AsientoContable cnAsiento = new CN_AsientoContable();
        private readonly CN_DetalleAsientoContable cnDetalle = new CN_DetalleAsientoContable();
        private readonly CN_ConfiguracionCuentaContable cnConfiguracion = new CN_ConfiguracionCuentaContable();
        private readonly CN_PeriodoContable cnPeriodo = new CN_PeriodoContable();
        private readonly CN_ReferenciaAsientoContable cnReferencia = new CN_ReferenciaAsientoContable();

        private bool ValidarPeriodoAbierto(DateTime fecha, out string mensaje)
        {
            mensaje = string.Empty;

            var periodo = cnPeriodo.Obtener(fecha.Year, fecha.Month);

            if (periodo == null)
            {
                mensaje = "No existe un período contable para la fecha indicada.";
                return false;
            }

            if (!periodo.Activo)
            {
                mensaje = "El período contable está inactivo.";
                return false;
            }

            if (!periodo.EstaAbierto)
            {
                mensaje = "El período contable está cerrado.";
                return false;
            }

            return true;
        }

        private string ObtenerCuentaConfigurada(string codigoOperacion, out string mensaje)
        {
            mensaje = string.Empty;

            var config = cnConfiguracion.ObtenerActivaPorOperacion(codigoOperacion);

            if (config == null)
            {
                mensaje = "No existe configuración contable activa para la operación: " + codigoOperacion;
                return string.Empty;
            }

            if (!config.ConfiguracionValida)
            {
                mensaje = "La configuración contable no es válida para la operación: " + codigoOperacion;
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(config.CodigoCuenta))
            {
                mensaje = "La operación " + codigoOperacion + " no tiene cuenta contable asociada.";
                return string.Empty;
            }

            return config.CodigoCuenta;
        }

        private bool CrearAsientoSimple(
            DateTime fecha,
            string concepto,
            string codigoCuentaDebe,
            string codigoCuentaHaber,
            decimal monto,
            string moduloOrigen,
            string documentoOrigen,
            string tipoMovimiento,
            out string mensaje,
            out string numeroAsientoGenerado
        )
        {
            mensaje = string.Empty;
            numeroAsientoGenerado = string.Empty;

            if (monto <= 0)
            {
                mensaje = "El monto del asiento debe ser mayor a cero.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(moduloOrigen))
            {
                mensaje = "Debe indicar el módulo de origen del asiento.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(documentoOrigen))
            {
                mensaje = "Debe indicar el documento de origen del asiento.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(tipoMovimiento))
            {
                mensaje = "Debe indicar el tipo de movimiento del asiento.";
                return false;
            }

            string asientoExistente = string.Empty;

            bool yaExiste = cnReferencia.Existe(
                moduloOrigen,
                documentoOrigen,
                tipoMovimiento,
                out asientoExistente
            );

            if (yaExiste)
            {
                numeroAsientoGenerado = asientoExistente;
                mensaje = "Ya existe un asiento contable para este movimiento: " + asientoExistente + ".";
                return true;
            }

            if (!ValidarPeriodoAbierto(fecha, out mensaje))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(codigoCuentaDebe))
            {
                mensaje = "Debe indicar la cuenta contable del Debe.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(codigoCuentaHaber))
            {
                mensaje = "Debe indicar la cuenta contable del Haber.";
                return false;
            }

            if (concepto.Length > 150)
            {
                concepto = concepto.Substring(0, 150);
            }

            AsientoContable asiento = new AsientoContable
            {
                Anio = fecha.Year,
                Mes = fecha.Month,
                FechaAsiento = fecha,
                TipoAsiento = "Automatico",
                Concepto = concepto,
                TotalDebe = 0,
                TotalHaber = 0,
                Activo = true
            };

            int resultadoAsiento = cnAsiento.Registrar(
                asiento,
                out mensaje,
                out numeroAsientoGenerado
            );

            if (resultadoAsiento == 0 || string.IsNullOrWhiteSpace(numeroAsientoGenerado))
            {
                return false;
            }

            int numeroLineaGenerada = 0;

            DetalleAsientoContable lineaDebe = new DetalleAsientoContable
            {
                NumeroAsiento = numeroAsientoGenerado,
                NumeroLinea = 0,
                CodigoCuenta = codigoCuentaDebe,
                Debe = monto,
                Haber = 0,
                DescripcionLinea = concepto,
                Activo = true
            };

            int resultadoDebe = cnDetalle.Registrar(
                lineaDebe,
                out mensaje,
                out numeroLineaGenerada
            );

            if (resultadoDebe == 0)
            {
                return false;
            }

            DetalleAsientoContable lineaHaber = new DetalleAsientoContable
            {
                NumeroAsiento = numeroAsientoGenerado,
                NumeroLinea = 0,
                CodigoCuenta = codigoCuentaHaber,
                Debe = 0,
                Haber = monto,
                DescripcionLinea = concepto,
                Activo = true
            };

            int resultadoHaber = cnDetalle.Registrar(
                lineaHaber,
                out mensaje,
                out numeroLineaGenerada
            );

            if (resultadoHaber == 0)
            {
                return false;
            }

            bool recalculado = cnAsiento.RecalcularTotales(
                numeroAsientoGenerado,
                out mensaje
            );

            if (!recalculado)
            {
                return false;
            }

            bool cuadrado = cnAsiento.ValidarCuadre(
                numeroAsientoGenerado,
                out mensaje
            );

            if (!cuadrado)
            {
                return false;
            }

            ReferenciaAsientoContable referencia = new ReferenciaAsientoContable
            {
                NumeroAsiento = numeroAsientoGenerado,
                ModuloOrigen = moduloOrigen,
                DocumentoOrigen = documentoOrigen,
                TipoMovimiento = tipoMovimiento,
                Activo = true
            };

            int resultadoReferencia = cnReferencia.Registrar(
                referencia,
                out mensaje
            );

            if (resultadoReferencia == 0)
            {
                return false;
            }

            mensaje = "Asiento contable automático generado correctamente.";
            return true;
        }

        public bool GenerarPorIngreso(Ingreso obj, out string mensaje, out string numeroAsientoGenerado)
        {
            mensaje = string.Empty;
            numeroAsientoGenerado = string.Empty;

            if (obj == null)
            {
                mensaje = "No se recibió información del ingreso.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroIngreso))
            {
                mensaje = "El ingreso debe tener número generado antes de crear el asiento.";
                return false;
            }

            if (obj.Monto <= 0)
            {
                mensaje = "El monto del ingreso debe ser mayor a cero.";
                return false;
            }

            string cuentaCaja = ObtenerCuentaConfigurada("CAJA_BANCOS", out mensaje);

            if (string.IsNullOrWhiteSpace(cuentaCaja))
            {
                return false;
            }

            string cuentaIngreso = obj.CodigoCuenta;

            if (string.IsNullOrWhiteSpace(cuentaIngreso))
            {
                cuentaIngreso = ObtenerCuentaConfigurada("INGRESOS_VARIOS", out mensaje);

                if (string.IsNullOrWhiteSpace(cuentaIngreso))
                {
                    return false;
                }
            }

            string concepto = "Ingreso " + obj.NumeroIngreso;

            if (!string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                concepto = concepto + " - " + obj.Descripcion;
            }

            return CrearAsientoSimple(
                obj.FechaIngreso,
                concepto,
                cuentaCaja,
                cuentaIngreso,
                obj.Monto,
                "INGRESOS",
                obj.NumeroIngreso,
                "REGISTRO",
                out mensaje,
                out numeroAsientoGenerado
            );
        }

        public bool GenerarPorGasto(Gasto obj, out string mensaje, out string numeroAsientoGenerado)
        {
            mensaje = string.Empty;
            numeroAsientoGenerado = string.Empty;

            if (obj == null)
            {
                mensaje = "No se recibió información del gasto.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroGasto))
            {
                mensaje = "El gasto debe tener número generado antes de crear el asiento.";
                return false;
            }

            if (obj.Monto <= 0)
            {
                mensaje = "El monto del gasto debe ser mayor a cero.";
                return false;
            }

            string cuentaCaja = ObtenerCuentaConfigurada("CAJA_BANCOS", out mensaje);

            if (string.IsNullOrWhiteSpace(cuentaCaja))
            {
                return false;
            }

            string cuentaGasto = obj.CodigoCuenta;

            if (string.IsNullOrWhiteSpace(cuentaGasto))
            {
                cuentaGasto = ObtenerCuentaConfigurada("GASTOS_OPERATIVOS", out mensaje);

                if (string.IsNullOrWhiteSpace(cuentaGasto))
                {
                    return false;
                }
            }

            string concepto = "Gasto " + obj.NumeroGasto;

            if (!string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                concepto = concepto + " - " + obj.Descripcion;
            }

            return CrearAsientoSimple(
                obj.FechaGasto,
                concepto,
                cuentaGasto,
                cuentaCaja,
                obj.Monto,
                "GASTOS",
                obj.NumeroGasto,
                "REGISTRO",
                out mensaje,
                out numeroAsientoGenerado
            );
        }


        public bool GenerarPorAbonoCuentaPorCobrar(AbonoCuentaPorCobrar obj, out string mensaje, out string numeroAsientoGenerado)
        {
            mensaje = string.Empty;
            numeroAsientoGenerado = string.Empty;

            if (obj == null)
            {
                mensaje = "No se recibió información del abono de cuenta por cobrar.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroFactura))
            {
                mensaje = "El abono debe tener número de factura asociado.";
                return false;
            }

            if (obj.NumeroAbono <= 0)
            {
                mensaje = "El abono debe tener número de abono generado antes de crear el asiento.";
                return false;
            }

            if (obj.MontoAbono <= 0)
            {
                mensaje = "El monto del abono debe ser mayor a cero.";
                return false;
            }

            string cuentaCaja = ObtenerCuentaConfigurada("CAJA_BANCOS", out mensaje);

            if (string.IsNullOrWhiteSpace(cuentaCaja))
            {
                return false;
            }

            string cuentaCxC = ObtenerCuentaConfigurada("CUENTAS_POR_COBRAR", out mensaje);

            if (string.IsNullOrWhiteSpace(cuentaCxC))
            {
                return false;
            }

            string documentoOrigen = obj.NumeroFactura + "-ABONO-" + obj.NumeroAbono;

            string concepto = "Abono CxC " + documentoOrigen;

            return CrearAsientoSimple(
                obj.FechaAbono,
                concepto,
                cuentaCaja,
                cuentaCxC,
                obj.MontoAbono,
                "CUENTAS_POR_COBRAR",
                documentoOrigen,
                "ABONO",
                out mensaje,
                out numeroAsientoGenerado
            );
        }

        public bool GenerarPorAbonoCuentaPorPagar(AbonoCuentaPorPagar obj, out string mensaje, out string numeroAsientoGenerado)
        {
            mensaje = string.Empty;
            numeroAsientoGenerado = string.Empty;

            if (obj == null)
            {
                mensaje = "No se recibió información del abono de cuenta por pagar.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroDocumento))
            {
                mensaje = "El abono debe tener número de documento asociado.";
                return false;
            }

            if (obj.NumeroAbono <= 0)
            {
                mensaje = "El abono debe tener número de abono generado antes de crear el asiento.";
                return false;
            }

            if (obj.MontoAbono <= 0)
            {
                mensaje = "El monto del abono debe ser mayor a cero.";
                return false;
            }

            string cuentaCxP = ObtenerCuentaConfigurada("CUENTAS_POR_PAGAR", out mensaje);

            if (string.IsNullOrWhiteSpace(cuentaCxP))
            {
                return false;
            }

            string cuentaCaja = ObtenerCuentaConfigurada("CAJA_BANCOS", out mensaje);

            if (string.IsNullOrWhiteSpace(cuentaCaja))
            {
                return false;
            }

            string documentoOrigen = obj.NumeroDocumento + "-ABONO-" + obj.NumeroAbono;

            string concepto = "Abono CxP " + documentoOrigen;

            return CrearAsientoSimple(
                obj.FechaAbono,
                concepto,
                cuentaCxP,
                cuentaCaja,
                obj.MontoAbono,
                "CUENTAS_POR_PAGAR",
                documentoOrigen,
                "ABONO",
                out mensaje,
                out numeroAsientoGenerado
            );
        }

        public bool GenerarPorCuentaPorPagar(CuentaPorPagar obj, out string mensaje, out string numeroAsientoGenerado)
        {
            mensaje = string.Empty;
            numeroAsientoGenerado = string.Empty;

            if (obj == null)
            {
                mensaje = "No se recibió información de la cuenta por pagar.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroDocumento))
            {
                mensaje = "La cuenta por pagar debe tener número de documento.";
                return false;
            }

            if (obj.MontoOriginal <= 0)
            {
                mensaje = "El monto original de la cuenta por pagar debe ser mayor a cero.";
                return false;
            }

            string cuentaGasto = ObtenerCuentaConfigurada("GASTOS_OPERATIVOS", out mensaje);

            if (string.IsNullOrWhiteSpace(cuentaGasto))
            {
                return false;
            }

            string cuentaCxP = ObtenerCuentaConfigurada("CUENTAS_POR_PAGAR", out mensaje);

            if (string.IsNullOrWhiteSpace(cuentaCxP))
            {
                return false;
            }

            string concepto = "Cuenta por pagar " + obj.NumeroDocumento;

            if (!string.IsNullOrWhiteSpace(obj.Concepto))
            {
                concepto = concepto + " - " + obj.Concepto;
            }

            return CrearAsientoSimple(
                obj.FechaEmision,
                concepto,
                cuentaGasto,
                cuentaCxP,
                obj.MontoOriginal,
                "CUENTAS_POR_PAGAR",
                obj.NumeroDocumento,
                "REGISTRO",
                out mensaje,
                out numeroAsientoGenerado
            );
        }
    }
}