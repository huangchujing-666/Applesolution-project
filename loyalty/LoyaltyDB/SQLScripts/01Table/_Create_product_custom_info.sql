/****** Object:  Table [dbo].[product_custom_info]    Script Date: 2013/7/29 下午 04:32:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[product_custom_info]') AND type in (N'U'))
DROP TABLE [dbo].[product_custom_info]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[product_custom_info](
	[info_id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] INT NOT NULL,
	[custom_field_id] int NOT NULL,
	[field_value] [nvarchar](max) NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_product_custom_info] PRIMARY KEY CLUSTERED 
(
	[info_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


