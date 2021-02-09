IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateGift]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateGift]
GO

/****** Object:  StoredProcedure [sp_CreateGift]    Script Date: 2013/10/10 上午 10:54:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/10 上午 10:54:53
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateGift]
	-- Access Object
    @access_object_id INT,
	@access_object_type INT, 
    -- Data
	-- @gift_id INT,
	@gift_no varchar(20),
	
	@point float,
	@alert_level int,
	@cost float,

	@discount bit,
	@discount_point float,
	@discount_active_date datetime,
	@discount_expiry_date datetime,

	@hot_item bit,
	@hot_item_active_date datetime,
	@hot_item_expiry_date datetime,
	@hot_item_display_order int,

	@display_public bit,
	@display_active_date datetime,
	@display_expiry_date datetime,
	@redeem_active_date datetime,
	@redeem_expiry_date datetime,

	@status int,
	@available_stock int,

	-- OUTPUT
	@new_gift_id INT OUTPUT,
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
    
    -- Begin Transaction
    BEGIN TRAN sp_CreateGift
    
    -- Check Access Object
    --SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    --FROM v_accessObject ao
    --WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
	SET @accessObjectValid = 1
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1

            IF @recValid = 1
    		    BEGIN
                    
					--SET IDENTITY_INSERT gift ON

                    INSERT INTO gift
                    (
						--gift_id,
                        gift_no, 
                        
                        point, 
                        alert_level, 
                        cost, 

                        discount,
                        discount_point,
                        discount_active_date, 
                        discount_expiry_date, 

                        hot_item, 
                        hot_item_active_date, 
                        hot_item_expiry_date, 
						hot_item_display_order, 

                        display_public, 
                        display_active_date, 
                        display_expiry_date, 
                        redeem_active_date, 
                        redeem_expiry_date, 
						                        
                        status,

						available_stock,

						crt_date,
						crt_by_type,
						crt_by,
						upd_date,
						upd_by_type,
						upd_by
                    )
                    VALUES (
						--@gift_id,
                        @gift_no,  --gift_no
                        
                        @point,  --point
                        @alert_level,  --alert_level
                        @cost,  --cost

                        @discount,  --discount
                        @discount_point,  --discount_point
                        @discount_active_date,  --discount_active_date
                        @discount_expiry_date,  --discount_expiry_date

                        @hot_item,  --hot_item
                        @hot_item_active_date,  --hot_item_active_date
                        @hot_item_expiry_date,  --hot_item_expiry_date
						@hot_item_display_order,  --display_order

						@display_public,  --display_public
                        @display_active_date,  --active_date
                        @display_expiry_date,  --expiry_date
                        @redeem_active_date, 
                        @redeem_expiry_date, 
						                        
                        @status,  --status

						@available_stock,

						GETDATE(),
						@access_object_type,
						@access_object_id,
						GETDATE(),
						@access_object_type,
						@access_object_id
                    )

					SET @new_gift_id = SCOPE_IDENTITY()
					
                    SET @sql_result = 1

					--SET IDENTITY_INSERT gift OFF
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
    COMMIT TRAN sp_CreateGift
END
GO