USE [AS]
GO
/****** Object:  StoredProcedure [usp_del_refreshtoken]    Script Date: 06/12/2017 1:12:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Taza Rojas
-- Create date: 06/12/2017
-- Description:	Removes refresh token
-- =============================================
CREATE PROCEDURE [usp_del_refreshtoken]
(
	@refreshTokenId varchar(100)
)
AS
BEGIN try
	SET NOCOUNT ON;
	begin transaction;

	delete	RefreshTokens
	where	Id = @refreshTokenId

	commit;
end try
begin catch
	if (@@trancount > 0) rollback;
	declare @errmsg nvarchar(4000), @errseverity int
	select @errmsg = ERROR_MESSAGE(), @errseverity = ERROR_SEVERITY()
	raiserror(@errmsg, @errseverity, 1)
end catch

