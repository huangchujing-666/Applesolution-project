/****** Object:  Table [dbo].[service_payment]    Script Date: 2013/10/8 下午 03:39:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[service_payment]') AND type in (N'U'))
DROP TABLE [dbo].[service_payment]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[service_payment](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	
	[transaction_id] [int] NOT NULL,
	[invoice_no] [varchar](200) NOT NULL,

	[member_id] [int] NOT NULL,
	[member_service_id] [int] NOT NULL,
	[plan_id] [int] NOT NULL,
	[service_start_date] [datetime] NULL,
	[service_end_date] [datetime] NULL,
	
	[amount] [float] NOT NULL,
	[paid_amount] [float] NOT NULL,
	[payment_date] [datetime] NOT NULL,
	[payment_method] [int] NOT NULL,
	
	[status] [int] NOT NULL,
	
	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_service_payment] PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] --TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO