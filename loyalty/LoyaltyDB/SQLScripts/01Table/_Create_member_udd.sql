IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[member_udd]') AND type in (N'U'))
DROP TABLE [dbo].[member_udd]
GO

/****** Object:  Table [dbo].[member_udd]    Script Date: 2013/7/17 上午 11:50:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[member_udd](
	[udd_id] [int] IDENTITY(1,1) NOT NULL,

	[member_id] [int] NOT NULL,

	[column1] [nvarchar](MAX) NULL,
	[column2] [nvarchar](MAX) NULL,
	[column3] [nvarchar](MAX) NULL,
	[column4] [nvarchar](MAX) NULL,
	[column5] [nvarchar](MAX) NULL,
	[column6] [nvarchar](MAX) NULL,
	[column7] [nvarchar](MAX) NULL,
	[column8] [nvarchar](MAX) NULL,
	[column9] [nvarchar](MAX) NULL,
	[column10] [nvarchar](MAX) NULL,
	[column11] [nvarchar](MAX) NULL,
	[column12] [nvarchar](MAX) NULL,
	[column13] [nvarchar](MAX) NULL,
	[column14] [nvarchar](MAX) NULL,
	[column15] [nvarchar](MAX) NULL,
	[column16] [nvarchar](MAX) NULL,
	[column17] [nvarchar](MAX) NULL,
	[column18] [nvarchar](MAX) NULL,
	[column19] [nvarchar](MAX) NULL,
	[column20] [nvarchar](MAX) NULL,

	[column21] [int] NULL,
	[column22] [int] NULL,
	[column23] [int] NULL,
	[column24] [int] NULL,
	[column25] [int] NULL,
	[column26] [int] NULL,
	[column27] [int] NULL,
	[column28] [int] NULL,
	[column29] [int] NULL,
	[column30] [int] NULL,
	[column31] [int] NULL,
	[column32] [int] NULL,
	[column33] [int] NULL,
	[column34] [int] NULL,
	[column35] [int] NULL,
	[column36] [int] NULL,
	[column37] [int] NULL,
	[column38] [int] NULL,
	[column39] [int] NULL,
	[column40] [int] NULL,

	[column41] [float] NULL,
	[column42] [float] NULL,
	[column43] [float] NULL,
	[column44] [float] NULL,
	[column45] [float] NULL,
	[column46] [float] NULL,
	[column47] [float] NULL,
	[column48] [float] NULL,
	[column49] [float] NULL,
	[column50] [float] NULL,
	[column51] [float] NULL,
	[column52] [float] NULL,
	[column53] [float] NULL,
	[column54] [float] NULL,
	[column55] [float] NULL,
	[column56] [float] NULL,
	[column57] [float] NULL,
	[column58] [float] NULL,
	[column59] [float] NULL,
	[column60] [float] NULL,

	[column61] [datetime] NULL,
	[column62] [datetime] NULL,
	[column63] [datetime] NULL,
	[column64] [datetime] NULL,
	[column65] [datetime] NULL,
	[column66] [datetime] NULL,
	[column67] [datetime] NULL,
	[column68] [datetime] NULL,
	[column69] [datetime] NULL,
	[column70] [datetime] NULL,
	[column71] [datetime] NULL,
	[column72] [datetime] NULL,
	[column73] [datetime] NULL,
	[column74] [datetime] NULL,
	[column75] [datetime] NULL,
	[column76] [datetime] NULL,
	[column77] [datetime] NULL,
	[column78] [datetime] NULL,
	[column79] [datetime] NULL,
	[column80] [datetime] NULL,
	
	--column 81-100: select 
	[column81] [int] NULL,
	[column82] [int] NULL,
	[column83] [int] NULL,
	[column84] [int] NULL,
	[column85] [int] NULL,
	[column86] [int] NULL,
	[column87] [int] NULL,
	[column88] [int] NULL,
	[column89] [int] NULL,
	[column90] [int] NULL,
	[column91] [int] NULL,
	[column92] [int] NULL,
	[column93] [int] NULL,
	[column94] [int] NULL,
	[column95] [int] NULL,
	[column96] [int] NULL,
	[column97] [int] NULL,
	[column98] [int] NULL,
	[column99] [int] NULL,
	[column100] [int] NULL,

	[crt_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,

	[upd_date] [datetime] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,

	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_member_udd] PRIMARY KEY CLUSTERED 
(
	[udd_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO