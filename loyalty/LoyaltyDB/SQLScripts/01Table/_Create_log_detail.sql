/****** Object:  Table [dbo].[log_detail]    Script Date: 2013/8/9 下午 06:31:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[log_detail]') AND type in (N'U'))
DROP TABLE [dbo].[log_detail]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[log_detail](
	[log_detail_id] [bigint] IDENTITY(1,1) NOT NULL,
	
	[log_id] [int] NOT NULL,
	[field_name] [nvarchar](100) NOT NULL,
	[old_value] [nvarchar](MAX) NOT NULL,
	[new_value] [nvarchar](MAX) NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0
 CONSTRAINT [PK_log_detail] PRIMARY KEY CLUSTERED 
(
	[log_detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO