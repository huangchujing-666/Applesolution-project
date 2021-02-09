/****** Object:  Table [dbo].[privilege]    Script Date: 2013/7/7 下午 02:46:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[privilege]') AND type in (N'U'))
DROP TABLE [dbo].[privilege]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[privilege](
	[privilege_id] [int] IDENTITY(1,1) NOT NULL,
	[object_type] [int] NOT NULL,
	[object_id] [int] NOT NULL,
	[section_id] [int] NOT NULL,
	[read_status] [int] NOT NULL,
	[insert_status] [int] NOT NULL,
	[update_status] [int] NOT NULL,
	[delete_status] [int] NOT NULL,
	[status] [int] NOT NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_privilege] PRIMARY KEY CLUSTERED 
(
	[privilege_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


