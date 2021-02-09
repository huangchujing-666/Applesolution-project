IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreatePromotionRule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreatePromotionRule]
GO

/****** Object:  StoredProcedure [sp_CreatePromotionRule]    Script Date: 2014/2/28 下午 02:48:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/2/28 下午 02:48:21
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreatePromotionRule]
    -- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
    -- Data
	@name nvarchar(100),
	@start_date datetime,
	@end_date datetime,
	@type int,
	@conjunction int,
	@transaction_criteria int,
	@special_criteria_type int,
	@special_criteria_value int,
	@earn_point_type int,
	@earn_point_value float,
	@earn_gift_id int,
	@earn_gift_quantity float,
	@redeem_discount float,
	@status int,
	
    -- Output
	@new_rule_id INT OUTPUT,
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
    BEGIN TRAN sp_CreatePromotionRule
    
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
                    INSERT INTO promotion_rule
                    (
                        name, 
                        start_date, 
                        end_date, 
                        type, 
						conjunction,
                        transaction_criteria, 
                        special_criteria_type, 
                        special_criteria_value, 
                        earn_point_type, 
                        earn_point_value, 
                        earn_gift_id, 
                        earn_gift_quantity, 
						redeem_discount,
                        status,
						crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
                        @name,  --name
                        @start_date,  --start_date
                        @end_date,  --end_date
                        @type,  --type
						@conjunction,
                        @transaction_criteria,  --transaction_criteria
                        @special_criteria_type,  --special_criteria_type
                        @special_criteria_value,  --special_criteria_value
                        @earn_point_type,  --earn_point_type
                        @earn_point_value,  --earn_point_value
                        @earn_gift_id,  --earn_gift_id
                        @earn_gift_quantity,  --earn_gift_quantity
						@redeem_discount, --redeem_discount
                        @status,  --status
						GETDATE(),  --crt_date
@access_object_type,
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
@access_object_type,
                        @access_object_id  --upd_by
                    )                   
                      
					SET @new_rule_id = SCOPE_IDENTITY()

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
    COMMIT TRAN sp_CreatePromotionRule
END
GO
