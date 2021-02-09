IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateGiftRedemption]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateGiftRedemption]
GO

/****** Object:  StoredProcedure [sp_CreateGiftRedemption]    Script Date: 2013/10/20 下午 06:31:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/20 下午 06:31:51
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateGiftRedemption]
	-- Access Object
    @access_object_id INT,
	@access_object_type INT, 
	-- Data
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
	@new_redemption_id INT OUTPUT,
    @sql_result INT OUTPUT
AS
    -- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')
        
    SET @sql_result = 0
	
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_CreateGiftRedemption
    
    -- Check Access Object
	--SELECT @accessObjectValid = 1, @accessObjectType = ao.type
 --   FROM v_accessObject ao
 --   WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    SET @accessObjectValid = 1
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1
    		  
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO gift_redemption
                    (
						transaction_id, 
						redemption_code,
                        redemption_channel, 
                        member_id, 
                        gift_id, 
                        quantity, 
                        point_used, 
                        redemption_status, 
                        collect_date, 
                        collect_location_id, 
                        void_date, 
                        void_user_id, 
                        remark, 
                        status, 

                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                     
                    )
                    VALUES (
                        
                        @transaction_id,  --transaction_id
						@redemption_code,
                        @redemption_channel,  --redemption_channel
                        @member_id,  --member_id
                        @gift_id,  --gift_id
                        @quantity,  --quantity
                        @point_used,  --point_used
                        @redemption_status,  --redemption_status
                        @collect_date,  --collect_date
                        @collect_location_id,  --collect_location_id
                        @void_date,  --void_date
                        @void_user_id,  --void_user_id
                        @remark,  --remark
                        @status,  --status

                        GETDATE(),  --crt_date
                        @access_object_type,  --crt_by_type
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
                        @access_object_type,  --upd_by_type
                        @access_object_id  --upd_by
                    )                   
                      
                    SET @new_redemption_id = SCOPE_IDENTITY()
                    SET @sql_result = 100 --normal
                END
	    	ELSE
    		    BEGIN
        			SET @sql_result = 1112  --Record Invalid
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_result = 1111 --no permission
		END
        
    -- Commit Transaction
    COMMIT TRAN sp_CreateGiftRedemption
END
GO