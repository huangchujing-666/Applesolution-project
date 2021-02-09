IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreatePasscodePrefix]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreatePasscodePrefix]
GO

/****** Object:  StoredProcedure [sp_CreatePasscodePrefix]    Script Date: 2013/8/19 下午 12:26:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/19 下午 12:26:04
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreatePasscodePrefix]
	-- Access Object
    @access_object_id INT,
    
	-- Data
	@product_id int,
	@format_id bigint,
	@prefix_value varchar(10),
	@current_generated bigint,
	@usage_precent float,
	@status int,

	-- Outpput
	@prefix_id INT OUTPUT,
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
    BEGIN TRAN sp_CreatePasscodePrefix
    
    -- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1
    	
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO passcode_prefix
                    (
                        
                        product_id, 
                        format_id, 
                        prefix_value, 
                        current_generated, 
                        usage_precent, 
                        status, 
                        crt_date, 
                        upd_date, 
                        crt_by, 
                        upd_by
                    )
                    VALUES (
                      
                        @product_id,  --product_id
                        @format_id,  --format_id
                        @prefix_value,  --prefix_value
                        @current_generated,  --current_generated
                        @usage_precent,  --usage_precent
                        @status,  --status
                        GETDATE(),  --crt_date
                        GETDATE(),  --upd_date
                        @access_object_id,  --crt_by
                        @access_object_id  --upd_by
             
                    )                   
                      
                    SELECT @prefix_id = SCOPE_IDENTITY()

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
    COMMIT TRAN sp_CreatePasscodePrefix
END
GO
    
    
