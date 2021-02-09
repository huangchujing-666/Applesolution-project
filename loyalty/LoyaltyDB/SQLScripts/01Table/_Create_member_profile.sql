IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[member_profile]') AND type in (N'U'))
DROP TABLE [dbo].[member_profile]
GO

/****** Object:  Table [dbo].[member_profile]    Script Date: 2013/7/17 上午 11:50:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[member_profile](
	[member_id] [int] IDENTITY(1,1) NOT NULL,

	[member_no] [varchar](20) NOT NULL,
	[password] [NVARCHAR](MAX) NULL,
	[email] [varchar](100) NULL,
	[fbid] [VARCHAR](100) NULL,
	[fbemail] [VARCHAR](100) NULL,
	[mobile_no] [varchar](12) NOT NULL,
	[salutation] [int] NOT NULL,
	
	[firstname] [nvarchar](100) NOT NULL,
	[middlename] [nvarchar](100) NOT NULL,
	[lastname] [nvarchar](100) NOT NULL,
	[fullname] [nvarchar](300) NOT NULL,
	
	[birth_year] [int] NOT NULL,
	[birth_month] [int] NOT NULL,
	[birth_day] [int] NOT NULL,
	[gender] [int] NOT NULL,
	[hkid] [varchar](20) NOT NULL,
	[address1] [nvarchar](100) NULL, --unit
	[address2] [nvarchar](100) NULL, --building
	[address3] [nvarchar](100) NULL, --address
	[district] [int] NOT NULL DEFAULT 0,
	[region] [int] NOT NULL DEFAULT 0,
	[reg_source] [int] NOT NULL,
	[referrer] [int] NULL,
	[reg_status] [int] NOT NULL,
	[reg_ip] [varchar](50) NOT NULL,
	[activate_key] [varchar](50) NOT NULL,
	[hash_key] [varchar](50) NOT NULL,
	[session] [NVARCHAR](MAX) NULL,
	[status] [int] NOT NULL,
	[opt_in] [int] NOT NULL,
	[member_level_id] [int] NOT NULL,
	[member_category_id] [int] NOT NULL,
	[available_point] [float] NOT NULL,
	[crt_date] [datetime] NOT NULL,
	[upd_date] [datetime] NOT NULL,
	[crt_by_type] [int] NOT NULL,
	[crt_by] [int] NOT NULL,
	[upd_by_type] [int] NOT NULL,
	[upd_by] [int] NOT NULL,
	[record_status] [int] NOT NULL DEFAULT 0

 CONSTRAINT [PK_member_profile] PRIMARY KEY CLUSTERED 
(
	[member_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
