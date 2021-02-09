IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateGift]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateGift]
GO

/****** Object:  StoredProcedure [sp_UpdateGift]    Script Date: 2013/10/15 下午 07:26:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/15 下午 07:26:46
-- Description:	Stored Procedure for sp_UpdateGift
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateGift]
	-- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	-- Data
	@gift_id int,
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
    BEGIN TRAN sp_UpdateGift
    
    -- Check Access Object
---	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM gift
    		WHERE gift_id = @gift_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                    UPDATE gift
                    SET
                        gift_no = @gift_no, 
                        
                        point = @point, 
                        alert_level = @alert_level, 
                        cost = @cost, 

                        discount = @discount, 
                        discount_point = @discount_point, 
                        discount_active_date = @discount_active_date, 
                        discount_expiry_date = @discount_expiry_date, 

                        hot_item = @hot_item, 
                        hot_item_active_date = @hot_item_active_date, 
                        hot_item_expiry_date = @hot_item_expiry_date, 
						hot_item_display_order = @hot_item_display_order,

                        display_public = @display_public, 
                        display_active_date = @display_active_date, 
                        display_expiry_date = @display_expiry_date, 
                        redeem_active_date = @redeem_active_date, 
                        redeem_expiry_date = @redeem_expiry_date, 

                        status = @status, 
                   
						available_stock = @available_stock,

                        upd_date = GETDATE(), 
upd_by_type = @access_object_type, 
                        upd_by = @access_object_id
                   
                    WHERE gift_id = @gift_id
                    
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
    COMMIT TRAN sp_UpdateGift
END
GO
