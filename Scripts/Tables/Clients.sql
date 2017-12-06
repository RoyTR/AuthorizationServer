USE [AS]
GO

/****** Object:  Table [Clients]    Script Date: 06/12/2017 1:06:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [Clients](
	[Id] [varchar](25) NOT NULL,
	[Secret] [varchar](250) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ApplicationType] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[RefreshTokenLifeTime] [int] NOT NULL,
	[AllowedOrigin] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFFe
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lifetime expressed in minutes.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Clients', @level2type=N'COLUMN',@level2name=N'RefreshTokenLifeTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sites the client accepts the request.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Clients', @level2type=N'COLUMN',@level2name=N'AllowedOrigin'
GO


