/****** Object:  Table [dbo].[location_lang]    Script Date: 2013/10/8 下午 03:39:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[location_lang]') AND type in (N'U'))
DROP TABLE [dbo].[location_lang]
GO

/****** Object:  Table [dbo].[location_lang]    Script Date: 2013/10/16 下午 03:42:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[location_lang](
	[location_lang_id] [int] IDENTITY(1,1) NOT NULL,
	[location_id] [int] NOT NULL,
	[lang_id] [int] NOT NULL,
	
	[name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[operation_info] [nvarchar](100) NOT NULL,

	[address_unit] [nvarchar](100) NOT NULL,
	[address_building] [nvarchar](100) NOT NULL,
	[address_street] [nvarchar](100) NOT NULL,

	[status] [int] NOT NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0
 CONSTRAINT [PK_location_lang] PRIMARY KEY CLUSTERED 
(
	[location_lang_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
