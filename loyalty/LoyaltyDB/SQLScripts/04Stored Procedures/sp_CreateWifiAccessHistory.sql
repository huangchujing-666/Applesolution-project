IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateWifiAccessHistory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateWifiAccessHistory]
GO

/****** Object:  StoredProcedure [sp_CreateWifiAccessHistory]    Script Date: 2014/4/25 下午 04:05:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/4/25 下午 04:05:25
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateWifiAccessHistory]
    -- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
    -- Data
	
	@location_id int,
	@member_id int,
	@client_ip varchar(50),
	@client_mac_address varchar(50),
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
    BEGIN TRAN sp_CreateWifiAccessHistory
    
    -- Check Access Object
---    SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO wifi_access_history
                    (
                        location_id, 
                        member_id, 
                        client_ip, 
                        client_mac_address, 
                        status, 
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
                        @location_id,  --location_id
                        @member_id,  --member_id
                        @client_ip,  --client_ip
                        @client_mac_address,  --client_mac_address
                        @status,  --status
                        GETDATE(),  --crt_date
@access_object_type,
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
@access_object_type,
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
    COMMIT TRAN sp_CreateWifiAccessHistory
END
GO
