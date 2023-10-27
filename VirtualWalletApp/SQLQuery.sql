USE [VirtualWalletDB]
GO
/****** Object:  Table [dbo].[DataTarjets]    Script Date: 26/10/2023 18:11:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataTarjets](
	[Logo] [varchar](max) NOT NULL,
	[Bank] [varchar](max) NOT NULL,
	[Emitter] [varchar](max) NOT NULL,
	[OwnerName] [varchar](max) NOT NULL,
	[TarjetNumber] [varchar](max) NOT NULL,
	[CVV] [varchar](50) NOT NULL,
	[ExpirationDate] [date] NOT NULL,
	[OwnerDNI] [int] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 26/10/2023 18:11:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[DNI] [int] NOT NULL,
	[Email] [varchar](max) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	[UserName] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[DNI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[DataTarjets]  WITH CHECK ADD  CONSTRAINT [FK_DataTarjets_Users] FOREIGN KEY([OwnerDNI])
REFERENCES [dbo].[Users] ([DNI])
GO
ALTER TABLE [dbo].[DataTarjets] CHECK CONSTRAINT [FK_DataTarjets_Users]
GO
/****** Object:  StoredProcedure [dbo].[CreateTarjet]    Script Date: 26/10/2023 18:11:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateTarjet]
    @Bank NVARCHAR(MAX),
    @Emitter NVARCHAR(MAX),
    @OwnerName NVARCHAR(MAX),
    @TarjetNumber NVARCHAR(MAX),
    @CVV NVARCHAR(50),
    @ExpirationDate DATE,
    @OwnerDNI INT,
    @Logo NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    -- Insertar datos en la tabla DataTarjets
    INSERT INTO DataTarjets (Logo, Bank, Emitter, OwnerName, TarjetNumber, CVV, ExpirationDate, OwnerDNI)
    VALUES ('/WalletLogo/' + @Logo, @Bank, @Emitter, @OwnerName, @TarjetNumber, @CVV, @ExpirationDate, @OwnerDNI);

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteTarjet]    Script Date: 26/10/2023 18:11:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteTarjet]
    @ID INT
AS
BEGIN
    -- Eliminar la tarjeta de la tabla (reemplaza 'DataTarjet' con el nombre real de tu tabla)
    DELETE FROM DataTarjet
    WHERE ID = @ID;

    RETURN 0;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetTarjetDataByEmail]    Script Date: 26/10/2023 18:11:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Crear el stored procedure para obtener los datos de tarjetas por Email
CREATE PROCEDURE [dbo].[GetTarjetDataByEmail]
    @Email VARCHAR(MAX)
AS
BEGIN
    DECLARE @UserDNI INT;

    -- Obtener el DNI del usuario por email
    SELECT @UserDNI = [DNI]
    FROM [dbo].[Users]
    WHERE [Email] = @Email;

    -- Si se encontró el DNI, obtener datos de tarjetas
    IF @UserDNI IS NOT NULL
    BEGIN
        -- Seleccionar los datos de tarjetas donde el OwnerDNI sea igual al DNI del usuario
        SELECT 
            [Logo],
            [Bank],
            [Emitter],
            [OwnerName],
            [TarjetNumber],
            [CVV],
            [ExpirationDate],
			[ID],
            [OwnerDNI]
        FROM [dbo].[DataTarjets]
        WHERE [OwnerDNI] = @UserDNI;
    END
    ELSE
    BEGIN
        PRINT 'Usuario no encontrado por email.';
    END
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateTarjet]    Script Date: 26/10/2023 18:11:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateTarjet]
    @ID INT,
    @Bank NVARCHAR(255),
    @Emitter NVARCHAR(255),
    @OwnerName NVARCHAR(255),
    @NumTarjet NVARCHAR(255),
    @CVV NVARCHAR(10),
    @ExpirationDate DATETIME,
    @Logo NVARCHAR(255),
    @OwnerDNI INT
AS
BEGIN
    -- Actualizar la tarjeta en tu tabla (reemplaza 'DataTarjet' con el nombre real de tu tabla)
    UPDATE DataTarjet
    SET
        Bank = @Bank,
        Emitter = @Emitter,
        OwnerName = @OwnerName,
        NumTarjet = @NumTarjet,
        CVV = @CVV,
        ExpirationDate = @ExpirationDate,
        Logo = @Logo,
        OwnerDNI = @OwnerDNI
    WHERE
        ID = @ID;

    RETURN 0;
END;
GO
/****** Object:  StoredProcedure [dbo].[ValidateLoginData]    Script Date: 26/10/2023 18:11:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[ValidateLoginData]
    @Email VARCHAR(MAX),
    @Password VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StoredPassword VARCHAR(MAX)

    -- Inicializa el resultado en 0 (credenciales inválidas) por defecto.
    DECLARE @Result INT = 0

    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        -- Busca la contraseña correspondiente al nombre de usuario proporcionado
        SELECT @StoredPassword = Password
        FROM Users
        WHERE Email = @Email

        -- Verifica si la contraseña proporcionada coincide con la almacenada
        IF @StoredPassword IS NOT NULL AND @StoredPassword = @Password
        BEGIN
            -- Las credenciales son válidas
            SET @Result = 1
        END
    END

    -- Devuelve el resultado
    SELECT @Result AS Result
END
GO
