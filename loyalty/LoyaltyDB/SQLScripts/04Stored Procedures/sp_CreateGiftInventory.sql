IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateGiftInventory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateGiftInventory]
GO

/****** Object:  StoredProcedure [sp_CreateGiftInventory]    Script Date: 2013/10/18 上午 11:07:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/18 上午 11:07:03
-- Description:	Stored Procedure for CreateGiftInventory
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateGiftInventory]
	-- Access Object
    @access_object_id INT,
    @access_object_type INT, 
	-- Data
	@source_id int,
	@location_id int,
	@gift_id int,
	@stock_change_type int,
	@stock_change int,
	@remark nvarchar(max),
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
    BEGIN TRAN sp_CreateGiftInventory
    
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
                    
                    INSERT INTO gift_inventory
                    (
                        source_id, 
                        location_id, 
                        gift_id, 
                        stock_change_type, 
                        stock_change, 
                        remark, 
                        status, 
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
    
                    )
                    VALUES (
                        @source_id,  --source_id
                        @location_id,  --location_id
                        @gift_id,  --gift_id
                        @stock_change_type,  --stock_change_type
                        @stock_change,  --stock_change
                        @remark,  --remark
                        @status,  --status
                        GETDATE(),  --crt_date
                        @access_object_type,  --crt_by_type
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
                        @access_object_type,  --upd_by_type
                        @access_object_id --upd_by
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
    COMMIT TRAN sp_CreateGiftInventory
END
GO