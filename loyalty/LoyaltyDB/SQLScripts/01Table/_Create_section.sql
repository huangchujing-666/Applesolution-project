/****** Object:  Table [dbo].[section]    Script Date: 2013/7/7 下午 02:46:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[section]') AND type in (N'U'))
DROP TABLE [dbo].[section]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[section](
	[section_id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NOT NULL,
	[leaf] [int] NOT NULL,

	[name] [nvarchar](100) NOT NULL,	
	[link] [varchar](100) NOT NULL,
	[module] [varchar](40) NOT NULL,
	[object_type] [int] NOT NULL,
	[icon] [int] NOT NULL,

	[display] [int] NOT NULL,
	[display_order] [int] NOT NULL,
	
	[grid_page_size] [int] NOT NULL,
	[grid_search_hidden] [int] NOT NULL,
	[grid_add_hidden] [int] NOT NULL,
	[grid_delete_hidden] [int] NOT NULL,
	[grid_export_hidden] [int] NOT NULL,
	[grid_checkbox_hidden] [int] NOT NULL,
	
	[status] [int] NOT NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_section] PRIMARY KEY CLUSTERED 
(
	[section_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO