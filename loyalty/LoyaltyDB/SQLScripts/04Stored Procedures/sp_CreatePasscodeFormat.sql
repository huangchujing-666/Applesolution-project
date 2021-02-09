IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreatePasscodeFormat]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreatePasscodeFormat]
GO

/****** Object:  StoredProcedure [sp_CreatePasscodeFormat]    Script Date: 2013/8/8 下午 06:16:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/8 下午 06:16:32
-- Description:	Stored Procedure for Create Passcode Format
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreatePasscodeFormat]
	-- Access Object
    @access_object_id INT,

	-- Data
    @format_id INT,

	@passcode_format varchar(50),
	@number_combination bigint,
	@safetyLimit_precent float,
	@maximum_generate bigint,
	@expiry_month int,
	@status int,

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
    
    -- Begin Transaction
    BEGIN TRAN sp_CreatePasscodeFormat
    
    -- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF (@accessObjectValid = 1 OR @access_object_id=0) --user_id=0 for installation
		BEGIN
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                    
					SET IDENTITY_INSERT passcode_format ON

                    INSERT INTO passcode_format
                    (
						format_id,
                        passcode_format, 
                        number_combination, 
                        safetyLimit_precent, 
						maximum_generate,
                        expiry_month, 
                        status, 
                        crt_date, 
                        upd_date, 
                        crt_by, 
                        upd_by
                    )
                    VALUES (
						@format_id, 
                        @passcode_format,  --passcode_format
                        @number_combination,  --number_combination
                        @safetyLimit_precent,  --safetyLimit_precent
						@maximum_generate,
                        @expiry_month,  --expiry_month
                        @status,  --status
                        GETDATE(),  --crt_date
                        GETDATE(),  --upd_date
                        @access_object_id,  --crt_by
                        @access_object_id  --upd_by
                    )                   
                      
                    SET IDENTITY_INSERT passcode_format OFF

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
    COMMIT TRAN sp_CreatePasscodeFormat
END
GO
    
    
