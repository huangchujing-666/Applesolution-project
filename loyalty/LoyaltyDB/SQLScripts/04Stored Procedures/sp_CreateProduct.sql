IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateProduct]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateProduct]
GO

/****** Object:  StoredProcedure [sp_CreateProduct]    Script Date: 2013/8/6 下午 05:40:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/6 下午 05:40:06
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateProduct]
	-- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	-- Data
	--@product_id INT,
	@product_no varchar(20),
	
	@price float,
	@point float,
	@consumption_period int,
	@lost_customer_period int,
	
	@status int,

	-- Output
	@new_product_id INT OUTPUT,
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
    BEGIN TRAN sp_CreateProduct
    
    -- Check Access Object
---	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1
            
            IF @recValid = 1
    		    BEGIN
                    -- SET IDENTITY_INSERT product ON

                    INSERT INTO product
                    (
						--product_id,
                        product_no,
                        
                        price, 
                        point, 
                        consumption_period, 
                        lost_customer_period, 
                        status, 
                       
						crt_date,
						crt_by_type,
						crt_by,
						upd_date,
						upd_by_type,
						upd_by
                    )
                    VALUES (
                        
						--@product_id,
						@product_no,
                        
                        @price,  --price
                        @point,  --point
                        @consumption_period,  --consumption_period
                        @lost_customer_period,  --lost_customer_period
                        
                        @status,  --status
                        
						GETDATE(),
@access_object_type,
						@access_object_id,
						GETDATE(),
@access_object_type,
						@access_object_id
                    )                   
                        
					SET @new_product_id = SCOPE_IDENTITY()

                    SET @sql_result = 1

					-- SET IDENTITY_INSERT product OFF

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
    COMMIT TRAN sp_CreateProduct

END
GO
