IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateCombineRedemption]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateCombineRedemption]
GO

/****** Object:  StoredProcedure [sp_UpdateCombineRedemption]    Script Date: 2015/5/21 下午 04:02:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2015/5/21 下午 04:02:33
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateCombineRedemption]
    -- Access Object
    @access_object_id INT,
    
    -- Data
	@combine_id int,
	@member_id int,
	@coupon_id int,
	@position int,
	@no_of_ppl int,
	@join_combine_id int,
	@notified_host int,
	@status int,

    -- Output
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
    BEGIN TRAN sp_UpdateCombineRedemption
    
    -- Check Access Object
    SELECT @access_object_valid = 1, @access_object_type = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @access_object_valid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM combine_redemption
    		WHERE combine_id = @combine_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                
                   UPDATE combine_redemption
                   SET
                        member_id = @member_id, 
						coupon_id = @coupon_id,
                        position = @position, 
                        no_of_ppl = @no_of_ppl, 
                        join_combine_id = @join_combine_id, 
                        notified_host = @notified_host, 
                        status = @status, 
                        upd_date = GETDATE(),
                        upd_by_type = @access_object_type,
                        upd_by = @access_object_id
                    WHERE combine_id = @combine_id
                    
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
    COMMIT TRAN sp_UpdateCombineRedemption
END
GO