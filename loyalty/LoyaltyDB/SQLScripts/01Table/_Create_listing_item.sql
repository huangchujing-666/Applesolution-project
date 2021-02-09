/****** Object:  Table [dbo].[listing_item]    Script Date: 2013/7/7 下午 02:46:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[listing_item]') AND type in (N'U'))
DROP TABLE [dbo].[listing_item]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[listing_item](
	[list_item_id] [int] IDENTITY(1,1) NOT NULL,
	[list_id] [int] NOT NULL,
	[value] [int] NOT NULL,
	[name] [nvarchar](300) NOT NULL,
	[display_order] [int] NOT NULL,
	[status] [int] NOT NULL,
	[crt_date] [datetime] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
 CONSTRAINT [PK_listing_item] PRIMARY KEY CLUSTERED 
(
	[list_item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


