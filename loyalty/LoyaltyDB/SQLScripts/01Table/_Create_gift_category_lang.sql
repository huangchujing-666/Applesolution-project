IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gift_category_lang]') AND type in (N'U'))
DROP TABLE [dbo].[gift_category_lang]
GO

/****** Object:  Table [dbo].[gift_category_lang]    Script Date: 2013/10/17 下午 12:25:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[gift_category_lang](
	[category_lang_id] [int] IDENTITY(1,1) NOT NULL,

	[category_id] [int] NOT NULL,
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
 CONSTRAINT [PK_gift_category_lang] PRIMARY KEY CLUSTERED 
(
	[category_lang_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

