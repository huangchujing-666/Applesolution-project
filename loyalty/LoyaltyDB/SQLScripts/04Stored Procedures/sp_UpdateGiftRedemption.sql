IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateGiftRedemption]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateGiftRedemption]
GO

/****** Object:  StoredProcedure [sp_UpdateGiftRedemption]    Script Date: 2014/1/16 下午 02:33:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/1/16 下午 02:33:51
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateGiftRedemption]
    -- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
    -- Data
	@redemption_id int,
	@transaction_id int,
	@redemption_code varchar(20),
	@redemption_channel int,
	@member_id int,
	@gift_id int,
	@quantity int,
	@point_used float,
	@redemption_status int,
	@collect_date datetime,
	@collect_location_id int,
	@void_date datetime,
	@void_user_id int,
	@remark nvarchar(max),
	@status int,
	
    -- Output
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
    BEGIN TRAN sp_UpdateGiftRedemption
    
    -- Check Access Object
---    SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM gift_redemption
    		WHERE redemption_id = @redemption_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                
                    UPDATE gift_redemption
                    SET
                        transaction_id = @transaction_id, 
						redemption_code = @redemption_code,
                        redemption_channel = @redemption_channel, 
                        member_id = @member_id, 
                        gift_id = @gift_id, 
                        quantity = @quantity, 
                        point_used = @point_used, 
                        redemption_status = @redemption_status, 
                        collect_date = @collect_date, 
                        collect_location_id = @collect_location_id, 
                        void_date = @void_date, 
                        void_user_id = @void_user_id, 
                        remark = @remark, 
                        status = @status,  
                       
                        upd_date = GETDATE(), 
upd_by_type = @access_object_type, 
                        upd_by = @access_object_id

                    WHERE redemption_id = @redemption_id
                    
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
    COMMIT TRAN sp_UpdateGiftRedemption
END
GO
