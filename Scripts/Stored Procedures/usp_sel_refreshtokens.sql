USE [AS]
GO
/****** Object:  StoredProcedure [usp_sel_refreshtokens]    Script Date: 06/12/2017 1:13:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Taza Rojas
-- Create date: 06/12/2017
-- Description:	Selects refresh tokens
-- =============================================
CREATE PROCEDURE [usp_sel_refreshtokens]
(
	@refreshTokenId varchar(100)
)
AS
BEGIN
	SET NOCOUNT ON;

	select	Id,
			[Subject],
			ClientId,
			IssuedUtc,
			ExpiresUtc,
			ProtectedTicket
	from	RefreshTokens
	where	(@refreshTokenId = '' or Id = @refreshTokenId)
			and GETUTCDATE() < ExpiresUtc
END

