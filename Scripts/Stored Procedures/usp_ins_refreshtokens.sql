USE [AS]
GO
/****** Object:  StoredProcedure [usp_ins_refreshtokens]    Script Date: 06/12/2017 1:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Roy Taza Rojas
-- Create date: 06/12/2017
-- Description:	Inserts refresh token
-- =============================================
CREATE PROCEDURE [usp_ins_refreshtokens]
(
	@xmlRefreshTokens nvarchar(max)
)
AS
BEGIN try
	SET NOCOUNT ON;
	begin transaction;

	declare @idata int
	declare	@dataRefreshTokens table(
		Id				varchar(500),
		[Subject]		varchar(50),
		ClientId		varchar(50),
		IssuedUtc		datetime,
		ExpiresUtc		datetime,
		ProtectedTicket	varchar(max)
	)

	exec sp_xml_preparedocument @idata OUTPUT, @xmlRefreshTokens

	insert into @dataRefreshTokens(Id, [Subject], ClientId, IssuedUtc, ExpiresUtc, ProtectedTicket)
	select 
			Id,
			[Subject],
			ClientId,
			IssuedUtc,
			ExpiresUtc,
			ProtectedTicket
	from openxml (@idata, '/ArrayOfRefreshToken/RefreshToken', 2)
	with (
			Id					varchar(500)	'@RefreshTokenId',
			[Subject]			varchar(50)		'@Subject',
			ClientId			varchar(50)		'@ClientId',
			IssuedUtc			datetime		'@IssuedUtc',
			ExpiresUtc			datetime		'@ExpiresUtc',
			ProtectedTicket		varchar(max)	'@ProtectedTicket'
	)
		
	exec sp_xml_removedocument @idata;
	
	
	--delete	A
	--from	oauth.RefreshTokens A
	--		inner join @dataRefreshTokens B on A.[Subject] = B.[Subject] and A.ClientId = B.ClientId

	insert into RefreshTokens(Id, [Subject], ClientId, IssuedUtc, ExpiresUtc, ProtectedTicket)
	select	Id, [Subject], ClientId, IssuedUtc, ExpiresUtc, ProtectedTicket
	from	@dataRefreshTokens A

	commit;
end try
begin catch
	if (@@trancount > 0) rollback;
	declare @errmsg nvarchar(4000), @errseverity int
	select @errmsg = ERROR_MESSAGE(), @errseverity = ERROR_SEVERITY()
	raiserror(@errmsg, @errseverity, 1)
end catch

