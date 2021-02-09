IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateMemberAdvanceSearchRule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateMemberAdvanceSearchRule]
GO

/****** Object:  StoredProcedure [sp_CreateMemberAdvanceSearchRule]    Script Date: 2015/10/12 下午 05:06:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2015/10/12 下午 05:06:43
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateMemberAdvanceSearchRule]
    -- Access Object
    @access_object_id INT,
    @access_object_type INT, 
    -- Data
	@search_id int,
	@group_id int,
	@row_id int,
	@target_field int,
	@target_condition int,
	@target_value nvarchar(500),
	
    -- Output
    @new_id bigint OUTPUT,
    @sql_result INT OUTPUT
AS
	-- access object params
    DECLARE @access_object_valid INT SET @access_object_valid = 0
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
    BEGIN TRAN sp_CreateMemberAdvanceSearchRule
    
    -- Check Access Object
    --SELECT @access_object_valid = 1, @access_object_type = ao.type
    --FROM v_accessObject ao
    --WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
	SET @access_object_valid = 1

    IF @access_object_valid = 1
		BEGIN
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO member_advance_search_rule
                    (
                        search_id, 
                        group_id, 
						row_id,
                        target_field, 
                        target_condition, 
                        target_value, 
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (

                        @search_id,  --search_id
                        @group_id,  --group_id
						@row_id, --row_id
                        @target_field,  --target_field
                        @target_condition,  --target_condition
                        @target_value,  --target_value
                        GETDATE(), --crt_date
                        @access_object_type, --crt_by_type
                        @access_object_id, --crt_by
                        GETDATE(), --upd_date
                        @access_object_type, --upd_by_type
                        @access_object_id --upd_by                        
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
    COMMIT TRAN sp_CreateMemberAdvanceSearchRule
END
GO