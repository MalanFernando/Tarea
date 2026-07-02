CREATE DATABASE BaseTarea3;
GO

USE BaseTarea3;
GO

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

CREATE TABLE tbl_usuario(
    usu_id INT IDENTITY(1,1) PRIMARY KEY,
    usu_nombre VARCHAR(50) NOT NULL,
    usu_correo VARCHAR(100) NOT NULL UNIQUE,
    usu_correo_secundario VARCHAR(100) NULL,
    usu_contrasena VARBINARY(MAX) NOT NULL,
    usu_cedula VARCHAR(10) NULL,
    usu_celular VARCHAR(10) NULL,
    usu_rol VARCHAR(20) NOT NULL,
    usu_fecha_nacimiento DATE NULL,
    usu_estado CHAR(1) DEFAULT 'A',
    usu_intentos INT DEFAULT 0,
    usu_fecha_creacion DATETIME DEFAULT GETDATE(),

    CONSTRAINT CK_usuario_rol CHECK (usu_rol IN ('Admin', 'Usuario')),
    CONSTRAINT CK_usuario_estado CHECK (usu_estado IN ('A', 'I', 'B'))
);
GO

CREATE INDEX IX_usuario_rol ON tbl_usuario(usu_rol);
CREATE INDEX IX_usuario_estado ON tbl_usuario(usu_estado);
GO

INSERT INTO tbl_usuario (usu_nombre, usu_correo, usu_contrasena, usu_rol, usu_estado)
VALUES ('Administrador', 'admin@admin.com', dbo.encripta('admin123'), 'Admin', 'A');

INSERT INTO tbl_usuario (usu_nombre, usu_correo, usu_contrasena, usu_rol, usu_estado)
VALUES ('Usuario Ejemplo', 'usuario@ejemplo.com', dbo.encripta('usuario123'), 'Usuario', 'A');
GO

SELECT * FROM tbl_usuario
