/****** Object:  Table [dbo].[combine_redemption]    Script Date: 2013/10/8 下午 03:39:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[combine_redemption]') AND type in (N'U'))
DROP TABLE [dbo].[combine_redemption]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[combine_redemption](
	[combine_id] [int] IDENTITY(1,1) NOT NULL,
	[member_id] [int] NOT NULL,
	[coupon_id] [int] NOT NULL,
	[position] [int] NOT NULL,
	[no_of_ppl] [int] NOT NULL,
	[join_combine_id] [int] NOT NULL,
	[notified_host] [int] NOT NULL,
	
	[status] [int] NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_combine_redemption] PRIMARY KEY CLUSTERED 
(
	[combine_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] --TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO