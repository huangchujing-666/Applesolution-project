IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetLocationLangDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetLocationLangDetail]
GO

/****** Object:  StoredProcedure [sp_GetLocationLangDetail]    Script Date: 2013/10/16 下午 08:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/16 下午 08:23:46
-- Description:	Stored Procedure for sp_GetLocationLangDetail
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetLocationLangDetail]
	-- Access Object
    @access_object_id INT,

	-- Data
    @location_id INT,

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
    		FROM location_lang
    		WHERE location_id = @location_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                    
                    SELECT 
                        l.location_lang_id, 
                        l.location_id, 
                        l.lang_id, 
                        l.name, 
                        l.description, 
                        l.operation_info, 
                        l.address_unit, 
                        l.address_building, 
                        l.address_street, 
                        l.status, 
                        l.crt_date, 
                        l.crt_by_type, 
                        l.crt_by, 
                        l.upd_date, 
                        l.upd_by_type, 
                        l.upd_by, 
                        l.record_status
                    FROM  location_lang l
                    WHERE location_id = @location_id
                    AND l.[record_status] <> @recDeleted
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                    SELECT 
                        l.location_lang_id, 
                        l.location_id, 
                        l.lang_id, 
                        l.name, 
                        l.description, 
                        l.operation_info, 
                        l.address_unit, 
                        l.address_building, 
                        l.address_street, 
                        l.status, 
                        l.crt_date, 
                        l.crt_by_type, 
                        l.crt_by, 
                        l.upd_date, 
                        l.upd_by_type, 
                        l.upd_by, 
                        l.record_status
                    FROM  location_lang l
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