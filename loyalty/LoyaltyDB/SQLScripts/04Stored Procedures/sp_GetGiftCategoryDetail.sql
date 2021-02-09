IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetGiftCategoryDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetGiftCategoryDetail]
GO

/****** Object:  StoredProcedure [sp_GetGiftCategoryDetail]    Script Date: 2013/10/17 下午 02:53:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/17 下午 02:53:21
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetGiftCategoryDetail]
	-- Access Object
    @access_object_id INT,

	-- Data
    @category_id INT,

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
    		FROM gift_category
    		WHERE category_id = @category_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                      
                    SELECT 
                        g.category_id,
						g.parent_id, 
						g.leaf,
                        g.photo_file_name, 
                        g.photo_file_extension, 
                        g.display_order, 
                        g.status, 
                        g.crt_date, 
                        g.crt_by_type, 
                        g.crt_by, 
                        g.upd_date, 
                        g.upd_by_type, 
                        g.upd_by, 
                        g.record_status
                    FROM  gift_category g
                    WHERE category_id = @category_id
                    AND g.[record_status] <> @recDeleted
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                    SELECT 
                        g.category_id, 
						g.parent_id,
						g.leaf,
                        g.photo_file_name, 
                        g.photo_file_extension, 
                        g.display_order, 
                        g.status, 
                        g.crt_date, 
                        g.crt_by_type, 
                        g.crt_by, 
                        g.upd_date, 
                        g.upd_by_type, 
                        g.upd_by, 
                        g.record_status
                    FROM  gift_category g
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