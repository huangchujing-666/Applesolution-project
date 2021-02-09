/****** Object:  Table [dbo].[transaction_use]    Script Date: 2013/10/18 下午 05:53:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[transaction_use]') AND type in (N'U'))
DROP TABLE [dbo].[transaction_use]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[transaction_use](
	[use_id] [INT] IDENTITY(1,1) NOT NULL,
	[transaction_id] INT NOT NULL,

	[earn_id] [int] NOT NULL, -- foregin key of [transaction_earn]
	[point_used] [float] NOT NULL,

	[status] [int] NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0
 CONSTRAINT [PK_transaction_use] PRIMARY KEY CLUSTERED 
(
	[use_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
