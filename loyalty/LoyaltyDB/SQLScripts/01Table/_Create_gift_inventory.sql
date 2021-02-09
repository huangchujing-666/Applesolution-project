/****** Object:  Table [dbo].[gift_inventory]    Script Date: 2013/10/8 下午 03:39:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gift_inventory]') AND type in (N'U'))
DROP TABLE [dbo].[gift_inventory]
GO

/****** Object:  Table [dbo].[gift_inventory]    Script Date: 2013/10/17 下午 08:23:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[gift_inventory](
	[inventory_id] [int] IDENTITY(1,1) NOT NULL,
	
	[source_id] [int] NOT NULL,	-- source id to indice stock change
	[location_id] [int] NOT NULL,
	[gift_id] [int] NOT NULL,
	
	[stock_change_type] [int] NOT NULL,  -- StockChangeType
	[stock_change] [int] NOT NULL,
	
	[remark] [nvarchar](max) NULL,
	
	[status] [int] NOT NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_gift_inventory] PRIMARY KEY CLUSTERED 
(
	[inventory_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO