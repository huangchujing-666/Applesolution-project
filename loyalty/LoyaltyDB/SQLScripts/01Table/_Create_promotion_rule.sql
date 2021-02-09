/****** Object:  Table [dbo].[promotion_rule]    Script Date: 2013/10/8 下午 03:39:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[promotion_rule]') AND type in (N'U'))
DROP TABLE [dbo].[promotion_rule]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[promotion_rule](
	[rule_id] [int] IDENTITY(1,1) NOT NULL,
	[name] nvarchar(100) NOT NULL,

	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,

	[type] [int] NOT NULL,
	[conjunction] [int] NOT NULL,
	[transaction_criteria] [int] NULL,

	[special_criteria_type] [int] NULL,
	[special_criteria_value] [int] NULL,

	[earn_point_type] [int] NULL,
	[earn_point_value] [float] NULL,

	[earn_gift_id] [int] NULL,
	[earn_gift_quantity] [float] NULL,

	[redeem_discount] [float] NULL,


	[status] [int] NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_promotion_rule] PRIMARY KEY CLUSTERED 
(
	[rule_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] --TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO