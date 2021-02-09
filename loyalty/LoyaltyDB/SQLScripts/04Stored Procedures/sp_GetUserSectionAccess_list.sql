IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetUserSectionAccess_list]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetUserSectionAccess_list]
GO

/****** Object:  StoredProcedure [sp_GetUserSectionAccess_list]    Script Date: 2013/12/12 下午 04:59:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/12/12 下午 04:59:51
-- Description:	Stored Procedure for GetUserSectionAccess_list
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUserSectionAccess_list]
    -- Access Object
    @access_object_id INT,
    
	@user_id INT,
	@module VARCHAR(40),

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
        
    -- Set default output value        
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
		    
			SET @module = LOWER(@module)

            SELECT @recValid = 1
			FROM system_object so
			WHERE object_id = @user_id AND [status] = @statusActive AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN

                    SELECT 
                        p.privilege_id,
                        p.object_type,
                        p.object_id,
                        p.section_id,
                        p.read_status,
                        p.insert_status,
                        p.update_status,
                        p.delete_status, 
                        p.status, 
                        p.crt_date, 
						p.crt_by_type, 
                        p.crt_by, 
						p.upd_date, 
                        p.upd_by_type, 
                        p.upd_by
                    FROM privilege p, user_role ur, section s
                    WHERE
						ur.user_id = @user_id
					AND ur.role_id = p.object_id
					AND s.module = @module
					AND s.section_id = p.section_id
                    
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                     SELECT
                        p.privilege_id,
                        p.object_type,
                        p.object_id,
                        p.section_id,
                        p.read_status,
                        p.insert_status,
                        p.update_status,
                        p.delete_status, 
                        p.status, 
                        p.crt_date, 
						p.crt_by_type, 
                        p.crt_by, 
						p.upd_date, 
                        p.upd_by_type, 
                        p.upd_by
                    FROM privilege p, user_role ur
                    WHERE 1=2
        			SET @sql_remark = 'RecordInvalid'
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_remark = 'AccessObjectInvalid'
		END
        
END
GO