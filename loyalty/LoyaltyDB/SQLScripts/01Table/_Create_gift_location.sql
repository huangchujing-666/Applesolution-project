/****** Object:  Table [dbo].[gift_location]    Script Date: 2013/8/7 下午 06:07:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gift_location]') AND type in (N'U'))
DROP TABLE [dbo].[gift_location]
GO

/****** Object:  Table [dbo].[gift_location]    Script Date: 2013/10/17 下午 05:53:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[gift_location](
	[gift_location_id] [int] IDENTITY(1,1) NOT NULL,
	[gift_id] [int] NOT NULL,
	[location_id] [int] NOT NULL,
	[status] [int] NOT NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_gift_location] PRIMARY KEY CLUSTERED 
(
	[gift_location_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO