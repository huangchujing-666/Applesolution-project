/****** Object:  Table [dbo].[member_level]    Script Date: 2013/7/7 下午 02:46:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[member_level]') AND type in (N'U'))
DROP TABLE [dbo].[member_level]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[member_level](
	[level_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[point_required] [float] NOT NULL,
	[redeem_discount] [float] NOT NULL,
	[display_order] [int] NOT NULL,
	[status] [int] NOT NULL,
	[crt_date] [datetime] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
 CONSTRAINT [PK_member_level] PRIMARY KEY CLUSTERED 
(
	[level_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


