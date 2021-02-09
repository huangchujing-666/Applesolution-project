IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetProductID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetProductID]
GO

/****** Object:  StoredProcedure [sp_GetProductID]    Script Date: 2013/8/26 下午 06:49:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/26 下午 06:49:34
-- Description:	Stored Procedure for GetProductID by product no
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetProductID]
	-- Access Object
    @access_object_id INT,
    
	-- Data
	@product_no VARCHAR(20),

	-- Output
	@product_id INT OUTPUT,
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

            SELECT @recValid = 1, @product_id = product_id
    		FROM product
    		WHERE product_no = @product_no AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN                    
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