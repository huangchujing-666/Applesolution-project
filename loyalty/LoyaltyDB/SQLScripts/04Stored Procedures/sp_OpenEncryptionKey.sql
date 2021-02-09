
/****** Object:  StoredProcedure [dbo].[sp_OpenEncryptionKey]    Script Date: 2013/7/2 ¤U¤È 06:19:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-01-29
-- Description:	Stored Procedure for Open Encryption Key
-- =============================================
CREATE PROCEDURE [dbo].[sp_OpenEncryptionKey]
	-- Add the parameters for the stored procedure here
	@EncryptionKey NVARCHAR(255) OUTPUT
AS
	DECLARE @EncryptionCertificate NVARCHAR(MAX)
	DECLARE @OpenEncryptionKeySQL NVARCHAR(255)
	DECLARE @CloseEncryptionKeySQL NVARCHAR(255)

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @EncryptionKey = [dbo].fn_GetSettingValueByName('EncryptionKey')
	SET @EncryptionCertificate = [dbo].fn_GetSettingValueByName('EncryptionCertificate')
		
	SET @OpenEncryptionKeySQL = 'OPEN SYMMETRIC KEY ' + @EncryptionKey + ' DECRYPTION BY CERTIFICATE ' + @EncryptionCertificate;
	--SELECT @OpenEncryptionKeySQL

	EXEC sp_executesql @OpenEncryptionKeySQL
	
END

GO


