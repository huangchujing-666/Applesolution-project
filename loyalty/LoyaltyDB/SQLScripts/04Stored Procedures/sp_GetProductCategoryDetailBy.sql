IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetProductCategoryDetailBy]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetProductCategoryDetailBy]
GO

/****** Object:  StoredProcedure [sp_GetProductCategoryDetailBy]    Script Date: 2013/8/7 下午 06:03:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/7 下午 06:03:14
-- Description:	Stored Procedure for Get Product Category Detail By User ID
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetProductCategoryDetailBy]
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
    		FROM product_category
    		WHERE category_id = @category_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                    
                    SELECT 
                        p.category_id, 
                        p.parent_id, 
						p.leaf,
                        p.photo_file_name, 
                        p.photo_file_extension, 
                        p.display_order, 
                      
						li.name 'status',
                        p.status 'status_id'
                     
                    FROM  product_category p, listing l, listing_item li
                    WHERE category_id = @category_id
						AND p.[record_status] <> @recDeleted

						AND p.[status] = li.[value]
						AND l.[list_id] = li.[list_id]
						AND l.code = 'Status' 
						AND p.[record_status] <> @recDeleted
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
					SELECT 
                        p.category_id, 
                        p.parent_id, 
						p.leaf,
                        p.photo_file_name, 
                        p.photo_file_extension, 
                        p.display_order, 
                      
						li.name 'status',
                        p.status 'status_id'
                     
                    FROM  product_category p, listing l, listing_item li
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
    
    
