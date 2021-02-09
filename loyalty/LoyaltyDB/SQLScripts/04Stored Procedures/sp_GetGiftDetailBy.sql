IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetGiftDetailBy]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetGiftDetailBy]
GO

/****** Object:  StoredProcedure [sp_GetGiftDetailBy]    Script Date: 2013/10/8 下午 05:51:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/8 下午 05:51:22
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetGiftDetailBy]
	-- Access Object
    @access_object_id INT,
    
	-- Data
	@gift_id INT,

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
    
    -- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM gift
    		WHERE gift_id = @gift_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                      
                    SELECT 
                        g.gift_id, 
                        g.gift_no, 
                        
                        g.point, 
                        g.alert_level, 
                        g.cost, 

                        g.discount, 
                        g.discount_point, 
                        g.discount_active_date, 
                        g.discount_expiry_date, 

                        g.hot_item, 
                        g.hot_item_active_date, 
						g.hot_item_display_order,
                        g.hot_item_expiry_date, 

                        g.display_public,
                        g.display_active_date, 
                        g.display_expiry_date, 
                        g.redeem_active_date, 
                        g.redeem_expiry_date, 
						 
                        g.status, 

                        g.crt_date, 
                        g.crt_by_type, 
                        g.crt_by, 
                        g.upd_date, 
                        g.upd_by_type, 
                        g.upd_by, 
                        g.record_status,

						gl.name 'name'
                    FROM  gift g, gift_lang gl
                    WHERE g.gift_id = @gift_id
					AND gl.gift_id = g.gift_id
					AND gl.lang_id = 2
                    AND g.[record_status] <> @recDeleted
					
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