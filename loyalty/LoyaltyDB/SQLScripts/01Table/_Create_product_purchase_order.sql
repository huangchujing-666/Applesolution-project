--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[product_purchase]') AND type in (N'U'))
--DROP TABLE [dbo].[product_purchase]
--GO

--/****** Object:  Table [dbo].[product_purchase]    Script Date: 2013/7/29 下午 04:32:33 ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--CREATE TABLE [dbo].[product_purchase](
--	[purchase_id] [int] IDENTITY(1,1) NOT NULL,
	
--	[transaction_id] [int] NOT NULL,

--	[member_id] [int] NOT NULL,
--	[product_id] [int] NOT NULL,
--	[quantity] [int] NOT NULL,

--	[status] [int] NOT NULL, -- refer to ProductPurchaseStatus in listing_item

--	[crt_date] [datetime] NOT NULL,
--	[crt_by_type] [int] NOT NULL,
--	[crt_by] [int] NOT NULL,

--	[upd_date] [datetime] NOT NULL,
--	[upd_by_type] [int] NOT NULL,
--	[upd_by] [int] NOT NULL,

--	[record_status] [int] NOT NULL DEFAULT 0

-- CONSTRAINT [PK_product_purchase] PRIMARY KEY CLUSTERED 
--(
--	[purchase_id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--) ON [PRIMARY]

--GO