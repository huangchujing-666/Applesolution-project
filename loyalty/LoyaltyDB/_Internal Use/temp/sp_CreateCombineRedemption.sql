IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateCombineRedemption]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateCombineRedemption]
GO

/****** Object:  StoredProcedure [sp_CreateCombineRedemption]    Script Date: 2015/5/20 下午 06:59:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2015/5/20 下午 06:59:54
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateCombineRedemption]
    -- Access Object
    @access_object_id INT,
    
    -- Data
	@member_id int,
	@coupon_id int,
	
	@position int,
	@no_of_ppl int,
	@join_combine_id int,
	@notified_host int,
	@status int,

    -- Output
    @new_id  INT OUTPUT,
    @sql_result INT OUTPUT
AS
	-- access object params
    DECLARE @access_object_valid INT SET @access_object_valid = 0
	DECLARE @access_object_type INT SET @access_object_type = 0
    
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
    BEGIN TRAN sp_CreateCombineRedemption
    
    -- Check Access Object
    SELECT @access_object_valid = 1, @access_object_type = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @access_object_valid = 1
		BEGIN
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO combine_redemption
                    (
                        
                        member_id, 
						coupon_id,
                        position, 
                        no_of_ppl, 
                        join_combine_id, 
                        notified_host, 
                        status, 
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by, 
                        record_status
                    )
                    VALUES (
                        
                        @member_id,  --member_id
						@coupon_id, --coupon_id
                        @position,  --position
                        @no_of_ppl,  --no_of_ppl
                        @join_combine_id,  --join_combine_id
                        @notified_host,  --notified_host
                        @status,  --status
                        GETDATE(), --crt_date
                        @access_object_type, --crt_by_type
                        @access_object_id, --crt_by
                        GETDATE(), --upd_date
                        @access_object_type, --upd_by_type
                        @access_object_id, --upd_by
                        0 --record_status
                    )                   
                      
                    SET @new_id = SCOPE_IDENTITY()
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
    COMMIT TRAN sp_CreateCombineRedemption
END
GO