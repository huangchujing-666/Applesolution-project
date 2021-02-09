/****** Object:  Table [dbo].[member_level_lang]    Script Date: 2013/7/7 下午 02:46:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[member_level_lang]') AND type in (N'U'))
DROP TABLE [dbo].[member_level_lang]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[member_level_lang](
	[level_lang_id] [int] IDENTITY(1,1) NOT NULL,
	[level_id] [int] NOT NULL,
	[lang_id] [int] NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[status] [int] NOT NULL,
	[crt_date] [datetime] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
 CONSTRAINT [PK_member_level_lang] PRIMARY KEY CLUSTERED 
(
	[level_lang_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


