/****** Object:  Table [dbo].[passcode_failImport]    Script Date: 2013/8/7 下午 06:07:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[passcode_failImport]') AND type in (N'U'))
DROP TABLE [dbo].[passcode_failImport]
GO

/****** Object:  Table [dbo].[passcode_failImport]    Script Date: 2013/8/9 下午 06:31:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[passcode_failImport](
	[fail_id] [bigint] IDENTITY(1,1) NOT NULL,
	[generate_id] [int] NOT NULL,
	[excel_row_no] [int] NOT NULL,
	[passcode_id] [bigint] NOT NULL,
	[error_message] [nvarchar](max),

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

CONSTRAINT [PK_passcode_failImport] PRIMARY KEY CLUSTERED
(
	[fail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO