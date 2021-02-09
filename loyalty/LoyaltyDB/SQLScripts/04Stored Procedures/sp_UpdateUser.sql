IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateUser]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		LEO
-- Create date: 2013-06-03 15:00
-- Description:	Stored Procedure for Update User
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateUser]
	-- Access Object
 @access_object_type INT, 
	@access_object_id INT,
    
	-- Data
	@user_id int,
	@login_id varchar(20),
	@name nvarchar(80),
	@password nvarchar(max),
	@email varchar(80),
	@action_ip nvarchar(50),
	@action_date datetime,
	@status int,

	-- Output
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
	BEGIN TRAN sp_UpdateUser

	-- Check Access Object
---	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted

SET @accessObjectValid = 1 
	IF @accessObjectValid = 1
		BEGIN

			SELECT @recValid = 1
			FROM user_profile
			WHERE user_id = @user_id AND record_status <> @recDeleted
            
			IF @recValid = 1
				BEGIN

					UPDATE user_profile
					SET
						login_id = @login_id,
						name = @name,
						password = @password,
						email = @email, 
						action_ip = @action_ip, 
						action_date = @action_date, 
                
						status = @status,
						upd_date = GETDATE(), 
upd_by_type = @access_object_type, 
						upd_by = @access_object_id
					WHERE user_id = @user_id

					SET @sql_result = 100 --normal
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
	COMMIT TRAN sp_UpdateUser
END
GO
