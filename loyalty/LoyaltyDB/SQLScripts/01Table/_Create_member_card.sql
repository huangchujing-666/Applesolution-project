/****** Object:  Table [dbo].[member_card]    Script Date: 2013/10/8 下午 03:39:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[member_card]') AND type in (N'U'))
DROP TABLE [dbo].[member_card]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[member_card](
	[card_id] [int] IDENTITY(1,1) NOT NULL,
	
	[member_id] [int] NOT NULL,
	[card_no] varchar(20) NOT NULL,
	[card_version] [int] NOT NULL,
	[card_type] [int] NOT NULL,
	[card_status] [int] NOT NULL,
	[issue_date] [datetime] NULL,
	
	[old_card_id] [int] NOT NULL,
	[remark] [nvarchar](max) NULL,

	[status] [int] NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_member_card] PRIMARY KEY CLUSTERED 
(
	[card_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] --TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO