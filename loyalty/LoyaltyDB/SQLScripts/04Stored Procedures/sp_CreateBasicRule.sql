IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateBasicRule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateBasicRule]
GO

/****** Object:  StoredProcedure [sp_CreateBasicRule]    Script Date: 2014/5/5 下午 12:03:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/5/5 下午 12:03:30
-- Description:	Stored Procedure for Create Basic Rule
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateBasicRule]
    -- Access Object
    @access_object_id INT,
    @access_object_type INT,
    -- Data
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
    BEGIN TRAN sp_CreateBasicRule
    
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
                    
                    INSERT INTO basic_rule
                    (
                        type, 
                        target_id, 
                        member_level_id, 
                        memebr_category_id, 
                        ratio_payment, 
                        ratio_point, 
                        point, 
                        point_expiry_month, 
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
                        @type,  --type
                        @target_id,  --target_id
                        @member_level_id,  --member_level_id
                        @memebr_category_id,  --memebr_category_id
                        @ratio_payment,  --ratio_payment
                        @ratio_point,  --ratio_point
                        @point,  --point
                        @point_expiry_month,  --point_expiry_month
                        @remark,  --remark
                        @status,  --status
                        GETDATE(),  --crt_date
                        @access_object_type,  --crt_by_type
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
                        @access_object_type,  --upd_by_type
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
    COMMIT TRAN sp_CreateBasicRule
END
GO