IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateRolePrivilege]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateRolePrivilege]
GO

/****** Object:  StoredProcedure [sp_CreateRolePrivilege]    Script Date: 2013/12/9 上午 10:54:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/12/9 上午 10:54:02
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateRolePrivilege]
    -- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	-- Data
	@object_type int,
	@object_id int,
	@section_id int,
	@read_status int,
	@insert_status int,
	@update_status int,
	@delete_status int,
	@status int,
    
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
    BEGIN TRAN sp_CreateRolePrivilege
    
    -- Check Access Object
---    SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1

            IF @recValid = 1
    		    BEGIN
                
                    
                    INSERT INTO privilege
                    (                        
                        object_type, 
                        object_id, 
                        section_id, 
                        read_status, 
                        insert_status, 
                        update_status, 
                        delete_status, 
                        status, 
                        crt_date, 
						crt_by_type, 
						crt_by, 
                        upd_date, 
                        upd_by_type,
						upd_by
                    )
                    VALUES (
                        @object_type,  --object_type
                        @object_id,  --object_id
                        @section_id,  --section_id
                        @read_status,  --read_status
                        @insert_status,  --insert_status
                        @update_status,  --update_status
                        @delete_status,  --delete_status
                        @status,  --status
                        GETDATE(),  --crt_date
@access_object_type,
						@access_object_id,  --crt_by
                        GETDATE(),  --upd_date
@access_object_type,
                        @access_object_id --upd_by
                    )                                         
                    
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
    COMMIT TRAN sp_CreateRolePrivilege
END
GO
