IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateProduct]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateProduct]
GO

/****** Object:  StoredProcedure [sp_UpdateProduct]    Script Date: 2013/8/5 上午 11:18:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/5 上午 11:18:16
-- Description:	Stored Procedure for Update Product
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateProduct]
	-- Access Object
    @access_object_id INT,
    
	-- Data
	@product_id INT,
	@product_no VARCHAR(20),
	
	@price float,
	@point float,
	@consumption_period int,
	@lost_customer_period int,
	
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
    BEGIN TRAN sp_UpdateProduct
    
    -- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM product
    		WHERE product_id = @product_id
		
            IF @recValid = 1
    		    BEGIN
                            UPDATE product
                            SET
                                product_no = @product_no,
                                
                                price = @price,
                                point = @point,
                                consumption_period = @consumption_period,
                                lost_customer_period = @lost_customer_period,
                                
                                status = @status,
                            
                                upd_date = GETDATE(),
                                upd_by = @access_object_id
                                
                            WHERE product_id = @product_id
                    
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
     
    COMMIT TRAN sp_UpdateProduct
END
GO
