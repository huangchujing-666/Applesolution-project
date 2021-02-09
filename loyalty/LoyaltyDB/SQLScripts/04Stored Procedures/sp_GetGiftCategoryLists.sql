IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetGiftCategoryLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetGiftCategoryLists]
GO

/****** Object:  StoredProcedure [sp_GetGiftCategoryLists]    Script Date: 2013/10/17 下午 12:30:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/17 下午 12:30:57
-- Description:	Stored Procedure for GetGiftCategoryLists
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetGiftCategoryLists]
	-- Access Object
    @access_object_id INT,
    @lang_code INT, 
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
                        g.category_id, 
						gl.name,
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
                    FROM  gift_category g, gift_category_lang gl
                    WHERE g.category_id = gl.category_id
						AND gl.lang_id = @lang_code -- EN
						AND g.[record_status] <> @recDeleted
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                    SELECT 
                        g.category_id, 
						gl.name,
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
                    FROM  gift_category g, gift_category_lang gl
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