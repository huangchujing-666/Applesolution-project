IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdatePasscode]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdatePasscode]
GO

/****** Object:  StoredProcedure [sp_UpdatePasscode]    Script Date: 2013/10/20 下午 05:55:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/20 下午 05:55:22
-- Description:	Stored Procedure for UpdatePasscode
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdatePasscode]
	-- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	-- Data
	@passcode_id BIGINT,
	@pin_value varchar(30),
	@generate_id int,
	@passcode_prefix_id int,
	@active_date datetime,
	@expiry_date datetime,
	@point float,
	@product_id int,
	@status int,
	@registered int,
	@void_date datetime,
	@void_reason nvarchar(max),
	
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
        
    SET @sql_result = 0
	
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_UpdatePasscode
    
    -- Check Access Object
---	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM passcode
    		WHERE passcode_id = @passcode_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                    UPDATE passcode
                    SET
						pin_value = @pin_value,
                        generate_id = @generate_id, 
                        passcode_prefix_id = @passcode_prefix_id, 
                        active_date = @active_date, 
                        expiry_date = @expiry_date, 
                        point = @point, 
                        product_id = @product_id, 
                        status = @status, 
                        registered = @registered, 
                        void_date = @void_date, 
                        void_reason = @void_reason, 
                      
                        upd_date = GETDATE(), 
upd_by_type = @access_object_type, 
                        upd_by = @access_object_id
                       
                    WHERE passcode_id = @passcode_id
                    
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
    COMMIT TRAN sp_UpdatePasscode
END
