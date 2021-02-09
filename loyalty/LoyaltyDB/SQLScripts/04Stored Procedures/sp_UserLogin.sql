IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UserLogin]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UserLogin]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserLogin]    Script Date: 2013/7/2 ¤U¤È 06:23:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Leo
-- Create date: 2013-06-0. 15:35
-- Description:	Stored Procedure for User Login Checking
-- Pending Task:
-- =============================================
CREATE PROCEDURE [dbo].[sp_UserLogin]
	-- Add the parameters for the stored procedure here
	@login_id VARCHAR(20),
	@password NVARCHAR(255),
	@action_ip NVARChAR(50),

	@status INT OUTPUT,
	@remark VARCHAR(100) OUTPUT,
	@user_id INT OUTPUT,
	@name NVARCHAR(100) OUTPUT
AS
	DECLARE @userExist INT SET @userExist = 0
	DECLARE @userActive INT SET @userActive = 0
	DECLARE @passwordMatch INT SET @passwordMatch = 0
	
	-- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')

	DECLARE @section_id INT
	DECLARE @description NVARCHAR(MAX)

	SET @status = 0
	SET @remark = ''
	SET @user_id = 0
	SET @name = ''

	SET @section_id = 54	
	SET @description = N'Web Server Login'
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Check Username exist, user active and password match or not
    SELECT 
		@userExist = 1, 
		@userActive = u.status,
		@passwordMatch = 
			CASE WHEN u.password = @password
			THEN 1 ELSE 0 END, 
		@user_id = u.user_id,
		@name = u.name
    FROM user_profile u
    WHERE (u.login_id = @login_id or u.email = @login_id)
	
	AND u.status = @statusActive
	AND u.record_status <> @recDeleted

	SELECT @status = 1
	WHERE @userExist = 1 AND @userActive = 1 AND @passwordMatch = 1	

	IF @status=1
	BEGIN
		UPDATE [user_profile]
		SET [action_date] = GETDATE(), [action_ip] = @action_ip
		WHERE [user_id] = @user_id
	END

    -- Set Login Fail Reason
    IF @userExist = 0 
		SET @remark = 'UserInvalid'
	ELSE IF @userActive = 0
		SET @remark = 'UserLocked'
    ELSE IF @passwordMatch = 0
		SET @remark = 'PasswordInvalid'

END

GO