/****** Object:  Table [dbo].[passcode_registry]    Script Date: 2013/8/7 下午 06:07:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[passcode_registry]') AND type in (N'U'))
DROP TABLE [dbo].[passcode_registry]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[passcode_registry](
	[registry_id] [bigint] IDENTITY(1,1) NOT NULL,
	[passcode_id] [bigint] NOT NULL, 
	[member_id] int NOT NULL,

	--[status] [int] NOT NULL,

	--[void_date] [datetime] NULL,
	--[void_reason] [NVARCHAR](MAX) NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,
	
	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
	
	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_passcode_registry] PRIMARY KEY CLUSTERED 
(
	[registry_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO