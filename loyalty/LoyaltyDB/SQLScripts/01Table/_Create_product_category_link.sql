/****** Object:  Table [dbo].[product_category_link]    Script Date: 2013/8/7 下午 06:07:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[product_category_link]') AND type in (N'U'))
DROP TABLE [dbo].[product_category_link]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[product_category_link](
	[link_id] [int] IDENTITY(1,1) NOT NULL,

	[product_id]  [int] NOT NULL,
	[category_id] [int] NOT NULL,
	[display_order] [int] NOT NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
	
	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_product_category_link] PRIMARY KEY CLUSTERED 
(
	[link_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO