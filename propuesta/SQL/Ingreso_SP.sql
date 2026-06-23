IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Movimientos')
BEGIN
    EXEC('CREATE SCHEMA Movimientos');
END;
GO

/* =========================================================
   INGRESO - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Ingreso_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        i.NumeroIngreso,
        i.FechaIngreso,
        i.Descripcion,
        i.IdTipoIngreso,
        ti.Nombre AS TipoIngresoNombre,
        i.CodigoCuenta,
        cc.NombreCuenta,
        i.IdentificacionCliente,
        i.NumeroFactura,
        LTRIM(RTRIM(ISNULL(pc.Nombre, '') + ' ' + ISNULL(pc.PrimerApellido, '') + ' ' + ISNULL(pc.SegundoApellido, ''))) AS ClienteNombre,
        i.IdentificacionClienteAbono,
        i.NumeroFacturaAbono,
        i.NumeroAbonoCuentaPorCobrar,
        a.FechaAbono,
        a.MontoAbono,
        i.OrigenIngreso,
        i.Monto,
        i.FechaCreacion,
        i.FechaModificacion,
        i.Activo
    FROM dbo.Ingreso i
    INNER JOIN dbo.TipoIngreso ti
        ON ti.IdTipoIngreso = i.IdTipoIngreso
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = i.CodigoCuenta
    LEFT JOIN dbo.Cliente c
        ON c.Identificacion = i.IdentificacionCliente
    LEFT JOIN dbo.Persona pc
        ON pc.Identificacion = c.Identificacion
    LEFT JOIN dbo.AbonoCuentaPorCobrar a
        ON a.IdentificacionCliente = i.IdentificacionClienteAbono
       AND a.NumeroFactura = i.NumeroFacturaAbono
       AND a.NumeroAbono = i.NumeroAbonoCuentaPorCobrar
    ORDER BY i.FechaIngreso DESC, i.NumeroIngreso DESC;
END;
GO

/* =========================================================
   INGRESO - LISTAR ACTIVOS
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Ingreso_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        i.NumeroIngreso,
        i.FechaIngreso,
        i.Descripcion,
        i.IdTipoIngreso,
        ti.Nombre AS TipoIngresoNombre,
        i.CodigoCuenta,
        cc.NombreCuenta,
        i.IdentificacionCliente,
        i.NumeroFactura,
        LTRIM(RTRIM(ISNULL(pc.Nombre, '') + ' ' + ISNULL(pc.PrimerApellido, '') + ' ' + ISNULL(pc.SegundoApellido, ''))) AS ClienteNombre,
        i.IdentificacionClienteAbono,
        i.NumeroFacturaAbono,
        i.NumeroAbonoCuentaPorCobrar,
        a.FechaAbono,
        a.MontoAbono,
        i.OrigenIngreso,
        i.Monto,
        i.FechaCreacion,
        i.FechaModificacion,
        i.Activo
    FROM dbo.Ingreso i
    INNER JOIN dbo.TipoIngreso ti
        ON ti.IdTipoIngreso = i.IdTipoIngreso
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = i.CodigoCuenta
    LEFT JOIN dbo.Cliente c
        ON c.Identificacion = i.IdentificacionCliente
    LEFT JOIN dbo.Persona pc
        ON pc.Identificacion = c.Identificacion
    LEFT JOIN dbo.AbonoCuentaPorCobrar a
        ON a.IdentificacionCliente = i.IdentificacionClienteAbono
       AND a.NumeroFactura = i.NumeroFacturaAbono
       AND a.NumeroAbono = i.NumeroAbonoCuentaPorCobrar
    WHERE i.Activo = 1
    ORDER BY i.FechaIngreso DESC, i.NumeroIngreso DESC;
END;
GO

/* =========================================================
   INGRESO - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Ingreso_Obtener
    @NumeroIngreso VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroIngreso = LTRIM(RTRIM(ISNULL(@NumeroIngreso, '')));

    SELECT
        i.NumeroIngreso,
        i.FechaIngreso,
        i.Descripcion,
        i.IdTipoIngreso,
        ti.Nombre AS TipoIngresoNombre,
        i.CodigoCuenta,
        cc.NombreCuenta,
        i.IdentificacionCliente,
        i.NumeroFactura,
        LTRIM(RTRIM(ISNULL(pc.Nombre, '') + ' ' + ISNULL(pc.PrimerApellido, '') + ' ' + ISNULL(pc.SegundoApellido, ''))) AS ClienteNombre,
        i.IdentificacionClienteAbono,
        i.NumeroFacturaAbono,
        i.NumeroAbonoCuentaPorCobrar,
        a.FechaAbono,
        a.MontoAbono,
        i.OrigenIngreso,
        i.Monto,
        i.FechaCreacion,
        i.FechaModificacion,
        i.Activo
    FROM dbo.Ingreso i
    INNER JOIN dbo.TipoIngreso ti
        ON ti.IdTipoIngreso = i.IdTipoIngreso
    INNER JOIN dbo.CuentaContable cc
        ON cc.CodigoCuenta = i.CodigoCuenta
    LEFT JOIN dbo.Cliente c
        ON c.Identificacion = i.IdentificacionCliente
    LEFT JOIN dbo.Persona pc
        ON pc.Identificacion = c.Identificacion
    LEFT JOIN dbo.AbonoCuentaPorCobrar a
        ON a.IdentificacionCliente = i.IdentificacionClienteAbono
       AND a.NumeroFactura = i.NumeroFacturaAbono
       AND a.NumeroAbono = i.NumeroAbonoCuentaPorCobrar
    WHERE i.NumeroIngreso = @NumeroIngreso;
END;
GO

/* =========================================================
   INGRESO - GENERAR NUMERO
   Formato: ING-2026-000001
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Ingreso_GenerarNumero
    @Anio INT,
    @NumeroIngreso VARCHAR(45) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Consecutivo INT;

    SET @NumeroIngreso = '';

    IF (@Anio < 2000 OR @Anio > 2100)
    BEGIN
        SET @Anio = YEAR(GETDATE());
    END;

    SELECT @Consecutivo = ISNULL(MAX(TRY_CAST(RIGHT(NumeroIngreso, 6) AS INT)), 0) + 1
    FROM dbo.Ingreso
    WHERE NumeroIngreso LIKE 'ING-' + CAST(@Anio AS VARCHAR(4)) + '-%';

    SET @NumeroIngreso = 'ING-' + CAST(@Anio AS VARCHAR(4)) + '-' + RIGHT('000000' + CAST(@Consecutivo AS VARCHAR(6)), 6);
END;
GO

/* =========================================================
   INGRESO - REGISTRAR AUTOMATICO
   Usado por factura contado y abono CxC.
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Ingreso_RegistrarAutomatico
    @FechaIngreso DATETIME,
    @Descripcion VARCHAR(150),
    @OrigenIngreso VARCHAR(45),
    @Monto DECIMAL(18,2),
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @IdentificacionClienteAbono VARCHAR(45),
    @NumeroFacturaAbono VARCHAR(45),
    @NumeroAbonoCuentaPorCobrar INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdTipoIngreso INT;
    DECLARE @CodigoCuenta VARCHAR(45);
    DECLARE @NumeroIngreso VARCHAR(45);
    DECLARE @Anio INT;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));
    SET @OrigenIngreso = LTRIM(RTRIM(ISNULL(@OrigenIngreso, '')));
    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @IdentificacionClienteAbono = LTRIM(RTRIM(ISNULL(@IdentificacionClienteAbono, '')));
    SET @NumeroFacturaAbono = LTRIM(RTRIM(ISNULL(@NumeroFacturaAbono, '')));

    IF (@FechaIngreso IS NULL)
        SET @FechaIngreso = GETDATE();

    IF (@Descripcion = '')
        SET @Descripcion = 'Ingreso automático';

    IF (@OrigenIngreso = '')
        SET @OrigenIngreso = 'Manual';

    IF (@Monto <= 0)
    BEGIN
        SET @Mensaje = 'El monto del ingreso automático debe ser mayor a cero.';
        RETURN;
    END;

    IF (@IdentificacionCliente = '')
        SET @IdentificacionCliente = '000000000';

    IF (@NumeroFactura = '')
        SET @NumeroFactura = 'FAC-GENERAL-000000';

    IF (@IdentificacionClienteAbono = '')
        SET @IdentificacionClienteAbono = '000000000';

    IF (@NumeroFacturaAbono = '')
        SET @NumeroFacturaAbono = 'FAC-GENERAL-000000';

    IF (@NumeroAbonoCuentaPorCobrar < 0)
        SET @NumeroAbonoCuentaPorCobrar = 0;

    SELECT TOP 1 @IdTipoIngreso = IdTipoIngreso
    FROM dbo.TipoIngreso
    WHERE Activo = 1
      AND (
            Nombre COLLATE Latin1_General_CI_AI LIKE '%venta%'
         OR Nombre COLLATE Latin1_General_CI_AI LIKE '%factura%'
         OR Nombre COLLATE Latin1_General_CI_AI LIKE '%abono%'
         OR Nombre COLLATE Latin1_General_CI_AI LIKE '%ingreso%'
      )
    ORDER BY
        CASE
            WHEN Nombre COLLATE Latin1_General_CI_AI LIKE '%venta%' THEN 1
            WHEN Nombre COLLATE Latin1_General_CI_AI LIKE '%abono%' THEN 2
            ELSE 3
        END,
        IdTipoIngreso;

    IF (@IdTipoIngreso IS NULL)
    BEGIN
        SELECT TOP 1 @IdTipoIngreso = IdTipoIngreso
        FROM dbo.TipoIngreso
        WHERE Activo = 1
        ORDER BY IdTipoIngreso;
    END;

    IF (@IdTipoIngreso IS NULL)
    BEGIN
        SET @Mensaje = 'No existe un tipo de ingreso activo para registrar el ingreso automático.';
        RETURN;
    END;

    SELECT TOP 1 @CodigoCuenta = CodigoCuenta
    FROM dbo.CuentaContable
    WHERE Activo = 1
      AND AceptaMovimientos = 1
      AND (
            NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%caja%'
         OR NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%efectivo%'
         OR NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%banco%'
         OR NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%cuenta bancaria%'
      )
    ORDER BY
        CASE
            WHEN NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%caja%' THEN 1
            WHEN NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%efectivo%' THEN 2
            WHEN NombreCuenta COLLATE Latin1_General_CI_AI LIKE '%banco%' THEN 3
            ELSE 4
        END,
        CodigoCuenta;

    IF (@CodigoCuenta IS NULL)
    BEGIN
        SELECT TOP 1 @CodigoCuenta = CodigoCuenta
        FROM dbo.CuentaContable
        WHERE Activo = 1
          AND AceptaMovimientos = 1
        ORDER BY CodigoCuenta;
    END;

    IF (@CodigoCuenta IS NULL)
    BEGIN
        SET @Mensaje = 'No existe una cuenta contable activa que acepte movimientos para registrar el ingreso automático.';
        RETURN;
    END;

    IF (@OrigenIngreso = 'Factura')
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM dbo.Ingreso
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura
              AND OrigenIngreso = 'Factura'
              AND Activo = 1
        )
        BEGIN
            SET @Resultado = 1;
            SET @Mensaje = 'El ingreso automático de la factura ya existe.';
            RETURN;
        END;
    END;

    IF (@OrigenIngreso = 'Abono CxC')
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM dbo.Ingreso
            WHERE IdentificacionClienteAbono = @IdentificacionClienteAbono
              AND NumeroFacturaAbono = @NumeroFacturaAbono
              AND NumeroAbonoCuentaPorCobrar = @NumeroAbonoCuentaPorCobrar
              AND OrigenIngreso = 'Abono CxC'
              AND Activo = 1
        )
        BEGIN
            SET @Resultado = 1;
            SET @Mensaje = 'El ingreso automático del abono ya existe.';
            RETURN;
        END;
    END;

    BEGIN TRY
        SET @Anio = YEAR(@FechaIngreso);

        EXEC Movimientos.sp_Ingreso_GenerarNumero
            @Anio = @Anio,
            @NumeroIngreso = @NumeroIngreso OUTPUT;

        INSERT INTO dbo.Ingreso
        (
            NumeroIngreso,
            FechaIngreso,
            Descripcion,
            IdTipoIngreso,
            CodigoCuenta,
            IdentificacionCliente,
            NumeroFactura,
            IdentificacionClienteAbono,
            NumeroFacturaAbono,
            NumeroAbonoCuentaPorCobrar,
            OrigenIngreso,
            Monto,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @NumeroIngreso,
            @FechaIngreso,
            @Descripcion,
            @IdTipoIngreso,
            @CodigoCuenta,
            @IdentificacionCliente,
            @NumeroFactura,
            @IdentificacionClienteAbono,
            @NumeroFacturaAbono,
            @NumeroAbonoCuentaPorCobrar,
            @OrigenIngreso,
            @Monto,
            GETDATE(),
            GETDATE(),
            1
        );

        SET @Resultado = 1;
        SET @Mensaje = 'Ingreso automático registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   INGRESO - REGISTRAR MANUAL / SELECCIONADO DESDE VISTA
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Ingreso_Registrar
    @FechaIngreso DATETIME,
    @Descripcion VARCHAR(150),
    @IdTipoIngreso INT,
    @CodigoCuenta VARCHAR(45),
    @IdentificacionCliente VARCHAR(45),
    @NumeroFactura VARCHAR(45),
    @IdentificacionClienteAbono VARCHAR(45),
    @NumeroFacturaAbono VARCHAR(45),
    @NumeroAbonoCuentaPorCobrar INT,
    @OrigenIngreso VARCHAR(45),
    @Monto DECIMAL(18,2),
    @NumeroIngresoGenerado VARCHAR(45) OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NumeroIngreso VARCHAR(45);
    DECLARE @Anio INT;
    DECLARE @TipoFactura VARCHAR(100);

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroIngresoGenerado = '';

    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));
    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @IdentificacionCliente = LTRIM(RTRIM(ISNULL(@IdentificacionCliente, '')));
    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @IdentificacionClienteAbono = LTRIM(RTRIM(ISNULL(@IdentificacionClienteAbono, '')));
    SET @NumeroFacturaAbono = LTRIM(RTRIM(ISNULL(@NumeroFacturaAbono, '')));
    SET @OrigenIngreso = LTRIM(RTRIM(ISNULL(@OrigenIngreso, '')));

    IF (@IdentificacionCliente = '') SET @IdentificacionCliente = '000000000';
    IF (@NumeroFactura = '') SET @NumeroFactura = 'FAC-GENERAL-000000';
    IF (@IdentificacionClienteAbono = '') SET @IdentificacionClienteAbono = '000000000';
    IF (@NumeroFacturaAbono = '') SET @NumeroFacturaAbono = 'FAC-GENERAL-000000';
    IF (@NumeroAbonoCuentaPorCobrar < 0) SET @NumeroAbonoCuentaPorCobrar = 0;

    IF (@OrigenIngreso = '')
    BEGIN
        IF (@NumeroAbonoCuentaPorCobrar > 0)
            SET @OrigenIngreso = 'Abono CxC';
        ELSE IF (@NumeroFactura <> 'FAC-GENERAL-000000')
            SET @OrigenIngreso = 'Factura';
        ELSE
            SET @OrigenIngreso = 'Manual';
    END;

    IF (@FechaIngreso IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha del ingreso.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar una descripción.';
        RETURN;
    END;

    IF (LEN(@Descripcion) > 150)
    BEGIN
        SET @Mensaje = 'La descripción no puede superar los 150 caracteres.';
        RETURN;
    END;

    IF (@IdTipoIngreso <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de ingreso.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.TipoIngreso WHERE IdTipoIngreso = @IdTipoIngreso AND Activo = 1)
    BEGIN
        SET @Mensaje = 'El tipo de ingreso seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@CodigoCuenta = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta contable.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.CuentaContable WHERE CodigoCuenta = @CodigoCuenta AND Activo = 1 AND AceptaMovimientos = 1)
    BEGIN
        SET @Mensaje = 'La cuenta contable seleccionada no existe, está inactiva o no acepta movimientos.';
        RETURN;
    END;

    IF (@Monto <= 0)
    BEGIN
        SET @Mensaje = 'El monto del ingreso debe ser mayor a cero.';
        RETURN;
    END;

    IF (@OrigenIngreso = 'Factura')
    BEGIN
        SET @IdentificacionClienteAbono = '000000000';
        SET @NumeroFacturaAbono = 'FAC-GENERAL-000000';
        SET @NumeroAbonoCuentaPorCobrar = 0;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.Factura
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura
              AND Activo = 1
              AND Estado = 'Emitida'
              AND TotalFactura > 0
        )
        BEGIN
            SET @Mensaje = 'La factura seleccionada no existe, no está emitida o no está activa.';
            RETURN;
        END;

        SELECT @TipoFactura = tf.TipoFactura
        FROM dbo.Factura f
        INNER JOIN dbo.TipoFactura tf ON tf.IdTipoFactura = f.IdTipoFactura
        WHERE f.IdentificacionCliente = @IdentificacionCliente
          AND f.NumeroFactura = @NumeroFactura;

        IF (UPPER(@TipoFactura) COLLATE Latin1_General_CI_AI LIKE '%CREDITO%')
        BEGIN
            SET @Mensaje = 'Las facturas de crédito generan ingresos por medio de sus abonos, no directamente desde la factura.';
            RETURN;
        END;

        IF EXISTS (
            SELECT 1
            FROM dbo.Ingreso
            WHERE IdentificacionCliente = @IdentificacionCliente
              AND NumeroFactura = @NumeroFactura
              AND OrigenIngreso = 'Factura'
              AND Activo = 1
        )
        BEGIN
            SET @Mensaje = 'Ya existe un ingreso activo asociado a esta factura.';
            RETURN;
        END;
    END
    ELSE IF (@OrigenIngreso = 'Abono CxC')
    BEGIN
        SET @IdentificacionCliente = '000000000';
        SET @NumeroFactura = 'FAC-GENERAL-000000';

        IF (@NumeroAbonoCuentaPorCobrar <= 0)
        BEGIN
            SET @Mensaje = 'Debe seleccionar un abono válido.';
            RETURN;
        END;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.AbonoCuentaPorCobrar
            WHERE IdentificacionCliente = @IdentificacionClienteAbono
              AND NumeroFactura = @NumeroFacturaAbono
              AND NumeroAbono = @NumeroAbonoCuentaPorCobrar
              AND Activo = 1
              AND MontoAbono > 0
        )
        BEGIN
            SET @Mensaje = 'El abono seleccionado no existe o está inactivo.';
            RETURN;
        END;

        IF EXISTS (
            SELECT 1
            FROM dbo.Ingreso
            WHERE IdentificacionClienteAbono = @IdentificacionClienteAbono
              AND NumeroFacturaAbono = @NumeroFacturaAbono
              AND NumeroAbonoCuentaPorCobrar = @NumeroAbonoCuentaPorCobrar
              AND OrigenIngreso = 'Abono CxC'
              AND Activo = 1
        )
        BEGIN
            SET @Mensaje = 'Ya existe un ingreso activo asociado a este abono.';
            RETURN;
        END;
    END
    ELSE
    BEGIN
        SET @OrigenIngreso = 'Manual';
        SET @IdentificacionCliente = '000000000';
        SET @NumeroFactura = 'FAC-GENERAL-000000';
        SET @IdentificacionClienteAbono = '000000000';
        SET @NumeroFacturaAbono = 'FAC-GENERAL-000000';
        SET @NumeroAbonoCuentaPorCobrar = 0;
    END;

    BEGIN TRY
        SET @Anio = YEAR(@FechaIngreso);

        EXEC Movimientos.sp_Ingreso_GenerarNumero
            @Anio = @Anio,
            @NumeroIngreso = @NumeroIngreso OUTPUT;

        INSERT INTO dbo.Ingreso
        (
            NumeroIngreso, FechaIngreso, Descripcion, IdTipoIngreso, CodigoCuenta,
            IdentificacionCliente, NumeroFactura, IdentificacionClienteAbono,
            NumeroFacturaAbono, NumeroAbonoCuentaPorCobrar, OrigenIngreso,
            Monto, FechaCreacion, FechaModificacion, Activo
        )
        VALUES
        (
            @NumeroIngreso, @FechaIngreso, @Descripcion, @IdTipoIngreso, @CodigoCuenta,
            @IdentificacionCliente, @NumeroFactura, @IdentificacionClienteAbono,
            @NumeroFacturaAbono, @NumeroAbonoCuentaPorCobrar, @OrigenIngreso,
            @Monto, GETDATE(), GETDATE(), 1
        );

        SET @Resultado = 1;
        SET @NumeroIngresoGenerado = @NumeroIngreso;
        SET @Mensaje = 'Ingreso registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @NumeroIngresoGenerado = '';
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   INGRESO - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Ingreso_Editar
    @NumeroIngreso VARCHAR(45),
    @FechaIngreso DATETIME,
    @Descripcion VARCHAR(150),
    @IdTipoIngreso INT,
    @CodigoCuenta VARCHAR(45),
    @OrigenIngreso VARCHAR(45),
    @Monto DECIMAL(18,2),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroIngreso = LTRIM(RTRIM(ISNULL(@NumeroIngreso, '')));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));
    SET @CodigoCuenta = LTRIM(RTRIM(ISNULL(@CodigoCuenta, '')));
    SET @OrigenIngreso = LTRIM(RTRIM(ISNULL(@OrigenIngreso, '')));

    IF (@NumeroIngreso = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un ingreso válido.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Ingreso WHERE NumeroIngreso = @NumeroIngreso)
    BEGIN
        SET @Mensaje = 'El ingreso seleccionado no existe.';
        RETURN;
    END;

    IF (@FechaIngreso IS NULL)
    BEGIN
        SET @Mensaje = 'Debe ingresar la fecha del ingreso.';
        RETURN;
    END;

    IF (@Descripcion = '')
    BEGIN
        SET @Mensaje = 'Debe ingresar una descripción.';
        RETURN;
    END;

    IF (LEN(@Descripcion) > 150)
    BEGIN
        SET @Mensaje = 'La descripción no puede superar los 150 caracteres.';
        RETURN;
    END;

    IF (@IdTipoIngreso <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de ingreso.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.TipoIngreso WHERE IdTipoIngreso = @IdTipoIngreso AND Activo = 1)
    BEGIN
        SET @Mensaje = 'El tipo de ingreso seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@CodigoCuenta = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una cuenta contable.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.CuentaContable WHERE CodigoCuenta = @CodigoCuenta AND Activo = 1 AND AceptaMovimientos = 1)
    BEGIN
        SET @Mensaje = 'La cuenta contable seleccionada no existe, está inactiva o no acepta movimientos.';
        RETURN;
    END;

    IF (@Monto <= 0 AND @Activo = 1)
    BEGIN
        SET @Mensaje = 'El monto del ingreso debe ser mayor a cero.';
        RETURN;
    END;

    IF (@OrigenIngreso = '')
    BEGIN
        SET @Mensaje = 'Debe indicar el origen del ingreso.';
        RETURN;
    END;

    UPDATE dbo.Ingreso
    SET FechaIngreso = @FechaIngreso,
        Descripcion = @Descripcion,
        IdTipoIngreso = @IdTipoIngreso,
        CodigoCuenta = @CodigoCuenta,
        OrigenIngreso = @OrigenIngreso,
        Monto = @Monto,
        FechaModificacion = GETDATE(),
        Activo = @Activo
    WHERE NumeroIngreso = @NumeroIngreso;

    SET @Resultado = 1;
    SET @Mensaje = 'Ingreso actualizado correctamente.';
END;
GO

/* =========================================================
   INGRESO - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Ingreso_Inactivar
    @NumeroIngreso VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @NumeroIngreso = LTRIM(RTRIM(ISNULL(@NumeroIngreso, '')));

    IF (@NumeroIngreso = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un ingreso válido.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Ingreso WHERE NumeroIngreso = @NumeroIngreso AND Activo = 1)
    BEGIN
        SET @Mensaje = 'El ingreso no existe o ya está inactivo.';
        RETURN;
    END;

    UPDATE dbo.Ingreso
    SET Activo = 0,
        FechaModificacion = GETDATE()
    WHERE NumeroIngreso = @NumeroIngreso;

    SET @Resultado = 1;
    SET @Mensaje = 'Ingreso inactivado correctamente.';
END;
GO

/* =========================================================
   INGRESO - FACTURAS CONTADO PENDIENTES DE INGRESO
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Ingreso_ListarFacturasContadoPendientes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.IdentificacionCliente,
        f.NumeroFactura,
        LTRIM(RTRIM(ISNULL(p.Nombre, '') + ' ' + ISNULL(p.PrimerApellido, '') + ' ' + ISNULL(p.SegundoApellido, ''))) AS ClienteNombre,
        f.FechaFactura,
        f.TotalFactura,
        tf.TipoFactura
    FROM dbo.Factura f
    INNER JOIN dbo.TipoFactura tf ON tf.IdTipoFactura = f.IdTipoFactura
    INNER JOIN dbo.Cliente c ON c.Identificacion = f.IdentificacionCliente
    INNER JOIN dbo.Persona p ON p.Identificacion = c.Identificacion
    WHERE f.Activo = 1
      AND f.Estado = 'Emitida'
      AND f.TotalFactura > 0
      AND UPPER(tf.TipoFactura) COLLATE Latin1_General_CI_AI NOT LIKE '%CREDITO%'
      AND NOT EXISTS (
            SELECT 1
            FROM dbo.Ingreso i
            WHERE i.IdentificacionCliente = f.IdentificacionCliente
              AND i.NumeroFactura = f.NumeroFactura
              AND i.OrigenIngreso = 'Factura'
              AND i.Activo = 1
      )
    ORDER BY f.FechaFactura DESC, f.NumeroFactura DESC;
END;
GO

/* =========================================================
   INGRESO - ABONOS CXC PENDIENTES DE INGRESO
   ========================================================= */
CREATE OR ALTER PROCEDURE Movimientos.sp_Ingreso_ListarAbonosPendientes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.IdentificacionCliente,
        a.NumeroFactura,
        a.NumeroAbono,
        a.FechaAbono,
        a.MontoAbono,
        a.Observacion,
        LTRIM(RTRIM(ISNULL(p.Nombre, '') + ' ' + ISNULL(p.PrimerApellido, '') + ' ' + ISNULL(p.SegundoApellido, ''))) AS ClienteNombre,
        cxc.MontoOriginal,
        cxc.SaldoActual,
        cxc.Estado AS EstadoCuenta
    FROM dbo.AbonoCuentaPorCobrar a
    INNER JOIN dbo.CuentaPorCobrar cxc
        ON cxc.IdentificacionCliente = a.IdentificacionCliente
       AND cxc.NumeroFactura = a.NumeroFactura
    INNER JOIN dbo.Cliente c ON c.Identificacion = a.IdentificacionCliente
    INNER JOIN dbo.Persona p ON p.Identificacion = c.Identificacion
    WHERE a.Activo = 1
      AND a.MontoAbono > 0
      AND NOT EXISTS (
            SELECT 1
            FROM dbo.Ingreso i
            WHERE i.IdentificacionClienteAbono = a.IdentificacionCliente
              AND i.NumeroFacturaAbono = a.NumeroFactura
              AND i.NumeroAbonoCuentaPorCobrar = a.NumeroAbono
              AND i.OrigenIngreso = 'Abono CxC'
              AND i.Activo = 1
      )
    ORDER BY a.FechaAbono DESC, a.NumeroFactura DESC, a.NumeroAbono DESC;
END;
GO
