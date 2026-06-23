IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Facturacion')
BEGIN
    EXEC('CREATE SCHEMA Facturacion');
END;
GO

/* =========================================================
   DETALLE FACTURA - LISTAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_DetalleFactura_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        df.NumeroFactura,
        df.CodigoProducto,
        p.NombreProducto,
        df.IdImpuesto,
        i.Nombre AS ImpuestoNombre,
        df.IdDescuento,
        d.Nombre AS DescuentoNombre,
        df.DescripcionItem,
        df.Cantidad,
        df.PrecioUnitario,
        df.PorcentajeImpuesto,
        df.PorcentajeDescuento,
        df.SubtotalLinea,
        df.TotalLinea,
        df.FechaCreacion,
        df.FechaModificacion,
        df.Activo
    FROM dbo.DetalleFactura df
    INNER JOIN dbo.Producto p
        ON p.CodigoProducto = df.CodigoProducto
    INNER JOIN dbo.Impuesto i
        ON i.IdImpuesto = df.IdImpuesto
    INNER JOIN dbo.Descuento d
        ON d.IdDescuento = df.IdDescuento
    ORDER BY
        df.NumeroFactura DESC,
        p.NombreProducto ASC;
END;
GO

/* =========================================================
   DETALLE FACTURA - LISTAR POR FACTURA
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_DetalleFactura_ListarPorFactura
    @NumeroFactura VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));

    SELECT
        df.NumeroFactura,
        df.CodigoProducto,
        p.NombreProducto,
        df.IdImpuesto,
        i.Nombre AS ImpuestoNombre,
        df.IdDescuento,
        d.Nombre AS DescuentoNombre,
        df.DescripcionItem,
        df.Cantidad,
        df.PrecioUnitario,
        df.PorcentajeImpuesto,
        df.PorcentajeDescuento,
        df.SubtotalLinea,
        df.TotalLinea,
        df.FechaCreacion,
        df.FechaModificacion,
        df.Activo
    FROM dbo.DetalleFactura df
    INNER JOIN dbo.Producto p
        ON p.CodigoProducto = df.CodigoProducto
    INNER JOIN dbo.Impuesto i
        ON i.IdImpuesto = df.IdImpuesto
    INNER JOIN dbo.Descuento d
        ON d.IdDescuento = df.IdDescuento
    WHERE df.NumeroFactura = @NumeroFactura
    ORDER BY
        p.NombreProducto ASC;
END;
GO

/* =========================================================
   DETALLE FACTURA - OBTENER
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_DetalleFactura_Obtener
    @NumeroFactura VARCHAR(45),
    @CodigoProducto VARCHAR(45)
AS
BEGIN
    SET NOCOUNT ON;

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @CodigoProducto = LTRIM(RTRIM(ISNULL(@CodigoProducto, '')));

    SELECT
        df.NumeroFactura,
        df.CodigoProducto,
        p.NombreProducto,
        df.IdImpuesto,
        i.Nombre AS ImpuestoNombre,
        df.IdDescuento,
        d.Nombre AS DescuentoNombre,
        df.DescripcionItem,
        df.Cantidad,
        df.PrecioUnitario,
        df.PorcentajeImpuesto,
        df.PorcentajeDescuento,
        df.SubtotalLinea,
        df.TotalLinea,
        df.FechaCreacion,
        df.FechaModificacion,
        df.Activo
    FROM dbo.DetalleFactura df
    INNER JOIN dbo.Producto p
        ON p.CodigoProducto = df.CodigoProducto
    INNER JOIN dbo.Impuesto i
        ON i.IdImpuesto = df.IdImpuesto
    INNER JOIN dbo.Descuento d
        ON d.IdDescuento = df.IdDescuento
    WHERE df.NumeroFactura = @NumeroFactura
      AND df.CodigoProducto = @CodigoProducto;
END;
GO

/* =========================================================
   DETALLE FACTURA - REGISTRAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_DetalleFactura_Registrar
    @NumeroFactura VARCHAR(45),
    @CodigoProducto VARCHAR(45),
    @IdImpuesto INT,
    @IdDescuento INT,
    @DescripcionItem VARCHAR(150),
    @Cantidad DECIMAL(18,2),
    @PrecioUnitario DECIMAL(18,2),
    @PorcentajeImpuesto DECIMAL(5,2),
    @PorcentajeDescuento DECIMAL(5,2),
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SubtotalLinea DECIMAL(18,2);
    DECLARE @MontoDescuento DECIMAL(18,2);
    DECLARE @BaseImponible DECIMAL(18,2);
    DECLARE @MontoImpuesto DECIMAL(18,2);
    DECLARE @TotalLinea DECIMAL(18,2);
    DECLARE @StockActual DECIMAL(18,2);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @CodigoProducto = LTRIM(RTRIM(ISNULL(@CodigoProducto, '')));
    SET @DescripcionItem = LTRIM(RTRIM(ISNULL(@DescripcionItem, '')));

    SET @PorcentajeImpuesto = ISNULL(@PorcentajeImpuesto, 0);
    SET @PorcentajeDescuento = ISNULL(@PorcentajeDescuento, 0);

    IF (@NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una factura válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Factura
        WHERE NumeroFactura = @NumeroFactura
          AND Activo = 1
          AND Estado NOT IN ('Anulada', 'Cancelada')
    )
    BEGIN
        SET @Mensaje = 'La factura no existe, está inactiva o está anulada.';
        RETURN;
    END;

    IF (@CodigoProducto = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un producto.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Producto
        WHERE CodigoProducto = @CodigoProducto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El producto seleccionado no existe o está inactivo.';
        RETURN;
    END;

    SELECT @StockActual = StockActual
    FROM dbo.Producto
    WHERE CodigoProducto = @CodigoProducto;

    IF (@Cantidad <= 0)
    BEGIN
        SET @Mensaje = 'La cantidad debe ser mayor a 0.';
        RETURN;
    END;

    IF (@StockActual < @Cantidad)
    BEGIN
        SET @Mensaje = 'No hay stock suficiente para facturar este producto.';
        RETURN;
    END;

    IF (@PrecioUnitario < 0)
    BEGIN
        SET @Mensaje = 'El precio unitario no puede ser negativo.';
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

    IF (@IdDescuento <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un descuento.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Descuento
        WHERE IdDescuento = @IdDescuento
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El descuento seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@PorcentajeImpuesto < 0 OR @PorcentajeImpuesto > 100)
    BEGIN
        SET @Mensaje = 'El porcentaje de impuesto debe estar entre 0 y 100.';
        RETURN;
    END;

    IF (@PorcentajeDescuento < 0 OR @PorcentajeDescuento > 100)
    BEGIN
        SET @Mensaje = 'El porcentaje de descuento debe estar entre 0 y 100.';
        RETURN;
    END;

    IF (LEN(@DescripcionItem) > 150)
    BEGIN
        SET @Mensaje = 'La descripción del item no puede superar los 150 caracteres.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.DetalleFactura
        WHERE NumeroFactura = @NumeroFactura
          AND CodigoProducto = @CodigoProducto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'Este producto ya está registrado en el detalle de la factura.';
        RETURN;
    END;

/* 
   PrecioUnitario ya incluye IVA.
   Por eso NO se suma impuesto adicional al total.
*/
SET @SubtotalLinea = ROUND(@Cantidad * @PrecioUnitario, 2);

SET @MontoDescuento = ROUND(@SubtotalLinea * (@PorcentajeDescuento / 100), 2);

SET @TotalLinea = @SubtotalLinea - @MontoDescuento;

IF (@TotalLinea < 0)
BEGIN
    SET @TotalLinea = 0;
END;

IF (@PorcentajeImpuesto > 0)
BEGIN
    SET @BaseImponible = ROUND(@TotalLinea / (1 + (@PorcentajeImpuesto / 100)), 2);
    SET @MontoImpuesto = ROUND(@TotalLinea - @BaseImponible, 2);
END
ELSE
BEGIN
    SET @BaseImponible = @TotalLinea;
    SET @MontoImpuesto = 0;
END;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO dbo.DetalleFactura
        (
            NumeroFactura,
            CodigoProducto,
            IdImpuesto,
            IdDescuento,
            DescripcionItem,
            Cantidad,
            PrecioUnitario,
            PorcentajeImpuesto,
            PorcentajeDescuento,
            SubtotalLinea,
            TotalLinea,
            FechaCreacion,
            FechaModificacion,
            Activo
        )
        VALUES
        (
            @NumeroFactura,
            @CodigoProducto,
            @IdImpuesto,
            @IdDescuento,
            @DescripcionItem,
            @Cantidad,
            @PrecioUnitario,
            @PorcentajeImpuesto,
            @PorcentajeDescuento,
            @SubtotalLinea,
            @TotalLinea,
            GETDATE(),
            GETDATE(),
            1
        );

        UPDATE dbo.Producto
        SET
            StockActual = StockActual - @Cantidad,
            FechaModificacion = GETDATE()
        WHERE CodigoProducto = @CodigoProducto;

        EXEC Facturacion.sp_Factura_RecalcularTotales
            @NumeroFactura = @NumeroFactura;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Detalle de factura registrado correctamente.';
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
   DETALLE FACTURA - EDITAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_DetalleFactura_Editar
    @NumeroFactura VARCHAR(45),
    @CodigoProducto VARCHAR(45),
    @IdImpuesto INT,
    @IdDescuento INT,
    @DescripcionItem VARCHAR(150),
    @Cantidad DECIMAL(18,2),
    @PrecioUnitario DECIMAL(18,2),
    @PorcentajeImpuesto DECIMAL(5,2),
    @PorcentajeDescuento DECIMAL(5,2),
    @Activo BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CantidadAnterior DECIMAL(18,2);
    DECLARE @DiferenciaCantidad DECIMAL(18,2);
    DECLARE @StockActual DECIMAL(18,2);

    DECLARE @SubtotalLinea DECIMAL(18,2);
    DECLARE @MontoDescuento DECIMAL(18,2);
    DECLARE @BaseImponible DECIMAL(18,2);
    DECLARE @MontoImpuesto DECIMAL(18,2);
    DECLARE @TotalLinea DECIMAL(18,2);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @CodigoProducto = LTRIM(RTRIM(ISNULL(@CodigoProducto, '')));
    SET @DescripcionItem = LTRIM(RTRIM(ISNULL(@DescripcionItem, '')));

    SET @PorcentajeImpuesto = ISNULL(@PorcentajeImpuesto, 0);
    SET @PorcentajeDescuento = ISNULL(@PorcentajeDescuento, 0);

    IF (@NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una factura válida.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Factura
        WHERE NumeroFactura = @NumeroFactura
          AND Activo = 1
          AND Estado NOT IN ('Anulada', 'Cancelada')
    )
    BEGIN
        SET @Mensaje = 'La factura no existe, está inactiva o está anulada.';
        RETURN;
    END;

    IF (@CodigoProducto = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un producto.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.DetalleFactura
        WHERE NumeroFactura = @NumeroFactura
          AND CodigoProducto = @CodigoProducto
    )
    BEGIN
        SET @Mensaje = 'El detalle de factura no existe.';
        RETURN;
    END;

    IF (@Cantidad <= 0)
    BEGIN
        SET @Mensaje = 'La cantidad debe ser mayor a 0.';
        RETURN;
    END;

    IF (@PrecioUnitario < 0)
    BEGIN
        SET @Mensaje = 'El precio unitario no puede ser negativo.';
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

    IF (@IdDescuento <= 0)
    BEGIN
        SET @Mensaje = 'Debe seleccionar un descuento.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Descuento
        WHERE IdDescuento = @IdDescuento
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El descuento seleccionado no existe o está inactivo.';
        RETURN;
    END;

    IF (@PorcentajeImpuesto < 0 OR @PorcentajeImpuesto > 100)
    BEGIN
        SET @Mensaje = 'El porcentaje de impuesto debe estar entre 0 y 100.';
        RETURN;
    END;

    IF (@PorcentajeDescuento < 0 OR @PorcentajeDescuento > 100)
    BEGIN
        SET @Mensaje = 'El porcentaje de descuento debe estar entre 0 y 100.';
        RETURN;
    END;

    IF (LEN(@DescripcionItem) > 150)
    BEGIN
        SET @Mensaje = 'La descripción del item no puede superar los 150 caracteres.';
        RETURN;
    END;

    SELECT @CantidadAnterior = Cantidad
    FROM dbo.DetalleFactura
    WHERE NumeroFactura = @NumeroFactura
      AND CodigoProducto = @CodigoProducto;

    SELECT @StockActual = StockActual
    FROM dbo.Producto
    WHERE CodigoProducto = @CodigoProducto;

    SET @DiferenciaCantidad = @Cantidad - @CantidadAnterior;

    IF (@Activo = 1 AND @DiferenciaCantidad > 0 AND @StockActual < @DiferenciaCantidad)
    BEGIN
        SET @Mensaje = 'No hay stock suficiente para aumentar la cantidad.';
        RETURN;
    END;

/* 
   En Costa Rica el precio de venta al consumidor ya incluye IVA.
   Por eso el IVA NO se suma nuevamente al total.
*/
SET @SubtotalLinea = ROUND(@Cantidad * @PrecioUnitario, 2);

SET @MontoDescuento = ROUND(@SubtotalLinea * (@PorcentajeDescuento / 100), 2);

SET @TotalLinea = @SubtotalLinea - @MontoDescuento;

IF (@TotalLinea < 0)
BEGIN
    SET @TotalLinea = 0;
END;

/* IVA incluido dentro del precio */
IF (@PorcentajeImpuesto > 0)
BEGIN
    SET @BaseImponible = ROUND(@TotalLinea / (1 + (@PorcentajeImpuesto / 100)), 2);
    SET @MontoImpuesto = ROUND(@TotalLinea - @BaseImponible, 2);
END
ELSE
BEGIN
    SET @BaseImponible = @TotalLinea;
    SET @MontoImpuesto = 0;
END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@Activo = 1)
        BEGIN
            UPDATE dbo.Producto
            SET
                StockActual = StockActual - @DiferenciaCantidad,
                FechaModificacion = GETDATE()
            WHERE CodigoProducto = @CodigoProducto;
        END
        ELSE
        BEGIN
            UPDATE dbo.Producto
            SET
                StockActual = StockActual + @CantidadAnterior,
                FechaModificacion = GETDATE()
            WHERE CodigoProducto = @CodigoProducto;
        END;

        UPDATE dbo.DetalleFactura
        SET
            IdImpuesto = @IdImpuesto,
            IdDescuento = @IdDescuento,
            DescripcionItem = @DescripcionItem,
            Cantidad = @Cantidad,
            PrecioUnitario = @PrecioUnitario,
            PorcentajeImpuesto = @PorcentajeImpuesto,
            PorcentajeDescuento = @PorcentajeDescuento,
            SubtotalLinea = @SubtotalLinea,
            TotalLinea = @TotalLinea,
            FechaModificacion = GETDATE(),
            Activo = @Activo
        WHERE NumeroFactura = @NumeroFactura
          AND CodigoProducto = @CodigoProducto;

        EXEC Facturacion.sp_Factura_RecalcularTotales
            @NumeroFactura = @NumeroFactura;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Detalle de factura actualizado correctamente.';
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
   DETALLE FACTURA - INACTIVAR
   ========================================================= */
CREATE OR ALTER PROCEDURE Facturacion.sp_DetalleFactura_Inactivar
    @NumeroFactura VARCHAR(45),
    @CodigoProducto VARCHAR(45),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Cantidad DECIMAL(18,2);

    SET @Resultado = 0;
    SET @Mensaje = '';

    SET @NumeroFactura = LTRIM(RTRIM(ISNULL(@NumeroFactura, '')));
    SET @CodigoProducto = LTRIM(RTRIM(ISNULL(@CodigoProducto, '')));

    IF (@NumeroFactura = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar una factura válida.';
        RETURN;
    END;

    IF (@CodigoProducto = '')
    BEGIN
        SET @Mensaje = 'Debe seleccionar un producto válido.';
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.DetalleFactura
        WHERE NumeroFactura = @NumeroFactura
          AND CodigoProducto = @CodigoProducto
          AND Activo = 1
    )
    BEGIN
        SET @Mensaje = 'El detalle de factura no existe o ya está inactivo.';
        RETURN;
    END;

    SELECT @Cantidad = Cantidad
    FROM dbo.DetalleFactura
    WHERE NumeroFactura = @NumeroFactura
      AND CodigoProducto = @CodigoProducto;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.Producto
        SET
            StockActual = StockActual + @Cantidad,
            FechaModificacion = GETDATE()
        WHERE CodigoProducto = @CodigoProducto;

        UPDATE dbo.DetalleFactura
        SET
            Activo = 0,
            FechaModificacion = GETDATE()
        WHERE NumeroFactura = @NumeroFactura
          AND CodigoProducto = @CodigoProducto;

        EXEC Facturacion.sp_Factura_RecalcularTotales
            @NumeroFactura = @NumeroFactura;

        COMMIT TRANSACTION;

        SET @Resultado = 1;
        SET @Mensaje = 'Detalle de factura inactivado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO