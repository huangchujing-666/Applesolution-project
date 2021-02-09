/****** Object:  Table [dbo].[log_role]    Script Date: 2013/8/9 下午 06:31:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[log_role](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[name] varchar(15) NOT NULL, 
	[table_name] varchar(30) NOT NULL,
	[table_pk_name] varchar(30) NOT NULL

 CONSTRAINT [PK_log_role] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO