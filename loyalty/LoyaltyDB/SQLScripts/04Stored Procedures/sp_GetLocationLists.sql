IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetLocationLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetLocationLists]
GO

/****** Object:  StoredProcedure [sp_GetLocationLists]    Script Date: 2013/10/16 下午 03:52:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/16 下午 03:52:01
-- Description:	Stored Procedure for Get Location Lists
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetLocationLists]
   	-- Access Object
    @access_object_id INT,
   
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
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN

                    SELECT 
                        l.location_id,
						l.location_no,
                        l.photo_file_name, 
                        l.photo_file_extension, 
                        l.latitude, 
                        l.longitude, 
                        l.phone, 
                        l.fax, 
						ll.name,
						ll.description,
						ll.operation_info,
						ll.address_unit,
						ll.address_building,
						ll.address_street,
                        l.address_district, 
                        l.address_region, 
                        l.display_order, 
                        l.status,
						listi.name 'status_name'
                      
                    FROM  location l, location_lang ll, listing list, listing_item listi
                    WHERE 1=1
					AND l.[record_status] <> @recDeleted
					AND l.location_id = ll.location_id
					AND ll.lang_id = 2 -- ISNULL(@lang_id, ll.lang_id)
					AND l.[status] = listi.[value]
					AND list.[list_id] = listi.[list_id]
					AND list.code = 'Status'
                    ORDER BY l.location_no collate Latin1_General_CS_AI -- non case sensitive

                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                    SELECT 
                        l.location_id,
						l.location_no,
                        l.photo_file_name, 
                        l.photo_file_extension, 
                        l.latitude, 
                        l.longitude, 
                        l.phone, 
                        l.fax, 
						ll.name,
						ll.description,
						ll.operation_info,
						ll.address_unit,
						ll.address_building,
						ll.address_street,
                        l.address_district, 
                        l.address_region, 
                        l.display_order, 
                        l.status,
						listi.name 'status_name'
                      
                    FROM  location l, location_lang ll, listing list, listing_item listi
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