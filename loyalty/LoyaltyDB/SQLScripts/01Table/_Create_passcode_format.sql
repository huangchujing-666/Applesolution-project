/****** Object:  Table [dbo].[passcode_format]    Script Date: 2013/8/8 下午 04:25:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[passcode_format](
	[format_id] [int] IDENTITY(1,1) NOT NULL,
	[passcode_format] [varchar](30) NOT NULL,
	[number_combination] [bigint] NOT NULL,
	[safetyLimit_precent] [float] NOT NULL,
	[maximum_generate] [bigint] NOT NULL,
	[expiry_month] int NOT NULL,

	[status] [int] NOT NULL,
	[crt_date] [datetime] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_passcode_format] PRIMARY KEY CLUSTERED 
(
	[format_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

