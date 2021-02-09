/****** Object:  Table [dbo].[custom_field]    Script Date: 2013/7/29 下午 04:32:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[custom_field](
	[field_id] [int] IDENTITY(1,1) NOT NULL,
	[module] [varchar](50) NOT NULL,
	[field_name] [nvarchar](max) NOT NULL,
	[field_type] [varchar](10) NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_custom_field] PRIMARY KEY CLUSTERED 
(
	[field_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO