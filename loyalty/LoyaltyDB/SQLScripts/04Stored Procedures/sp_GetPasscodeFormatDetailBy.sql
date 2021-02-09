IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetPasscodeFormatDetailBy]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetPasscodeFormatDetailBy]
GO

/****** Object:  StoredProcedure [sp_GetPasscodeFormatDetailBy]    Script Date: 2013/8/9 下午 12:04:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/9 下午 12:04:45
-- Description:	Stored Procedure for Get Passcode Format
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetPasscodeFormatDetailBy]
	-- Access Object
    @access_object_id INT,
    
	-- Data
	@format_id INT,

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
    
    
   -- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM passcode_format
    		WHERE format_id = @format_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                    
                    SELECT 
                        p.format_id, 
                        p.passcode_format, 
                        p.number_combination, 
                        p.safetyLimit_precent, 
                        p.maximum_generate, 
                        p.expiry_month, 
                        p.status, 
                        p.crt_date, 
                        p.upd_date, 
                        p.crt_by, 
                        p.upd_by, 
                        p.record_status
                    FROM  passcode_format p
                    WHERE format_id = @format_id
                    AND p.[record_status] <> @recDeleted
                    
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
        
END
GO
    
    
