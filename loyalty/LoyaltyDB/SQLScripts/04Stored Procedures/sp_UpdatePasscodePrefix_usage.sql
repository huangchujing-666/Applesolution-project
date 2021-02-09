IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdatePasscodePrefix_usage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdatePasscodePrefix_usage]
GO

/****** Object:  StoredProcedure [sp_UpdatePasscodePrefix_usage]    Script Date: 2013/8/12 上午 10:51:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/12 上午 10:51:44
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdatePasscodePrefix_usage]
	-- Access Object
    @access_object_id INT,

	-- Data
	@prefix_id int,

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

    
    DECLARE @current_generated bigint
	DECLARE @maximum_generate bigint
	DECLARE @usage_precent float

        
    SET @sql_result = 0
	SET @sql_remark = ''
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_UpdatePasscodePrefix_usage
    
	-- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1, @maximum_generate = pf.maximum_generate
    		FROM passcode_prefix pp, passcode_format pf
    		WHERE pp.prefix_id = @prefix_id 
				AND pp.record_status <> @recDeleted
				AND pf.record_status <> @recDeleted
				AND pp.format_id = pf.format_id
            
            IF @recValid = 1
    		    BEGIN

					SELECT @current_generated = count(*)
					FROM passcode
					WHERE passcode_prefix_id = @prefix_id

					SET @usage_precent= ROUND(Convert (float,(@current_generated))/Convert (float,(@maximum_generate)) * 100, 4)

                    UPDATE passcode_prefix
                    SET
                        current_generated = @current_generated, 
						usage_precent = @usage_precent, 
						upd_date = GETDATE(), 
                        upd_by = @access_object_id

                    WHERE prefix_id = @prefix_id
                    
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
    COMMIT TRAN sp_UpdatePasscodePrefix_usage
END
GO
    
    
