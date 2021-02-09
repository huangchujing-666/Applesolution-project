IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gift_redemption]') AND type in (N'U'))
DROP TABLE [dbo].[gift_redemption]
GO

/****** Object:  Table [dbo].[gift_redemption]    Script Date: 2013/10/20 下午 06:11:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gift_redemption](
	[redemption_id] [int] IDENTITY(1,1) NOT NULL,
	
	[transaction_id] [int] NOT NULL,
	[redemption_code] [varchar](20) NOT NULL,
	[redemption_channel] [int] NOT NULL,  -- web, mobile, cms

	[member_id] [int] NOT NULL,
	[gift_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,

	[point_used] [float] NOT NULL, 
	
	[redemption_status] [int] NOT NULL, -- not collect, collected, return
	[collect_date] [datetime] NULL,
	[collect_location_id] [int] NOT NULL,

	[void_date] [datetime] NULL,
	[void_user_id] [int] NULL,

	[remark] [nvarchar](max) NULL,
		
	[status] [int] NOT NULL,  -- active, inactive
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_gift_redemption] PRIMARY KEY CLUSTERED 
(
	[redemption_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

