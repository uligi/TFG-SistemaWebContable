IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Inventario')
BEGIN
    EXEC('CREATE SCHEMA Inventario');
END;
GO

/* =========================================================
   PRODUCTO - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Inventario.sp_Producto_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.CodigoProducto,
        p.NombreProducto,
        p.IdTipoProducto,
        tp.Nombre AS TipoProductoNombre,
        p.IdImpuesto,
        i.Nombre AS ImpuestoNombre,
        i.Porcentaje AS PorcentajeImpuesto,
        p.Descripcion,
        p.PrecioVenta,
        p.StockActual,
        p.FechaCreacion,
        p.FechaModificacion,
        p.Activo
    FROM dbo.Producto p
    INNER JOIN dbo.TipoProducto tp
        ON tp.IdTipoProducto = p.IdTipoProducto
    INNER JOIN dbo.Impuesto i
        ON i.IdImpuesto = p.IdImpuesto
    ORDER BY
        p.NombreProducto;
END;
GO

/* =========================================================
   PRODUCTO - LISTAR ACTIVOS
   ========================================================= */
CREATE OR ALTER PROCEDURE Inventario.sp_Producto_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.CodigoProducto,
        p.NombreProducto,
        p.IdTipoProducto,
        tp.Nombre AS TipoProductoNombre,
        p.IdImpuesto,
        i.Nombre AS ImpuestoNombre,
        i.Porcentaje AS PorcentajeImpuesto,
        p.Descripcion,
        p.PrecioVenta,
        p.StockActual,
        p.FechaCreacion,
        p.FechaModificacion,
        p.Activo
    FROM dbo.Producto p
    INNER JOIN dbo.TipoProducto tp
        ON tp.IdTipoProducto = p.IdTipoProducto
    INNER JOIN dbo.Impuesto i
        ON i.IdImpuesto = p.IdImpuesto
    WHERE p.Activo = 1
    ORDER BY
        p.NombreProducto;
END;
GO

/* =========================================================
   PRODUCTO - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Inventario.sp_Producto_Obtener
    @CodigoProducto VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoProducto = LTRIM(RTRIM(ISNULL(@CodigoProducto, '')));

    SELECT
        p.CodigoProducto,
        p.NombreProducto,
        p.IdTipoProducto,
        tp.Nombre AS TipoProductoNombre,
        p.IdImpuesto,
        i.Nombre AS ImpuestoNombre,
        i.Porcentaje AS PorcentajeImpuesto,
        p.Descripcion,
        p.PrecioVenta,
        p.StockActual,
        p.FechaCreacion,
        p.FechaModificacion,
        p.Activo
    FROM dbo.Producto p
    INNER JOIN dbo.TipoProducto tp
        ON tp.IdTipoProducto = p.IdTipoProducto
    INNER JOIN dbo.Impuesto i
        ON i.IdImpuesto = p.IdImpuesto
    WHERE p.CodigoProducto = @CodigoProducto;
END;
GO

/* =========================================================
   PRODUCTO - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Inventario.sp_Producto_Registrar
    @NombreProducto VARCHAR(150),
    @IdTipoProducto INT,
    @IdImpuesto INT,
    @Descripcion VARCHAR(250),
    @PrecioVenta DECIMAL(18,2),
    @StockActual DECIMAL(18,2),
    @CodigoProductoGenerado VARCHAR(45) OUTPUT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NuevoNumero INT;
    DECLARE @CodigoProducto VARCHAR(45);

    SET @Resultado = 0;
    SET @Mensaje = '';
    SET @CodigoProductoGenerado = '';

    SET @NombreProducto = LTRIM(RTRIM(ISNULL(@NombreProducto, '')));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));

    IF (@NombreProducto = '')
    BEGIN
        SET @Mensaje = 'El nombre del producto es obligatorio.';
        RETURN;
    END;

    IF (LEN(@NombreProducto) > 150)
    BEGIN
        SET @Mensaje = 'El nombre del producto no puede superar los 150 caracteres.';
        RETURN;
    END;

    IF (@IdTipoProducto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de producto.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoProducto
        WHERE IdTipoProducto = @IdTipoProducto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El tipo de producto seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@IdImpuesto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un impuesto.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Impuesto
        WHERE IdImpuesto = @IdImpuesto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El impuesto seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@PrecioVenta < 0)
    BEGIN
        SET @Mensaje = 'El precio de venta no puede ser negativo.';
        RETURN;
    END;

    IF (@StockActual < 0)
    BEGIN
        SET @Mensaje = 'El stock actual no puede ser negativo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Producto
        WHERE UPPER(LTRIM(RTRIM(NombreProducto))) = UPPER(@NombreProducto)
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'Ya existe un producto activo con ese nombre.';
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @NuevoNumero =
            ISNULL(MAX(TRY_CAST(REPLACE(CodigoProducto, 'PROD-', '') AS INT)), 0) + 1
        FROM dbo.Producto
        WHERE CodigoProducto LIKE 'PROD-%';

        SET @CodigoProducto =
            'PROD-' + RIGHT('000000' + CAST(@NuevoNumero AS VARCHAR(10)), 6);

        INSERT INTO dbo.Producto
        (
            CodigoProducto,
            NombreProducto,
            IdTipoProducto,
            IdImpuesto,
            Descripcion,
            PrecioVenta,
            StockActual,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @CodigoProducto,
            @NombreProducto,
            @IdTipoProducto,
            @IdImpuesto,
            @Descripcion,
            @PrecioVenta,
            @StockActual,
            GETDATE(),
            GETDATE(),
            1
        );

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @CodigoProductoGenerado = @CodigoProducto;
        SET @Mensaje = 'Producto registrado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @CodigoProductoGenerado = '';
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   PRODUCTO - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Inventario.sp_Producto_Editar
    @CodigoProducto VARCHAR(45),
    @NombreProducto VARCHAR(150),
    @IdTipoProducto INT,
    @IdImpuesto INT,
    @Descripcion VARCHAR(250),
    @PrecioVenta DECIMAL(18,2),
    @StockActual DECIMAL(18,2),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoProducto = LTRIM(RTRIM(ISNULL(@CodigoProducto, '')));
    SET @NombreProducto = LTRIM(RTRIM(ISNULL(@NombreProducto, '')));
    SET @Descripcion = LTRIM(RTRIM(ISNULL(@Descripcion, '')));

    IF (@CodigoProducto = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un producto válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Producto
        WHERE CodigoProducto = @CodigoProducto
    )
    BEGIN
        SET @Mensaje = 'El producto no existe.';
        RETURN;
    END;

    IF (@NombreProducto = '')
    BEGIN
        SET @Mensaje = 'El nombre del producto es obligatorio.';
        RETURN;
    END;

    IF (LEN(@NombreProducto) > 150)
    BEGIN
        SET @Mensaje = 'El nombre del producto no puede superar los 150 caracteres.';
        RETURN;
    END;

    IF (@IdTipoProducto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un tipo de producto.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoProducto
        WHERE IdTipoProducto = @IdTipoProducto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El tipo de producto seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@IdImpuesto <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un impuesto.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Impuesto
        WHERE IdImpuesto = @IdImpuesto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El impuesto seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@PrecioVenta < 0)
    BEGIN
        SET @Mensaje = 'El precio de venta no puede ser negativo.';
        RETURN;
    END;

    IF (@StockActual < 0)
    BEGIN
        SET @Mensaje = 'El stock actual no puede ser negativo.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.Producto
        WHERE UPPER(LTRIM(RTRIM(NombreProducto))) = UPPER(@NombreProducto)
          AND CodigoProducto <> @CodigoProducto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'Ya existe otro producto activo con ese nombre.';
        RETURN;
    END;

    IF (@Activo = 0)
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM dbo.DetalleFactura df
            INNER JOIN dbo.Factura f
                ON f.NumeroFactura = df.NumeroFactura
            WHERE df.CodigoProducto = @CodigoProducto
              AND df.Activo = 1
              AND f.Activo = 1
              AND f.Estado NOT IN ('Anulada', 'Cancelada')
        )
        BEGIN
            SET @Mensaje = 'No se puede inactivar el producto porque tiene facturas activas asociadas.';
            RETURN;
        END;
    END;

    BEGIN TRY
        UPDATE dbo.Producto
        SET
            NombreProducto = @NombreProducto,
            IdTipoProducto = @IdTipoProducto,
            IdImpuesto = @IdImpuesto,
            Descripcion = @Descripcion,
            PrecioVenta = @PrecioVenta,
            StockActual = @StockActual,
            FechaModificacion = GETDATE(),
            Activo = @Activo
        WHERE CodigoProducto = @CodigoProducto;

        SET @Resultado = 1;
        SET @Mensaje = 'Producto actualizado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   PRODUCTO - AJUSTAR STOCK
   TipoMovimiento:
   - SUMAR
   - RESTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Inventario.sp_Producto_AjustarStock
    @CodigoProducto VARCHAR(45),
    @Cantidad DECIMAL(18,2),
    @TipoMovimiento VARCHAR(10),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StockActual DECIMAL(18,2);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoProducto = LTRIM(RTRIM(ISNULL(@CodigoProducto, '')));
    SET @TipoMovimiento = UPPER(LTRIM(RTRIM(ISNULL(@TipoMovimiento, ''))));

    IF (@CodigoProducto = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un producto válido.';
        RETURN;
    END;

    IF (@Cantidad <= 0)
    BEGIN
        SET @Mensaje = 'La cantidad debe ser mayor a 0.';
        RETURN;
    END;

    IF (@TipoMovimiento NOT IN ('SUMAR', 'RESTAR'))
    BEGIN
        SET @Mensaje = 'El tipo de movimiento debe ser SUMAR o RESTAR.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Producto
        WHERE CodigoProducto = @CodigoProducto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El producto no existe o está inactivo.';
        RETURN;
    END;

    SELECT @StockActual = StockActual
    FROM dbo.Producto
    WHERE CodigoProducto = @CodigoProducto;

    IF (@TipoMovimiento = 'RESTAR' AND @StockActual < @Cantidad)
    BEGIN
        SET @Mensaje = 'No hay stock suficiente para realizar el movimiento.';
        RETURN;
    END;

    BEGIN TRY
        UPDATE dbo.Producto
        SET
            StockActual =
                CASE
                    WHEN @TipoMovimiento = 'SUMAR' THEN StockActual + @Cantidad
                    ELSE StockActual - @Cantidad
                END,
            FechaModificacion = GETDATE()
        WHERE CodigoProducto = @CodigoProducto;

        SET @Resultado = 1;
        SET @Mensaje = 'Stock actualizado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

/* =========================================================
   PRODUCTO - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Inventario.sp_Producto_Inactivar
    @CodigoProducto VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @CodigoProducto = LTRIM(RTRIM(ISNULL(@CodigoProducto, '')));

    IF (@CodigoProducto = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un producto válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Producto
        WHERE CodigoProducto = @CodigoProducto
    )
    BEGIN
        SET @Mensaje = 'El producto no existe.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.DetalleFactura df
        INNER JOIN dbo.Factura f
            ON f.NumeroFactura = df.NumeroFactura
        WHERE df.CodigoProducto = @CodigoProducto
          AND df.Activo = 1
          AND f.Activo = 1
          AND f.Estado NOT IN ('Anulada', 'Cancelada')
    )
    BEGIN
        SET @Mensaje = 'No se puede inactivar el producto porque tiene facturas activas asociadas.';
        RETURN;
    END;

    BEGIN TRY
        UPDATE dbo.Producto
        SET
            Activo = 0,
            FechaModificacion = GETDATE()
        WHERE CodigoProducto = @CodigoProducto;

        SET @Resultado = 1;
        SET @Mensaje = 'Producto inactivado correctamente.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO