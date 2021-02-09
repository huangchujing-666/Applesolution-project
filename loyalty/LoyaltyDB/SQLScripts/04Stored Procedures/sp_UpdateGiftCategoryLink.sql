IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateGiftCategoryLink]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateGiftCategoryLink]
GO

/****** Object:  StoredProcedure [sp_UpdateGiftCategoryLink]    Script Date: 2014/2/25 下午 12:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/2/25 下午 12:11:09
-- Description:	Stored Procedure for Update Gift Category Link
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateGiftCategoryLink]
    -- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
    -- Data
	@gift_id int,
	@category_id int,
	@display_order int,
	
    -- Output
    @sql_result INT OUTPUT
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
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_UpdateGiftCategoryLink
    
    -- Check Access Object
---    SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM gift_category_link
    		WHERE gift_id = @gift_id AND category_id = @category_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                
                    UPDATE gift_category_link
                    SET
                        display_order = @display_order, 
                       
                        upd_date = GETDATE(), 
upd_by_type = @access_object_type, 
                        upd_by = @access_object_id
                       
                    WHERE gift_id = @gift_id AND category_id = @category_id 
                    
                    SET @sql_result = 100 --normal
                END
	    	ELSE
    		    BEGIN
        			SET @sql_result = 1112  --Record Invalid
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_result = 1111 --no_permission
		END
        
    -- Commit Transaction
    COMMIT TRAN sp_UpdateGiftCategoryLink
END
GO
