/****** Object:  Table [dbo].[gift]    Script Date: 2013/10/8 下午 03:39:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gift]') AND type in (N'U'))
DROP TABLE [dbo].[gift]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gift](
	[gift_id] [int] IDENTITY(1,1) NOT NULL,
	[gift_no] varchar(20) NOT NULL,
	
	[point] [float] NOT NULL,
	[alert_level] [int] NOT NULL,
	[cost] [float] NOT NULL,

	[discount] [bit] NOT NULL,
	[discount_point] [float] NULL,
	[discount_active_date] [datetime] NULL,
	[discount_expiry_date] [datetime] NULL,

	[hot_item] [bit] NOT NULL,
	[hot_item_active_date] [datetime] NULL,
	[hot_item_expiry_date] [datetime] NULL,
	[hot_item_display_order] [int] NOT NULL,

	[display_public] [bit] NOT NULL,
	[display_active_date] [datetime] NOT NULL,
	[display_expiry_date] [datetime] NOT NULL,
	[redeem_active_date] [datetime] NOT NULL,
	[redeem_expiry_date] [datetime] NOT NULL,

	[status] [int] NOT NULL,
	
	[available_stock] [int] NOT NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_gift] PRIMARY KEY CLUSTERED 
(
	[gift_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] --TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO