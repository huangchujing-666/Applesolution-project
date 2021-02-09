/****** Object:  Table [dbo].[table_structure]    Script Date: 2013/7/7 下午 02:46:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[table_structure]') AND type in (N'U'))
DROP TABLE [dbo].[table_structure]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[table_structure](
	[table_id] [int] IDENTITY(1,1) NOT NULL,
	[module] [nvarchar](50) NULL,
	[table_name] [varchar](100) NOT NULL,
	[field_name] [nvarchar](max) NULL,
	[json_name] [nvarchar](100) NULL,
	[display_label] [nvarchar](100) NOT NULL,
	[data_type] [varchar](50) NOT NULL,
	[field_tab_index] [int] NULL,
	[allow_blank] [bit] NOT NULL,
	[default_value] [nvarchar](500) NULL,
	[data_source] [nvarchar](500) NULL,
	[field_group] [nvarchar](100) NULL,
	[search_tab_index] [int] NULL,
	[show_in_search] [bit] NOT NULL,
	[search_type] [varchar](50) NULL,
	[show_in_grid] [bit] NOT NULL,
	[grid_type] [nvarchar](100) NULL,
	[grid_width] [varchar](10) NULL,
	[grid_tab_index] [int] NULL,
	[show_in_edit] [bit] NOT NULL,
	[edit_index] [int] NULL,
	[status] [int] NOT NULL,

	[crt_date] [datetime] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

 CONSTRAINT [PK_table_structure] PRIMARY KEY CLUSTERED 
(
	[table_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


