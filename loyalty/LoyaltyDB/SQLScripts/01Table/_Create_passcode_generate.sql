/****** Object:  Table [dbo].[passcode_generate]    Script Date: 2013/8/9 下午 06:31:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[passcode_generate](
	[generate_id] [int] IDENTITY(1,1) NOT NULL,
	[noToGenerate] BIGINT NOT NULL,
	[generateCompleteCounter] BIGINT NOT NULL,
	[insertErrorCounter] BIGINT NOT NULL,
	[error_messgae] NVARCHAR(MAX) NULL,
	[generate_status] [int] NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_passcode_generate] PRIMARY KEY CLUSTERED 
(
	[generate_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


