IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateBasicRule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateBasicRule]
GO

/****** Object:  StoredProcedure [sp_UpdateBasicRule]    Script Date: 2014/3/20 下午 06:16:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/3/20 下午 06:16:27
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateBasicRule]
    -- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
    -- Data
	@basic_rule_id int,
	@type int,
	@target_id int,
	@member_level_id int,
	@memebr_category_id int,
	@ratio_payment float,
	@ratio_point float,
	@point float,
	@point_expiry_month int,
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
    BEGIN TRAN sp_UpdateBasicRule
    
    -- Check Access Object
---    SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM basic_rule
    		WHERE basic_rule_id = @basic_rule_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                
                    UPDATE basic_rule
                    SET
                        type = @type, 
                        target_id = @target_id, 
                        member_level_id = @member_level_id, 
						memebr_category_id = @memebr_category_id,
                        ratio_payment = @ratio_payment, 
                        ratio_point = @ratio_point, 
						point = @point,
						point_expiry_month = @point_expiry_month,
                        remark = @remark, 
                        status = @status, 
                        upd_date = GETDATE(), 
upd_by_type = @access_object_type, 
                        upd_by = @access_object_id
                    WHERE basic_rule_id = @basic_rule_id
                    
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
    COMMIT TRAN sp_UpdateBasicRule
END
GO
