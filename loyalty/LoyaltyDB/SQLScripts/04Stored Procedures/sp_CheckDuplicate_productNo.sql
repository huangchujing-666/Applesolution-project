IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CheckDuplicate_productNo]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CheckDuplicate_productNo]
GO

/****** Object:  StoredProcedure [sp_CheckDuplicate_productNo]    Script Date: 2013/12/27 下午 06:23:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/12/27 下午 06:23:27
-- Description:	Stored Procedure for sp_CheckDuplicate_productNo
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CheckDuplicate_productNo]
    -- Access Object
    @access_object_id INT,
    
    -- Data
	@product_no VARCHAR(20),

    -- Output
	@duplicate INT OUTPUT,
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
	SET @duplicate = 0      
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
                       @duplicate = 1
                    FROM  product p
                    WHERE product_no = @product_no
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                    
        			SET @sql_remark = 'RecordInvalid'
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_remark = 'AccessObjectInvalid'
		END
        
END
GO