IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateProductPurchase]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateProductPurchase]
GO

/****** Object:  StoredProcedure [sp_CreateProductPurchase]    Script Date: 2014/3/10 上午 10:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/3/10 上午 10:50:27
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateProductPurchase]
    -- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
    -- Data
	@order_id int,
	@transaction_id int,
	@point float,
	@promotion_transaction_id int,
	@member_id int,
	@product_id int,
	@quantity int,
	@total_amount float,
	@purchase_date datetime,

	@status int,
	
    -- Output
	@new_purchase_id INT OUTPUT,
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
    BEGIN TRAN sp_CreateProductPurchase
    
    -- Check Access Object
---    SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO product_purchase
                    (
						order_id,
                        transaction_id, 
						point,
						promotion_transaction_id,
                        member_id, 
                        product_id, 
                        quantity, 
                        total_amount,
						purchase_date,

                        status, 
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
						@order_id,
						@transaction_id,  --transaction_id
						@point,
						@promotion_transaction_id,
                        @member_id,  --member_id
                        @product_id,  --product_id
                        @quantity,  --quantity
						@total_amount,
                        @purchase_date,

                        @status,  --status
                        GETDATE(),  --crt_date
@access_object_type,
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
@access_object_type,
                        @access_object_id  --upd_by
                    )                   
                    
					SET @new_purchase_id = SCOPE_IDENTITY()

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
    COMMIT TRAN sp_CreateProductPurchase
END
GO
