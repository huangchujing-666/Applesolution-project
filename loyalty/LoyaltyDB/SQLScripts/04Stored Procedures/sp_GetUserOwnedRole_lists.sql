IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetUserOwnedRole_lists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetUserOwnedRole_lists]
GO

/****** Object:  StoredProcedure [sp_GetUserOwnedRole_lists]    Script Date: 2013/11/3 下午 03:23:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/11/3 下午 03:23:38
-- Description:	Stored Procedure for GetUserOwnedRole_lists
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUserOwnedRole_lists]
	-- Access Object
	@access_object_id INT,
    
	-- Data
	@user_id INT,
    
	-- Output
    @sql_result INT OUTPUT,
    @sql_remark VARCHAR(100) OUTPUT
AS
	-- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')
        
    SET @sql_result = 0
	SET @sql_remark = ''
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN

            SELECT @recValid = 1
    		FROM user_profile
    		WHERE user_id = @user_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                   
					SELECT 
						up.user_id,
						ur.[role_id],
						ur.[status],
						
						r.name 'role_name'
					FROM  user_role ur, user_profile up, role r
					WHERE ur.user_id = up.user_id
					AND ur.role_id = r.role_id
					AND up.user_id = @user_id
					AND up.[record_status] <> @recDeleted 

                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                   SELECT 
						ur.[user_id],
						ur.[role_id],
						ur.[status],
						up.user_id,
						r.name 'role_name'
					FROM  user_role ur, user_profile up, role r
					WHERE 1=2
        			SET @sql_remark = 'RecordInvalid'
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_remark = 'UserInvalid'
		END
        
END
GO