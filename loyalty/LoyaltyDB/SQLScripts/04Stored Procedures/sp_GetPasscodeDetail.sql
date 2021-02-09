IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetPasscodeDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetPasscodeDetail]
GO

/****** Object:  StoredProcedure [sp_GetPasscodeDetail]    Script Date: 2013/10/20 下午 04:48:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/20 下午 04:48:36
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetPasscodeDetail]
	-- Access Object
    @access_object_id INT,
    
	-- Data
	@passcode_id BIGINT,
	@pin_value VARCHAR(30),

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
    		FROM passcode
    		WHERE (passcode_id = @passcode_id OR pin_value=@pin_value) AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN

                    SELECT 
                        p.passcode_id, 
						p.pin_value,
                        p.generate_id, 
                        p.passcode_prefix_id, 
                        p.active_date, 
                        p.expiry_date, 
                        p.point, 
                        p.product_id, 
                        p.status, 
                        p.registered, 
                        p.void_date, 
                        p.void_reason, 
                        p.crt_date, 
                        p.crt_by_type, 
                        p.crt_by, 
                        p.upd_date, 
                        p.upd_by_type, 
                        p.upd_by, 
                        p.record_status
                    FROM  passcode p
                    WHERE (passcode_id = @passcode_id OR pin_value=@pin_value)
                    AND p.[record_status] <> @recDeleted
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                    SELECT 
                        p.passcode_id, 
						p.pin_value,
                        p.generate_id, 
                        p.passcode_prefix_id, 
                        p.active_date, 
                        p.expiry_date, 
                        p.point, 
                        p.product_id, 
                        p.status, 
                        p.registered, 
                        p.void_date, 
                        p.void_reason, 
                        p.crt_date, 
                        p.crt_by_type, 
                        p.crt_by, 
                        p.upd_date, 
                        p.upd_by_type, 
                        p.upd_by, 
                        p.record_status
                    FROM  passcode p
                    WHERE 1=2
        			SET @sql_remark = 'RecordInvalid'
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_remark = 'UserInvalid'
		END
        
END
GO