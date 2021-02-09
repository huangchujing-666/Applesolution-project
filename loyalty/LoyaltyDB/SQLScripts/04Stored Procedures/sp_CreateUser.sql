IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateUser]
GO

/****** Object:  StoredProcedure [dbo].[sp_CreateUser]    Script Date: 2013/7/2 �U�� 06:10:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Leo
-- Create date: 2013-06-01
-- Description:	Stored Procedure for Create User
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateUser]
	-- Access Object
 @access_object_type INT, 
	@access_object_id INT,
	
	-- Data
	--@user_id INT,
	@login_id VARCHAR(20),
	@name NVARCHAR(80),
	@email VARCHAR(80),
	@password NVARCHAR(MAX),
	@status INT,

	-- Output
	@new_user_id INT OUTPUT,
    @sql_result INT OUTPUT
AS
    -- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')

	SET @sql_result = 0 
	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
				
	-- Begin Transaction
	BEGIN TRAN sp_CreateUser

	-- Check Access Object
---	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted

SET @accessObjectValid = 1 
	IF @accessObjectValid = 1
	BEGIN

		SET @recValid = 1
    		
        IF @recValid = 1
    		BEGIN
				--SET IDENTITY_INSERT [dbo].[user_profile] ON

				INSERT INTO user_profile (
					--[user_id],
					[login_id], 
					[name],
					[password], 
					[email], 
						
					[action_ip], 
					[action_date], 
						
					[status],
					[crt_date], 
					[crt_by_type], 
					[crt_by], 
					[upd_date], 
					[upd_by_type],
					[upd_by])
				VALUES (
					--@user_id,
					@login_id,
					@name,
					@password,
					@email,
						
					NULL, -- action_ip
					NULL, -- action_date
						
					@status,
					GETDATE(), -- crt_date
@access_object_type,
					@access_object_id,
					GETDATE(), -- upd_date
@access_object_type,
					@access_object_id)
				
				SET @new_user_id = SCOPE_IDENTITY()
				
				SET @sql_result = 100 --normal

				--SET IDENTITY_INSERT [dbo].[user_profile] OFF
		 END
	    	ELSE
    		    BEGIN
        			SET @sql_result = 1112  --Record Invalid
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_result = 1111 --no_permission
		END

	-- Commit Transaction
	COMMIT TRAN sp_CreateUser
END
GO
