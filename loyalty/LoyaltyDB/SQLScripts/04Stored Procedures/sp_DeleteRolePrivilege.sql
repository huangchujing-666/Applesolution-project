IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_DeleteRolePrivilege]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_DeleteRolePrivilege]
GO

/****** Object:  StoredProcedure [sp_DeleteRolePrivilege]    Script Date: 2013/12/9 上午 10:58:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/12/9 上午 10:58:58
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteRolePrivilege]
    -- Access Object
    @access_object_id INT,
    
    -- Data
	@object_type int,
	@object_id int,
	
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
    
    -- Begin Transaction
    BEGIN TRAN sp_DeleteRolePrivilege
    
    -- Check Access Object
    SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                
                    DELETE FROM privilege
                    WHERE
					object_type = @object_type
                    AND object_id = @object_id

                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
        			SET @sql_remark = 'RecordInvalid'
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_remark = 'AccessObjectInvalid'
		END
        
    -- Commit Transaction
    COMMIT TRAN sp_DeleteRolePrivilege
	END
GO