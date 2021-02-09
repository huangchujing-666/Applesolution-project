
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetProductCategoryLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetProductCategoryLists]
GO

/****** Object:  StoredProcedure [sp_GetProductCategoryLists]    Script Date: 2013/7/29 下午 07:19:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/29 下午 07:19:45
-- Description:	Stored Procedure for Get Product Category Lists
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetProductCategoryLists]
	-- Access Object
    @access_object_id INT,
    
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
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                    
					SELECT
						p.category_id,
                        
						pl.name,
					    p.parent_id,
						p.leaf,
					    p.photo_file_name, 
                        p.photo_file_extension, 

                        p.display_order,
                        
						
						li.name 'status',
                        p.status 'status_id',
                        p.crt_date,
                        p.upd_date,
                        p.crt_by,
                        p.upd_by
					FROM product_category p, listing l, listing_item li, product_category_lang pl
                    WHERE
						p.[record_status] <> @recDeleted
						AND p.[status] = li.[value]
						AND l.[list_id] = li.[list_id]
						AND l.code = 'Status'
						AND p.[record_status] <> @recDeleted
						AND p.category_id = pl.category_id
						AND pl.lang_id = 2 --EN

                    SET @sql_result = 1
    		    END
	    	ELSE
    		    BEGIN
        			SET @sql_remark = 'RecordInvalid'
    		    END
		END
	ELSE
		BEGIN
		    SET @sql_remark = 'UserInvalid'
		END
END
GO