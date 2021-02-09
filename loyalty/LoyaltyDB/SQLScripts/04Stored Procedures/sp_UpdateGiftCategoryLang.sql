IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateGiftCategoryLang]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateGiftCategoryLang]
GO

/****** Object:  StoredProcedure [sp_UpdateGiftCategoryLang]    Script Date: 2013/10/17 下午 04:02:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/17 下午 04:02:34
-- Description:	Stored Procedure for UpdateGiftCategoryLang
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateGiftCategoryLang]
	-- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	-- Data
	@category_id int,
	@lang_id int,
	
	@name nvarchar(100),
	@description nvarchar(max),
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
        
    SET @sql_result = 0
	SET @sql_remark = ''
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_UpdateGiftCategoryLang
    
    -- Check Access Object
---	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM gift_category
    		WHERE category_id = @category_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                    UPDATE gift_category_lang
                    SET
                        name = @name, 
                        description = @description, 
                        status = @status, 
                        
						upd_date = GETDATE(), 
upd_by_type = @access_object_type, 
                        upd_by = @access_object_id
                        
                    WHERE category_id = @category_id
					AND lang_id = @lang_id
                    
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
        
    -- Commit Transaction
    COMMIT TRAN sp_UpdateGiftCategoryLang
END
GO
