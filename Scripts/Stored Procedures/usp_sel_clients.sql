USE [AS]
GO
/****** Object:  StoredProcedure [usp_sel_clients]    Script Date: 06/12/2017 1:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Taza Rojas
-- Create date: 06/12/2017
-- Description:	Selects clients
-- =============================================
CREATE PROCEDURE [usp_sel_clients]
(
	@clientId varchar(100)
)
AS
BEGIN
	SET NOCOUNT ON;

	select	Id,
			[Secret],
			[Name],
			ApplicationType,
			Active,
			RefreshTokenLifeTime,
			AllowedOrigin
	from	Clients
	where	(@clientId = '' or Id = @clientId)
END

