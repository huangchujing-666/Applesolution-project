/****** Object:  Table [dbo].[log]    Script Date: 2013/8/9 下午 06:31:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[log]') AND type in (N'U'))
DROP TABLE [dbo].[log]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[log](
	[log_id] [int] IDENTITY(1,1) NOT NULL,
	
	[action_ip] [varchar](50) NOT NULL,
	[action_channel] [int] NOT NULL,		-- action channel type: refer listing_item.sql

	[action_type] [int] NOT NULL,	-- CRUD: 1:create, 2:read, 3:update, 4:delete / 5:login, 6:logout, 

	[target_obj_id] [bigint] NULL,
	[target_obj_type_id] [bigint] NULL, -- for non-main target object which is not system object
	[target_obj_name] [nvarchar](100) NULL, -- for non-main target object which is not system object

	[action_detail] [nvarchar](MAX) NULL,  --any additional info

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0
 CONSTRAINT [PK_log] PRIMARY KEY CLUSTERED 
(
	[log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO