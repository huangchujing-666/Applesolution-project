/****** Object:  Table [dbo].[gift_photo]    Script Date: 2013/8/7 下午 06:07:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gift_photo]') AND type in (N'U'))
DROP TABLE [dbo].[gift_photo]
GO

/****** Object:  Table [dbo].[gift_photo]    Script Date: 2013/10/16 上午 10:46:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[gift_photo](
	[gift_photo_id] [int] IDENTITY(1,1) NOT NULL,
	[gift_id] [int] NOT NULL,

	[file_name] [nvarchar](1000) NOT NULL,
	[file_extension] [varchar](8) NOT NULL,
	
	[name] [nvarchar](100) NOT NULL,
	[caption] [nvarchar](200) NOT NULL,
	
	[display_order] [int] NOT NULL,
	[status] [int] NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_gift_photo] PRIMARY KEY CLUSTERED 
(
	[gift_photo_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


