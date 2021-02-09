/****** Object:  Table [dbo].[passcode_prefix]    Script Date: 2013/8/8 下午 04:25:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[passcode_prefix](
	[prefix_id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [int] NOT NULL,
	[format_id] [bigint] NOT NULL,
	[prefix_value] [varchar](10) NOT NULL,
	[current_generated] [bigint] NOT NULL,
	
	[usage_precent] [float] NOT NULL,
	
	[status] [int] NOT NULL,
	[crt_date] [datetime] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_passcode_prefix] PRIMARY KEY CLUSTERED 
(
	[prefix_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


