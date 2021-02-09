IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateLocation]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateLocation]
GO

/****** Object:  StoredProcedure [sp_CreateLocation]    Script Date: 2013/10/16 下午 05:09:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/16 下午 05:09:32
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateLocation]
	-- Access Object
    @access_object_id INT,
    @access_object_type INT,
	-- Data
	@location_no varchar(20),
	@type int,
	@photo_file_name varchar(1000),
	@photo_file_extension varchar(10),
	@latitude float,
	@longitude float,
	@phone varchar(12),
	@fax varchar(12),
	@address_district int,
	@address_region int,
	@display_order int,
	@status int,
	
	-- Output
	@location_id INT OUTPUT,
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
    BEGIN TRAN sp_CreateLocation
    
    -- Check Access Object
	--SELECT @accessObjectValid = 1, @accessObjectType = ao.type
 --   FROM v_accessObject ao
 --   WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    SET @accessObjectValid = 1
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO location
                    (
                        location_no, 
						type,
                        photo_file_name, 
                        photo_file_extension, 
                        latitude, 
                        longitude, 
                        phone, 
                        fax, 
                        address_district, 
                        address_region, 
                        display_order, 
                        status, 
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
                        @location_no,  --location_no
						@type,
                        @photo_file_name,  --photo_file_name
                        @photo_file_extension,  --photo_file_extension
                        @latitude,  --latitude
                        @longitude,  --longitude
                        @phone,  --phone
                        @fax,  --fax
                        @address_district,  --address_district
                        @address_region,  --address_region
                        @display_order,  --display_order
                        @status,  --status
                        GETDATE(),  --crt_date
                        @access_object_type,  --crt_by_type
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
                        @access_object_type,  --upd_by_type
                        @access_object_id  --upd_by
                    )                   
                      
                    SET @sql_result = 1

					SET @location_id = SCOPE_IDENTITY()
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
    COMMIT TRAN sp_CreateLocation
END
GO