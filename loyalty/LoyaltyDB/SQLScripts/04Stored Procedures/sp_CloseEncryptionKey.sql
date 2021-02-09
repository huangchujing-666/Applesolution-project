
/****** Object:  StoredProcedure [dbo].[sp_CloseEncryptionKey]    Script Date: 2013/7/2 ¤U¤È 06:03:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-01-29
-- Description:	Stored Procedure for Close Encryption Key
-- =============================================
CREATE PROCEDURE [dbo].[sp_CloseEncryptionKey]
	-- Add the parameters for the stored procedure here
AS
	DECLARE @EncryptionPassword NVARCHAR(MAX)
	DECLARE @CloseEncryptionKeySQL NVARCHAR(255)
	DECLARE @EncryptionKey NVARCHAR(255)
	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @EncryptionKey = [dbo].fn_GetSettingValueByName('EncryptionKey')
	
	SET @CloseEncryptionKeySQL = 'CLOSE SYMMETRIC KEY ' + @EncryptionKey + ';'

	EXEC sp_executesql @CloseEncryptionKeySQL
	
END

GO


