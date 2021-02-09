IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetUserProfileDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetUserProfileDetail]
GO

/****** Object:  StoredProcedure [sp_GetUserProfileDetail]    Script Date: 2013/10/31 下午 02:33:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/31 下午 02:33:55
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUserProfileDetail]
	@search_user_id INT,
	@search_login_id VARCHAR(20),

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
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Insert statements for procedure here
    
	SET @userValid = 1
    
    IF @userValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM user_profile
    		WHERE (user_id = @search_user_id OR login_id = @search_login_id) AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                      
                    SELECT
                        u.user_id, 
                        u.login_id, 
                        u.password, 
                        so.name, 
                        u.email, 
                        so.status, 
                        u.action_ip, 
                        u.action_date, 
                        u.crt_date, 
                        u.upd_date, 
                        u.crt_by, 
                        u.upd_by, 
                        u.record_status
                    FROM  user_profile u, system_object so
                    WHERE (user_id = @search_user_id OR login_id = @search_login_id)
					AND u.user_id = so.object_id
                    AND u.[record_status] <> @recDeleted
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                    SELECT
                        u.user_id, 
                        u.login_id, 
                        u.password, 
                        so.name, 
                        u.email, 
                        so.status, 
                        u.action_ip, 
                        u.action_date, 
                        u.crt_date, 
                        u.upd_date, 
                        u.crt_by, 
                        u.upd_by, 
                        u.record_status
                    FROM  user_profile u, system_object so
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