/****** Object:  Table [dbo].[transaction]    Script Date: 2013/7/29 下午 04:32:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[transaction]') AND type in (N'U'))
DROP TABLE [dbo].[transaction]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[transaction](
	[transaction_id] [int] IDENTITY(1,1) NOT NULL,
	[type] INT NOT NULL, -- (refer to listing_item)
	[source_id] INT NOT NULL, -- source id to induce this transaction
	[location_id] INT NOT NULL, -- location id to induce this transaction

	[member_id] [int] NOT NULL,
		
	[point_change] [float] NOT NULL,
	[point_status] [int] NOT NULL, -- if one of earn entry is unrealized, then unrealized
	[point_expiry_date] [datetime] NULL, -- mark the nearest expiry date of all related earn entries

	[display] bit NOT NULL,

	[void_date] [datetime] NULL,
	[remark] [NVARCHAR](MAX) NULL,  -- remark for point adjustment, void reason tec

	[status] INT NOT NULL,  -- refer to TransactionStatus at listing_item
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_transaction] PRIMARY KEY CLUSTERED 
(
	[transaction_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO