/****** Object:  Table [dbo].[promotion_rule_purchase_criteria]    Script Date: 2013/10/8 下午 03:39:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[promotion_rule_purchase_criteria]') AND type in (N'U'))
DROP TABLE [dbo].[promotion_rule_purchase_criteria]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[promotion_rule_purchase_criteria](
	[rec_id] [int] IDENTITY(1,1) NOT NULL,
	
	[rule_id] [int] NOT NULL,
	[target_type] [int]  NOT NULL,
	[target_id] [int] NOT NULL,
	[criteria] [int] NOT NULL,
	[quantity] [int] NULL,
	[point] [float] NULL, 

	[status] [int] NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_promotion_rule_purchase_criteria] PRIMARY KEY CLUSTERED 
(
	[rec_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] --TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO