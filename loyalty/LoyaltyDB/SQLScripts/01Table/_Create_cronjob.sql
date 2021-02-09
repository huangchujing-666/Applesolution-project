/****** Object:  Table [dbo].[cronjob]    Script Date: 2016/8/16 ÉÏÎç 12::00:00 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id=OBJECT_ID(N'[dbo].[cronjob]') AND type in(N'U'))
DROP TABLE [dbo].[cronjob]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
CREATE TABLE cronjob
(
    cronjob_id int identity(1,1) NOT NULL,
    name nvarchar(100) NOT NULL,
    execute_date datetime NULL,
    complete_date datetime NULL,
    execute_result int NOT NULL,
    execute_message nvarchar(500) NOT NULL,
    no_of_processd int NOT NULL,
    no_of_success int NOT NULL,
    no_of_fail int NOT NULL,
    [status] int NOT NULL,
    crt_date datetime NOT NULL,
	crt_by_type int NOT NULL,
	crt_by int NOT NULL,
	upd_date datetime NOT NULL,
	upd_by_type int NOT NULL,
	upd_by int NOT NULL,
	record_status int NOT NULL,
	CONSTRAINT [PK_cronjob] PRIMARY KEY CLUSTERED
	(
	   cronjob_id asc
	)
)ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO