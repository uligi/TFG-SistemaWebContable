IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Facturacion')
BEGIN
    EXEC('CREATE SCHEMA Facturacion');
END;
GO

/* =========================================================
   FACTURA - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.NumeroFactura,
        f.IdentificacionCliente,
        LTRIM(RTRIM(ISNULL(pc.Nombre, '') + ' ' + ISNULL(pc.PrimerApellido, '') + ' ' + ISNULL(pc.SegundoApellido, ''))) AS ClienteNombre,
        f.IdentificacionEmpleado,
        LTRIM(RTRIM(ISNULL(pe.Nombre, '') + ' ' + ISNULL(pe.PrimerApellido, '') + ' ' + ISNULL(pe.SegundoApellido, ''))) AS EmpleadoNombre,
        f.IdTipoPago,
        tp.Nombre AS TipoPagoNombre,
        f.IdTipoFactura,
        tf.TipoFactura AS TipoFacturaNombre,
        f.FechaFactura,
        f.Subtotal,
        f.TotalImpuesto,
        f.TotalDescuento,
        f.TotalFactura,
        f.Estado,
        f.FechaCreacion,
        f.FechaModificacion,
        f.Activo
    FROM dbo.Factura f
    INNER JOIN dbo.Cliente c ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona pc ON pc.Identificacion = c.Identificacion
    INNER JOIN dbo.Empleado e ON e.Identificacion = f.IdentificacionEmpleado
    INNER JOIN dbo.Persona pe ON pe.Identificacion = e.Identificacion
    INNER JOIN dbo.TipoPago tp ON tp.IdTipoPago = f.IdTipoPago
    INNER JOIN dbo.TipoFactura tf ON tf.IdTipoFactura = f.IdTipoFactura
    ORDER BY f.FechaFactura DESC, f.NumeroFactura DESC;
END;
GO

/* =========================================================
   FACTURA - LISTAR ACTIVAS
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_ListarActivas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.NumeroFactura,
        f.IdentificacionCliente,
        LTRIM(RTRIM(ISNULL(pc.Nombre, '') + ' ' + ISNULL(pc.PrimerApellido, '') + ' ' + ISNULL(pc.SegundoApellido, ''))) AS ClienteNombre,
        f.IdentificacionEmpleado,
        LTRIM(RTRIM(ISNULL(pe.Nombre, '') + ' ' + ISNULL(pe.PrimerApellido, '') + ' ' + ISNULL(pe.SegundoApellido, ''))) AS EmpleadoNombre,
        f.IdTipoPago,
        tp.Nombre AS TipoPagoNombre,
        f.IdTipoFactura,
        tf.TipoFactura AS TipoFacturaNombre,
        f.FechaFactura,
        f.Subtotal,
        f.TotalImpuesto,
        f.TotalDescuento,
        f.TotalFactura,
        f.Estado,
        f.FechaCreacion,
        f.FechaModificacion,
        f.Activo
    FROM dbo.Factura f
    INNER JOIN dbo.Cliente c ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona pc ON pc.Identificacion = c.Identificacion
    INNER JOIN dbo.Empleado e ON e.Identificacion = f.IdentificacionEmpleado
    INNER JOIN dbo.Persona pe ON pe.Identificacion = e.Identificacion
    INNER JOIN dbo.TipoPago tp ON tp.IdTipoPago = f.IdTipoPago
    INNER JOIN dbo.TipoFactura tf ON tf.IdTipoFactura = f.IdTipoFactura
    WHERE f.Activo = 1
    ORDER BY f.FechaFactura DESC, f.NumeroFactura DESC;
END;
GO

/* =========================================================
   FACTURA - LISTAR POR CLIENTE
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_ListarPorCliente
    @IdentificacionCliente VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));

    SELECT
        f.NumeroFactura,
        f.IdentificacionCliente,
        LTRIM(RTRIM(ISNULL(pc.Nombre, '') + ' ' + ISNULL(pc.PrimerApellido, '') + ' ' + ISNULL(pc.SegundoApellido, ''))) AS ClienteNombre,
        f.IdentificacionEmpleado,
        LTRIM(RTRIM(ISNULL(pe.Nombre, '') + ' ' + ISNULL(pe.PrimerApellido, '') + ' ' + ISNULL(pe.SegundoApellido, ''))) AS EmpleadoNombre,
        f.IdTipoPago,
        tp.Nombre AS TipoPagoNombre,
        f.IdTipoFactura,
        tf.TipoFactura AS TipoFacturaNombre,
        f.FechaFactura,
        f.Subtotal,
        f.TotalImpuesto,
        f.TotalDescuento,
        f.TotalFactura,
        f.Estado,
        f.FechaCreacion,
        f.FechaModificacion,
        f.Activo
    FROM dbo.Factura f
    INNER JOIN dbo.Cliente c ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona pc ON pc.Identificacion = c.Identificacion
    INNER JOIN dbo.Empleado e ON e.Identificacion = f.IdentificacionEmpleado
    INNER JOIN dbo.Persona pe ON pe.Identificacion = e.Identificacion
    INNER JOIN dbo.TipoPago tp ON tp.IdTipoPago = f.IdTipoPago
    INNER JOIN dbo.TipoFactura tf ON tf.IdTipoFactura = f.IdTipoFactura
    WHERE f.IdentificacionCliente = @IdentificacionCliente
    ORDER BY f.FechaFactura DESC, f.NumeroFactura DESC;
END;
GO

/* =========================================================
   FACTURA - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_Obtener
    @NumeroFactura VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    SELECT
        f.NumeroFactura,
        f.IdentificacionCliente,
        LTRIM(RTRIM(ISNULL(pc.Nombre, '') + ' ' + ISNULL(pc.PrimerApellido, '') + ' ' + ISNULL(pc.SegundoApellido, ''))) AS ClienteNombre,
        f.IdentificacionEmpleado,
        LTRIM(RTRIM(ISNULL(pe.Nombre, '') + ' ' + ISNULL(pe.PrimerApellido, '') + ' ' + ISNULL(pe.SegundoApellido, ''))) AS EmpleadoNombre,
        f.IdTipoPago,
        tp.Nombre AS TipoPagoNombre,
        f.IdTipoFactura,
        tf.TipoFactura AS TipoFacturaNombre,
        f.FechaFactura,
        f.Subtotal,
        f.TotalImpuesto,
        f.TotalDescuento,
        f.TotalFactura,
        f.Estado,
        f.FechaCreacion,
        f.FechaModificacion,
        f.Activo
    FROM dbo.Factura f
    INNER JOIN dbo.Cliente c ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona pc ON pc.Identificacion = c.Identificacion
    INNER JOIN dbo.Empleado e ON e.Identificacion = f.IdentificacionEmpleado
    INNER JOIN dbo.Persona pe ON pe.Identificacion = e.Identificacion
    INNER JOIN dbo.TipoPago tp ON tp.IdTipoPago = f.IdTipoPago
    INNER JOIN dbo.TipoFactura tf ON tf.IdTipoFactura = f.IdTipoFactura
    WHERE f.NumeroFactura = @NumeroFactura;
END;
GO

/* =========================================================
   FACTURA - GENERAR NUMERO
   Formato: FAC-2026-000001
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_GenerarNumero
    @Anio INT,
    @NumeroFactura VARCHAR(45) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Consecutivo INT;

    SET @NumeroFactura = '';

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Anio = YEAR(GETDATE());
    END;

    SELECT @Consecutivo = ISNULL(MAX(TRY_CAST(RIGHT(NumeroFactura, 6) AS INT)), 0) + 1
    FROM dbo.Factura
    WHERE NumeroFactura LIKE 'FAC-' + CAST(@Anio AS VARCHAR(4)) + '-%';

    SET @NumeroFactura = 'FAC-' + CAST(@Anio AS VARCHAR(4)) + '-' + RIGHT('000000' + CAST(@Consecutivo AS VARCHAR(6)), 6);
END;
GO

/* =========================================================
   FACTURA - REGISTRAR ENCABEZADO
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_Registrar
    @IdentificacionCliente VARCHAR(45),
    @IdentificacionEmpleado VARCHAR(45),
    @IdTipoPago INT,
    @IdTipoFactura INT,
    @FechaFactura DATETIME,
    @Estado VARCHAR(45),
    @NumeroFacturaGenerado VARCHAR(45) OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NumeroFactura VARCHAR(45);
    DECLARE @Anio INT;

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroFacturaGenerado = '';

    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @IdentificacionEmpleado = LTRIM(RTRIM(ISNULL(@IdentificacionEmpleado, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    IF (@IdentificacionCliente = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un cliente.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Cliente WHERE Identificacion = @IdentificacionCliente AND Activo = 1)
    BEGIN
        SET @Mensaje = 'El cliente seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@IdentificacionEmpleado = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un empleado.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Empleado WHERE Identificacion = @IdentificacionEmpleado AND Activo = 1)
    BEGIN
        SET @Mensaje = 'El empleado seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@IdTipoPago <= 0 OR NOT EXISTS (SELECT 1 FROM dbo.TipoPago WHERE IdTipoPago = @IdTipoPago AND Activo = 1))
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de pago activo.';
        RETURN;
    END;

    IF (@IdTipoFactura <= 0 OR NOT EXISTS (SELECT 1 FROM dbo.TipoFactura WHERE IdTipoFactura = @IdTipoFactura AND Activo = 1))
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de factura activo.';
        RETURN;
    END;

    IF (@FechaFactura IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de factura.';
        RETURN;
    END;

    IF (@Estado = '')
        SET @Estado = 'Borrador';

    BEGIN TRY
        SET @Anio = YEAR(@FechaFactura);

        EXEC Facturacion.sp_Factura_GenerarNumero
            @Anio = @Anio,
            @NumeroFactura = @NumeroFactura OUTPUT;

        INSERT INTO dbo.Factura
        (
            NumeroFactura,
            IdentificacionCliente,
            IdentificacionEmpleado,
            IdTipoPago,
            IdTipoFactura,
            FechaFactura,
            Subtotal,
            TotalImpuesto,
            TotalDescuento,
            TotalFactura,
            Estado,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @NumeroFactura,
            @IdentificacionCliente,
            @IdentificacionEmpleado,
            @IdTipoPago,
            @IdTipoFactura,
            @FechaFactura,
            0,
            0,
            0,
            0,
            @Estado,
            GETDATE(),
            GETDATE(),
            1
        );

        SET @Resultado = 1;
        SET @NumeroFacturaGenerado = @NumeroFactura;
        SET @Mensaje = 'Factura registrada correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @NumeroFacturaGenerado = '';
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   FACTURA - EDITAR ENCABEZADO
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_Editar
    @NumeroFactura VARCHAR(45),
    @IdentificacionCliente VARCHAR(45),
    @IdentificacionEmpleado VARCHAR(45),
    @IdTipoPago INT,
    @IdTipoFactura INT,
    @FechaFactura DATETIME,
    @Estado VARCHAR(45),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @IdentificacionEmpleado = LTRIM(RTRIM(ISNULL(@IdentificacionEmpleado, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    IF (@NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una factura válida.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Factura WHERE NumeroFactura = @NumeroFactura)
    BEGIN
        SET @Mensaje = 'La factura no existe.';
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM dbo.Factura WHERE NumeroFactura = @NumeroFactura AND Estado IN ('Emitida', 'Anulada', 'Cancelada'))
    BEGIN
        SET @Mensaje = 'No se puede editar el encabezado de una factura emitida, anulada o cancelada.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Cliente WHERE Identificacion = @IdentificacionCliente AND Activo = 1)
    BEGIN
        SET @Mensaje = 'El cliente seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Empleado WHERE Identificacion = @IdentificacionEmpleado AND Activo = 1)
    BEGIN
        SET @Mensaje = 'El empleado seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@IdTipoPago <= 0 OR NOT EXISTS (SELECT 1 FROM dbo.TipoPago WHERE IdTipoPago = @IdTipoPago AND Activo = 1))
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de pago activo.';
        RETURN;
    END;

    IF (@IdTipoFactura <= 0 OR NOT EXISTS (SELECT 1 FROM dbo.TipoFactura WHERE IdTipoFactura = @IdTipoFactura AND Activo = 1))
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de factura activo.';
        RETURN;
    END;

    IF (@FechaFactura IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha de factura.';
        RETURN;
    END;

    IF (@Estado = '')
        SET @Estado = 'Borrador';

    UPDATE dbo.Factura
    SET IdentificacionCliente = @IdentificacionCliente,
        IdentificacionEmpleado = @IdentificacionEmpleado,
        IdTipoPago = @IdTipoPago,
        IdTipoFactura = @IdTipoFactura,
        FechaFactura = @FechaFactura,
        Estado = @Estado,
        FechaModificacion = GETDATE(),
        Activo = @Activo
    WHERE NumeroFactura = @NumeroFactura;

    SET @Resultado = 1;
    SET @Mensaje = 'Factura actualizada correctamente.';
END;
GO

/* =========================================================
   FACTURA - RECALCULAR TOTALES
   PrecioUnitario ya incluye IVA, por eso el IVA no se suma.
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_RecalcularTotales
    @NumeroFactura VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    UPDATE dbo.Factura
    SET Subtotal = ISNULL((
            SELECT SUM(SubtotalLinea)
            FROM dbo.DetalleFactura
            WHERE NumeroFactura = @NumeroFactura
              AND Activo = 1
        ), 0),
        TotalDescuento = ISNULL((
            SELECT SUM(SubtotalLinea * PorcentajeDescuento / 100.0)
            FROM dbo.DetalleFactura
            WHERE NumeroFactura = @NumeroFactura
              AND Activo = 1
        ), 0),
        TotalImpuesto = ISNULL((
            SELECT SUM(
                CASE
                    WHEN PorcentajeImpuesto > 0 THEN
                        TotalLinea - (TotalLinea / (1 + (PorcentajeImpuesto / 100.0)))
                    ELSE 0
                END
            )
            FROM dbo.DetalleFactura
            WHERE NumeroFactura = @NumeroFactura
              AND Activo = 1
        ), 0),
        TotalFactura = ISNULL((
            SELECT SUM(TotalLinea)
            FROM dbo.DetalleFactura
            WHERE NumeroFactura = @NumeroFactura
              AND Activo = 1
        ), 0),
        FechaModificacion = GETDATE()
    WHERE NumeroFactura = @NumeroFactura;
END;
GO

/* =========================================================
   FACTURA - CAMBIAR ESTADO SIMPLE
   No usar para emitir con efectos automáticos. Para emitir use sp_Factura_Emitir.
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_CambiarEstado
    @NumeroFactura VARCHAR(45),
    @Estado VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @Estado = LTRIM(RTRIM(ISNULL(@Estado, '')));

    IF (@NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una factura válida.';
        RETURN;
    END;

    IF (@Estado = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar el estado.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Factura WHERE NumeroFactura = @NumeroFactura)
    BEGIN
        SET @Mensaje = 'La factura no existe.';
        RETURN;
    END;

    UPDATE dbo.Factura
    SET Estado = @Estado,
        FechaModificacion = GETDATE()
    WHERE NumeroFactura = @NumeroFactura;

    SET @Resultado = 1;
    SET @Mensaje = 'Estado de factura actualizado correctamente.';
END;
GO

/* =========================================================
   FACTURA - INACTIVAR / ANULAR
   Inactiva ingreso automático de contado y CxC si no hay abonos.
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_Inactivar
    @NumeroFactura VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdentificacionCliente VARCHAR(45);
    DECLARE @TieneAbonos BIT;

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    IF (@NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una factura válida.';
        RETURN;
    END;

    SELECT @IdentificacionCliente = IdentificacionCliente
    FROM dbo.Factura
    WHERE NumeroFactura = @NumeroFactura
      AND Activo = 1;

    IF (@IdentificacionCliente IS NULL)
    BEGIN
        SET @Mensaje = 'La factura no existe o ya está inactiva.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.AbonoCuentaPorCobrar
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
          AND Activo = 1
          AND NumeroAbono > 0
    )
        SET @TieneAbonos = 1;
    ELSE
        SET @TieneAbonos = 0;

    IF (@TieneAbonos = 1)
    BEGIN
        SET @Mensaje = 'No se puede anular la factura porque la cuenta por cobrar ya tiene abonos activos.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.Factura
        SET Estado = 'Anulada',
            Activo = 0,
            FechaModificacion = GETDATE()
        WHERE NumeroFactura = @NumeroFactura;

        UPDATE dbo.DetalleFactura
        SET Activo = 0,
            FechaModificacion = GETDATE()
        WHERE NumeroFactura = @NumeroFactura
          AND Activo = 1;

        UPDATE dbo.Ingreso
        SET Activo = 0,
            FechaModificacion = GETDATE()
        WHERE IdentificacionCliente = @IdentificacionCliente
          AND NumeroFactura = @NumeroFactura
          AND OrigenIngreso = 'Factura'
          AND Activo = 1;

        IF EXISTS (
            SELECT 1
            FROM dbo.CuentaPorCobrar
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura
              AND Activo = 1
        )
        BEGIN
            UPDATE dbo.CuentaPorCobrar
            SET Estado = 'Anulada',
                Activo = 0,
                FechaModificacion = GETDATE()
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura;
        END;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Factura anulada correctamente. Se inactivaron los movimientos asociados.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   FACTURA - EMITIR
   Factura contado: crea ingreso automático.
   Factura crédito: crea cuenta por cobrar usando DiasCredito del cliente.
   ========================================================= */

CREATE OR ALTER PROCEDURE Facturacion.sp_Factura_Emitir
    @NumeroFactura VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdentificacionCliente VARCHAR(45);
    DECLARE @FechaFactura DATETIME;
    DECLARE @TotalFactura DECIMAL(18,2);
    DECLARE @EstadoFactura VARCHAR(45);
    DECLARE @Activo BIT;
    DECLARE @TipoFactura VARCHAR(100);
    DECLARE @EsCredito BIT;
    DECLARE @DiasCredito INT;

    DECLARE @ResultadoIngreso BIT;
    DECLARE @MensajeIngreso VARCHAR(500);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    IF (@NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una factura válida.';
        RETURN;
    END;

    SELECT
        @IdentificacionCliente = f.IdentificacionCliente,
        @FechaFactura = f.FechaFactura,
        @TotalFactura = f.TotalFactura,
        @EstadoFactura = f.Estado,
        @Activo = f.Activo,
        @TipoFactura = tf.TipoFactura,
        @DiasCredito = ISNULL(c.DiasCredito, 30)
    FROM dbo.Factura f
    INNER JOIN dbo.TipoFactura tf
        ON tf.IdTipoFactura = f.IdTipoFactura
    INNER JOIN dbo.Cliente c
        ON c.Identificacion = f.IdentificacionCliente
    WHERE f.NumeroFactura = @NumeroFactura;

    IF (@IdentificacionCliente IS NULL)
    BEGIN
        SET @Mensaje = 'La factura no existe.';
        RETURN;
    END;

    IF (@Activo = 0)
    BEGIN
        SET @Mensaje = 'No se puede emitir una factura inactiva.';
        RETURN;
    END;

    IF (@EstadoFactura IN ('Emitida', 'Anulada', 'Cancelada'))
    BEGIN
        SET @Mensaje = 'La factura ya fue emitida, anulada o cancelada.';
        RETURN;
    END;

    IF (@TotalFactura <= 0)
    BEGIN
        SET @Mensaje = 'No se puede emitir una factura con total cero.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.DetalleFactura
        WHERE NumeroFactura = @NumeroFactura
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'Debe agregar al menos un producto activo antes de emitir la factura.';
        RETURN;
    END;

    IF (@DiasCredito IS NULL OR @DiasCredito < 0)
    BEGIN
        SET @DiasCredito = 30;
    END;

    IF (@DiasCredito > 365)
    BEGIN
        SET @DiasCredito = 365;
    END;

    SET @EsCredito =
        CASE
            WHEN UPPER(@TipoFactura) COLLATE Latin1_General_CI_AI LIKE '%CREDITO%' THEN 1
            WHEN UPPER(@TipoFactura) COLLATE Latin1_General_CI_AI LIKE '%CRÉDITO%' THEN 1
            ELSE 0
        END;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.Factura
        SET
            Estado = 'Emitida',
            FechaModificacion = GETDATE()
        WHERE NumeroFactura = @NumeroFactura;

        /* =====================================================
           FACTURA CRÉDITO: CREAR CUENTA POR COBRAR
           ===================================================== */
        IF (@EsCredito = 1)
        BEGIN
            IF NOT EXISTS (
                SELECT 1
                FROM dbo.CuentaPorCobrar
                WHERE IdentificacionCliente = @IdentificacionCliente
                  AND NumeroFactura = @NumeroFactura
            )
            BEGIN
                INSERT INTO dbo.CuentaPorCobrar
                (
                    IdentificacionCliente,
                    NumeroFactura,
                    FechaEmision,
                    FechaVencimiento,
                    MontoOriginal,
                    SaldoActual,
                    Estado,
                    FechaCreacion,
                    FechaModificacion,
                    Activo
                )
                VALUES
                (
                    @IdentificacionCliente,
                    @NumeroFactura,
                    @FechaFactura,
                    DATEADD(DAY, @DiasCredito, @FechaFactura),
                    @TotalFactura,
                    @TotalFactura,
                    'Pendiente',
                    GETDATE(),
                    GETDATE(),
                    1
                );
            END;
        END
        /* =====================================================
           FACTURA CONTADO: CREAR INGRESO AUTOMÁTICO
           ===================================================== */
        ELSE
        BEGIN
            EXEC Movimientos.sp_Ingreso_RegistrarAutomatico
                @FechaIngreso = @FechaFactura,
                @Descripcion = 'Ingreso automático por factura de contado',
                @OrigenIngreso = 'Factura',
                @Monto = @TotalFactura,

                @IdentificacionCliente = @IdentificacionCliente,
                @NumeroFactura = @NumeroFactura,

                @IdentificacionClienteAbono = '000000000',
                @NumeroFacturaAbono = 'FAC-GENERAL-000000',
                @NumeroAbonoCuentaPorCobrar = 0,

                @Resultado = @ResultadoIngreso OUTPUT,
                @Mensaje = @MensajeIngreso OUTPUT;

            IF (@ResultadoIngreso = 0)
            BEGIN
                ROLLBACK TRANSACTION;

                SET @Resultado = 0;
                SET @Mensaje = @MensajeIngreso;
                RETURN;
            END;
        END;

        COMMIT TRANSACTION;

        SET @Resultado = 1;

        IF (@EsCredito = 1)
        BEGIN
            SET @Mensaje = 'Factura emitida correctamente y cuenta por cobrar generada con vencimiento según los días de crédito del cliente.';
        END
        ELSE
        BEGIN
            SET @Mensaje = 'Factura emitida correctamente e ingreso automático registrado.';
        END;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO