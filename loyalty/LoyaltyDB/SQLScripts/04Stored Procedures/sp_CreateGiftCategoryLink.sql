IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateGiftCategoryLink]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateGiftCategoryLink]
GO

/****** Object:  StoredProcedure [sp_CreateGiftCategoryLink]    Script Date: 2014/2/12 上午 11:38:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/2/12 上午 11:38:23
-- Description:	Stored Procedure for CreateGiftCategoryLink
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateGiftCategoryLink]
    -- Access Object
    @access_object_id INT,
    @access_object_type INT, 
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
    BEGIN TRAN sp_CreateGiftCategoryLink
    
    -- Check Access Object
    --SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    --FROM v_accessObject ao
    --WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
	SET @accessObjectValid = 1
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM gift
    		WHERE gift_id = @gift_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO gift_category_link
                    (
                        
                        gift_id, 
                        category_id, 
						display_order,
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
                        
                        @gift_id,  --gift_id
                        @category_id,  --category_id
						@display_order, --display_order
                        GETDATE(),  --crt_date
                        @access_object_type,  --crt_by_type
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
                        @access_object_type,  --upd_by_type
                        @access_object_id  --upd_by
                    )                   
                      
                    
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
    COMMIT TRAN sp_CreateGiftCategoryLink
END
GO