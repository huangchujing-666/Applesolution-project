
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_MemberLoginWithFB]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_MemberLoginWithFB]
GO

/****** Object:  StoredProcedure [sp_MemberLoginWithFB]    Script Date: 2013/7/25 下午 05:12:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/25 下午 05:12:48
-- Description:	Stored Procedure for MemberLogin With FB
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_MemberLoginWithFB]
    @fbid VARCHAR(100),
    
	@sql_result INT OUTPUT,
	@sql_remark VARCHAR(100) OUTPUT
AS
    DECLARE @memberExist INT SET @memberExist = 0
    
	-- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')

    DECLARE @current_member_valid INT SET @current_member_valid = 0
    
    SET @sql_result = 0  
	--return 2: new Facebook member
	--return 1: valid member
	--return 0: inactive member/invalid data (sql exception)
						
	SET @sql_remark = ''
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Insert statements for procedure here
    SELECT @memberExist = 1
    FROM member_profile
    WHERE fbid = @fbid
    
	SELECT @current_member_valid = 1
    FROM member_profile
    WHERE fbid = @fbid AND [status] = @statusActive AND record_status <> @recDeleted

    IF (@memberExist = 1 AND @current_member_valid = 1)
		BEGIN
			SET @sql_result = 1
		END
	ELSE IF (@memberExist = 0 AND @current_member_valid = 0)
		BEGIN
			SET @sql_result = 2
		    SET @sql_remark = 'NewFBID'
		END
    END
GO
