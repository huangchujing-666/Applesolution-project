IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[member_level_change]') AND type in (N'U'))
DROP TABLE [dbo].[member_level_change]
GO

/****** Object:  Table [dbo].[member_level_change]    Script Date: 2013/7/17 上午 11:50:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[member_level_change](
	[change_id] [int] IDENTITY(1,1) NOT NULL,

	[member_id] [int] NOT NULL,
	[source_type] [int] NOT NULL,
	[change_type] [int] NOT NULL,
	[reason_type] [int] NOT NULL,
	[old_member_level_id] [INT] NOT NULL,
	[new_member_level_id] [INT] NOT NULL,
	
	[remark] [nvarchar] NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,
	
	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_member_level_change] PRIMARY KEY CLUSTERED 
(
	[change_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


