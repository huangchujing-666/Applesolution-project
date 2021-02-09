/****** Object:  Table [dbo].[basic_rule]    Script Date: 2013/10/8 下午 03:39:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[basic_rule]') AND type in (N'U'))
DROP TABLE [dbo].[basic_rule]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[basic_rule](
	[basic_rule_id] [int] IDENTITY(1,1) NOT NULL,

	[type] [int] NOT NULL,
	[target_id] [int] NOT NULL, -- service plan id / product id 
	
	[member_level_id] [int] NOT NULL,
	[memebr_category_id] [int] NOT NULL,

	-- ratio or discrete point
	[ratio_payment] [float] NOT NULL,
	[ratio_point] [float] NOT NULL,
	[point] [float] NOT NULL,

	[point_expiry_month] [int] NOT NULL,
	[remark] [nvarchar](max) NULL,

	[status] [int] NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_basic_rule] PRIMARY KEY CLUSTERED 
(
	[basic_rule_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

SET ANSI_PADDING OFF
GO