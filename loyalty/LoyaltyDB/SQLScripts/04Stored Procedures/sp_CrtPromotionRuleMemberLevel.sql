IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CrtPromotionRuleMemberLevel]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CrtPromotionRuleMemberLevel]
GO

/****** Object:  StoredProcedure [sp_CrtPromotionRuleMemberLevel]    Script Date: 2014/2/28 下午 03:01:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/2/28 下午 03:01:02
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CrtPromotionRuleMemberLevel]
    -- Access Object
    @access_object_id INT,
    
    -- Data
	@rule_id int,
	@member_level_id int,
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
    BEGIN TRAN sp_CrtPromotionRuleMemberLevel
    
    -- Check Access Object
    SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO promotion_rule_member_level
                    (
                        rule_id, 
                        member_level_id, 
                        status, 
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
                        @rule_id,  --rule_id
                        @member_level_id,  --member_level_id
                        @status,  --status
                       	GETDATE(),  --crt_date
                        @accessObjectType,  --crt_by_type
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
                        @accessObjectType,  --upd_by_type
                        @access_object_id  --upd_by
                    )                   
                    
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
    COMMIT TRAN sp_CrtPromotionRuleMemberLevel
END
GO