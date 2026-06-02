CREATE DATABASE CRUD_Tienda;
GO

USE CRUD_Tienda;
GO

-- ============================================
-- FUNCIONES DE ENCRIPTACIÓN
-- ============================================
CREATE FUNCTION encripta(@clave VARCHAR(100))
RETURNS VARBINARY(MAX)
AS
BEGIN
    RETURN ENCRYPTBYPASSPHRASE('cl@ve', @clave);
END;
GO

CREATE FUNCTION desencripta(@clave VARBINARY(MAX))
RETURNS VARCHAR(100)
AS
BEGIN
    RETURN DECRYPTBYPASSPHRASE('cl@ve', @clave);
END;
GO

-- ============================================
-- TABLA DE CATEGORÍAS (normalización)
-- ============================================
CREATE TABLE tbl_categoria(
    cat_id INT IDENTITY(1,1) PRIMARY KEY,
    cat_nombre VARCHAR(50) NOT NULL UNIQUE,
    cat_descripcion VARCHAR(200) NULL,
    cat_fecha_creacion DATETIME DEFAULT GETDATE()
);
GO

-- ============================================
-- TABLA DE USUARIOS (con ruta de imagen)
-- ============================================
CREATE TABLE tbl_usuario(
    usu_id INT IDENTITY(1,1) PRIMARY KEY,
    usu_nombre VARCHAR(50) NOT NULL,
    usu_correo VARCHAR(100) NOT NULL UNIQUE,
    usu_contrasena VARBINARY(MAX) NOT NULL,
    usu_cedula VARCHAR(10) NULL,
    usu_celular VARCHAR(10) NULL,
    usu_rol VARCHAR(20) NOT NULL,
    usu_estado CHAR(1) DEFAULT 'A',
    usu_intentos INT DEFAULT 0,
    usu_imagen_path VARCHAR(255) NULL,          -- Ruta de la imagen de perfil
    usu_fecha_creacion DATETIME DEFAULT GETDATE(),
    usu_token_reset VARCHAR(100) NULL,
    usu_token_expiracion DATETIME NULL,

    CONSTRAINT CK_usuario_rol CHECK (usu_rol IN ('Administrador', 'Empleado')),
    CONSTRAINT CK_usuario_estado CHECK (usu_estado IN ('A', 'I', 'B'))
);
GO

-- Índices para búsquedas frecuentes
CREATE INDEX IX_usuario_rol ON tbl_usuario(usu_rol);
CREATE INDEX IX_usuario_estado ON tbl_usuario(usu_estado);
GO

-- ============================================
-- TABLA DE PROVEEDORES
-- ============================================
CREATE TABLE tbl_proveedor(
    prv_id INT IDENTITY(1,1) PRIMARY KEY,
    prv_nombre VARCHAR(100) NOT NULL,
    prv_contacto VARCHAR(100) NULL,
    prv_telefono VARCHAR(20) NULL,
    prv_correo VARCHAR(100) NULL,
    prv_imagen_path VARCHAR(255) NULL,
    prv_fecha_creacion DATETIME DEFAULT GETDATE()
);
GO

-- ============================================
-- TABLA DE PRODUCTOS
-- ============================================
CREATE TABLE tbl_producto(
    pr_id INT IDENTITY(1,1) PRIMARY KEY,
    pr_nombre VARCHAR(100) NOT NULL,
    pr_descripcion VARCHAR(200) NOT NULL,
    pr_precio DECIMAL(10,2) NOT NULL,
    pr_fecha_creacion DATETIME DEFAULT GETDATE(),
    pr_categoria_id INT NOT NULL,
    pr_usuario_id INT NOT NULL,                -- Usuario que registró el producto
    pr_proveedor_id INT NULL,                  -- Proveedor al que se compró

    CONSTRAINT FK_producto_categoria FOREIGN KEY (pr_categoria_id) REFERENCES tbl_categoria(cat_id),
    CONSTRAINT FK_producto_usuario FOREIGN KEY (pr_usuario_id) REFERENCES tbl_usuario(usu_id),
    CONSTRAINT FK_producto_proveedor FOREIGN KEY (pr_proveedor_id) REFERENCES tbl_proveedor(prv_id)
);
GO

-- Índices para búsqueda y joins
CREATE INDEX IX_producto_nombre ON tbl_producto(pr_nombre);
CREATE INDEX IX_producto_categoria ON tbl_producto(pr_categoria_id);
CREATE INDEX IX_producto_usuario ON tbl_producto(pr_usuario_id);
CREATE INDEX IX_producto_proveedor ON tbl_producto(pr_proveedor_id);
GO

-- ============================================
-- TABLA DE MÚLTIPLES IMÁGENES POR PRODUCTO
-- ============================================
CREATE TABLE tbl_producto_imagen(
    pim_id INT IDENTITY(1,1) PRIMARY KEY,
    pr_id INT NOT NULL,
    pim_path VARCHAR(255) NOT NULL,            -- Ruta de la imagen
    pim_orden INT DEFAULT 0,                   -- Orden de visualización
    pim_descripcion VARCHAR(200) NULL,
    pim_fecha_subida DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_producto_imagen FOREIGN KEY (pr_id) REFERENCES tbl_producto(pr_id) ON DELETE CASCADE
);
GO

CREATE INDEX IX_producto_imagen_producto ON tbl_producto_imagen(pr_id);
GO

-- ============================================
-- PROCEDIMIENTO PARA RECUPERAR CONTRASEÑA (DESENCRIPTADA)
-- ============================================
CREATE PROCEDURE sp_recuperar_contrasena
    @correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @contrasena_plana VARCHAR(100);
    DECLARE @estado CHAR(1);

    SELECT @contrasena_plana = dbo.desencripta(usu_contrasena), @estado = usu_estado
    FROM tbl_usuario
    WHERE usu_correo = @correo;

    IF @estado IS NULL
    BEGIN
        RAISERROR('Correo no registrado', 16, 1);
        RETURN;
    END

    IF @estado != 'A'
    BEGIN
        RAISERROR('Usuario inactivo o bloqueado', 16, 1);
        RETURN;
    END

    -- En un entorno real se enviaría por correo, aquí solo se devuelve
    SELECT @contrasena_plana AS contrasena_desencriptada;
END;
GO

-- ============================================
-- DATOS INICIALES
-- ============================================
-- Insertar categorías de ejemplo
INSERT INTO tbl_categoria (cat_nombre, cat_descripcion) VALUES
('Electrónica', 'Productos electrónicos y gadgets'),
('Ropa', 'Prendas de vestir y accesorios'),
('Hogar', 'Artículos para el hogar y decoración');
GO

-- Insertar usuario administrador (contraseña: admin123)
INSERT INTO tbl_usuario (usu_nombre, usu_correo, usu_contrasena, usu_rol, usu_estado)
VALUES ('Administrador', 'admin@admin.com', dbo.encripta('admin123'), 'Administrador', 'A');

-- Insertar usuario empleado (contraseña: empleado123)
INSERT INTO tbl_usuario (usu_nombre, usu_correo, usu_contrasena, usu_rol, usu_estado)
VALUES ('Empleado Ejemplo', 'empleado@ejemplo.com', dbo.encripta('empleado123'), 'Empleado', 'A');
GO

-- Insertar proveedores de ejemplo
INSERT INTO tbl_proveedor (prv_nombre, prv_contacto, prv_telefono, prv_correo) VALUES
('Distribuidora Tecno', 'Carlos López', '0991234567', 'carlos@tecnologia.com'),
('Importadora del Sur', 'María García', '0987654321', 'ventas@importsur.com');
GO 

select * from tbl_usuario
select * from tbl_producto
select * from tbl_proveedor