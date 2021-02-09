IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetRoleDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetRoleDetail]
GO

/****** Object:  StoredProcedure [sp_GetRoleDetail]    Script Date: 2013/11/1 下午 03:28:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/11/1 下午 03:28:07
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetRoleDetail]
	-- Access Object
    @access_object_id INT,

	-- Data
    @role_id INT,

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
    
    SET @accessObjectValid = 0
    SET @recValid = 0
       
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
    		FROM role
    		WHERE role_id = @role_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                    SELECT 
                        r.role_id,
                        r.name,
                        r.status,
                        r.crt_date,
                        r.crt_by_type,
                        r.crt_by,
                        r.upd_date,
                        r.upd_by_type,
                        r.upd_by,
                        r.record_status
                    FROM  role r
                    WHERE role_id = @role_id
                    AND r.[record_status] <> @recDeleted
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                    SELECT
                        r.role_id,
                        r.name,
                        r.status,
                        r.crt_date,
                        r.crt_by_type,
                        r.crt_by,
                        r.upd_date,
                        r.upd_by_type,
                        r.upd_by,
                        r.record_status
                    FROM  role r
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