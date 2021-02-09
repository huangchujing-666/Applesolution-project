/****** Object:  Table [dbo].[transaction_history]    Script Date: 2013/7/29 下午 04:32:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[transaction_earn]') AND type in (N'U'))
DROP TABLE [dbo].[transaction_earn]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[transaction_earn](
	[earn_id] INT IDENTITY(1,1) NOT NULL,

	[transaction_id] INT NOT NULL,
	[source_type] INT NOT NULL, -- source type to induce this earn, eg bonus point
	[source_id] INT NOT NULL, -- source id to induce this earn

	[point_earn] [float] NOT NULL,
	[point_status] [int] NOT NULL,
	[point_expiry_date] [datetime] NOT NULL,
	[point_used] [float] NOT NULL,
	
	[status] INT NOT NULL, -- refer to TransactionStatus at listing_item

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] INT NOT NULL,
	[crt_by] INT NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] INT NOT NULL,
	[upd_by] INT NOT NULL,

	[record_status] INT NOT NULL DEFAULT 0

 CONSTRAINT [PK_transaction_earn] PRIMARY KEY CLUSTERED 
(
	[earn_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO