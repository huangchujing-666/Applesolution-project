/****** Object:  Table [dbo].[location]    Script Date: 2013/10/8 下午 03:39:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[location]') AND type in (N'U'))
DROP TABLE [dbo].[location]
GO

/****** Object:  Table [dbo].[location]    Script Date: 2013/10/16 下午 03:35:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[location](
	[location_id] [int] IDENTITY(1,1) NOT NULL,
	[location_no] [varchar](20) NOT NULL,
	[type] [int] NOT NULL,

	[photo_file_name] [varchar](1000) NOT NULL,
	[photo_file_extension] [varchar](10) NOT NULL,

	[latitude] [float] NULL,
	[longitude] [float] NULL,

	[phone] [varchar](12) NOT NULL,
	[fax] [varchar](12) NOT NULL,

	[address_district] [int] NOT NULL,
	[address_region] [int] NOT NULL,

	[display_order] [int] NOT NULL,
	[status] [int] NOT NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0
 CONSTRAINT [PK_location] PRIMARY KEY CLUSTERED 
(
	[location_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] --TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO