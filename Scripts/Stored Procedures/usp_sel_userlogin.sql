USE [AS]
GO
/****** Object:  StoredProcedure [dbo].[usp_sel_userlogin]    Script Date: 06/12/2017 23:20:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Taza Rojas
-- Create date: 06/12/2017
-- Description:	Selecciona todos losrefresh tokens
-- =============================================
CREATE PROCEDURE [usp_sel_userlogin]
(
	@nombreUsuario varchar(50),
	@password varchar(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	select	Id,
			Nombres,
			ApellidoPaterno,
			ApellidoMaterno,
			NombreUsuario,
			UrlFoto,
			CorreoElectronico
	from	Usuarios
	where	NombreUsuario = @nombreUsuario
			and [Password] = @password
			and Estado = 1
END

