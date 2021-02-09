
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetMemberID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetMemberID]
GO

/****** Object:  StoredProcedure [sp_GetMemberID]    Script Date: 2013/7/30 上午 11:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/30 上午 11:14:20
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetMemberID]
    @member_token VARCHAR(100),

	@member_id INT OUTPUT,
	@sql_result INT OUTPUT,
	@sql_remark VARCHAR(100) OUTPUT
AS
    DECLARE @userValid INT SET @userValid = 0
    
	-- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')
    
    SET @sql_result = 0
	SET @sql_remark = ''
    SET @member_id = -1

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Insert statements for procedure here

    IF NOT(@member_token = NULL AND dbo.fn_TRIM(@member_token) = '')
		BEGIN
            SELECT @member_id = member_id
    		FROM member_profile
    		WHERE (member_no = @member_token OR mobile_no = @member_token OR email = @member_token)
		
            SET @sql_result = 1
		END
	ELSE
		BEGIN
		    SET @sql_remark = 'DataInvalid'
		END 
END
GO
