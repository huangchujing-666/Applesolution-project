IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetPasscodePrefixDetailBy]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetPasscodePrefixDetailBy]
GO

/****** Object:  StoredProcedure [sp_GetPasscodePrefixDetailBy]    Script Date: 2013/8/9 下午 07:18:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/9 下午 07:18:15
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetPasscodePrefixDetailBy]
	-- Access Object
    @access_object_id INT,
    
	-- Data
	@prefix_id INT,

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
    		FROM passcode_prefix
    		WHERE prefix_id = @prefix_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                    
                    SELECT 
                        p.prefix_id, 
                        p.product_id, 
						so.name 'product_name',
                        p.format_id, 
                        p.prefix_value,
						pf.passcode_format, 
                        p.current_generated, 
						pf.maximum_generate,
                        p.usage_precent, 
                        li.name 'status', 
						p.status 'status_id',  
                        p.crt_date, 
                        p.upd_date, 
                        p.crt_by, 
                        p.upd_by, 
                        p.record_status
                    FROM  passcode_prefix p, listing l, listing_item li, product pro, passcode_format pf, system_object so
                    WHERE prefix_id = @prefix_id
						AND p.[record_status] <> @recDeleted
                    	AND p.[status] = li.[value]
						AND l.[list_id] = li.[list_id]
						AND l.code = 'Status' 
						AND p.[record_status] <> @recDeleted

						AND p.product_id = pro.product_id
						AND p.format_id = pf.format_id

						AND so.object_id = pro.product_id

                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
					
					 SELECT 
                        p.prefix_id, 
                        p.product_id, 
						so.name 'product_name',
                        p.format_id, 
                        p.prefix_value,
						pf.passcode_format, 
                        p.current_generated, 
						pf.maximum_generate,
                        p.usage_precent, 
                        li.name 'status', 
						p.status 'status_id',  
                        p.crt_date, 
                        p.upd_date, 
                        p.crt_by, 
                        p.upd_by, 
                        p.record_status
                    FROM  passcode_prefix p, listing l, listing_item li, product pro, passcode_format pf, system_object so
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
    
    
