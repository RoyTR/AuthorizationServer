USE [AS]
GO
/****** Object:  StoredProcedure [dbo].[usp_del_refreshtoken]    Script Date: 07/12/2017 0:36:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Taza Rojas
-- Create date: 06/12/2017
-- Description:	Removes expired refresh tokens
-- =============================================
CREATE PROCEDURE [usp_del_expiredrefreshtokens]
(
	@param varchar(1)
)
AS
BEGIN try
	SET NOCOUNT ON;
	begin transaction;

	delete	RefreshTokens
	where	GETUTCDATE() > ExpiresUtc

	commit;
end try
begin catch
	if (@@trancount > 0) rollback;
	declare @errmsg nvarchar(4000), @errseverity int
	select @errmsg = ERROR_MESSAGE(), @errseverity = ERROR_SEVERITY()
	raiserror(@errmsg, @errseverity, 1)
end catch

