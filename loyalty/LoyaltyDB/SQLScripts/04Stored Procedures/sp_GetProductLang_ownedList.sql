IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetProductLang_ownedList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetProductLang_ownedList]
GO

/****** Object:  StoredProcedure [sp_GetProductLang_ownedList]    Script Date: 2014/1/17 下午 02:32:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/1/17 下午 02:32:35
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetProductLang_ownedList]
    -- Access Object
    @access_object_id INT,
    
	-- Data
	@product_id INT,
    
    -- Paging
	--@rowIndexStart bigint,
	--@rowLimit int,
	-- Searching
	--@searchParams NVARCHAR(MAX),

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
    
    -- Check Access Object
    SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM product
    		WHERE product_id = @product_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                      
                    SELECT 
                        p.product_lang_id, 
                        p.product_id, 
                        p.lang_id, 
                        p.name, 
                        p.description, 
                        p.status, 
                        p.crt_date, 
                        p.crt_by_type, 
                        p.crt_by, 
                        p.upd_date, 
                        p.upd_by_type, 
                        p.upd_by, 
                        p.record_status
                    FROM  product_lang p
                    WHERE product_id = @product_id
                    AND p.[record_status] <> @recDeleted
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                    SELECT 
                        p.product_lang_id, 
                        p.product_id, 
                        p.lang_id, 
                        p.name, 
                        p.description, 
                        p.status, 
                        p.crt_date, 
                        p.crt_by_type, 
                        p.crt_by, 
                        p.upd_date, 
                        p.upd_by_type, 
                        p.upd_by, 
                        p.record_status
                    FROM  product_lang p
                    WHERE 1=2
        			SET @sql_remark = 'RecordInvalid'
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_remark = 'AccessObjectInvalid'
		END
        
END
GO