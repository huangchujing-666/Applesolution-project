/****** Object:  Table [dbo].[gift_lang]    Script Date: 2013/10/8 下午 03:39:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gift_lang](
	[gift_lang_id] [int] IDENTITY(1,1) NOT NULL,
	[gift_id] [int] NOT NULL,
	[lang_id] [int] NOT NULL,
	
	[name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NOT NULL,

	[status] [int] NOT NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_gift_lang] PRIMARY KEY CLUSTERED 
(
	[gift_lang_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] --TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO