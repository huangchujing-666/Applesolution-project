IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreatePasscode]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreatePasscode]
GO

/****** Object:  StoredProcedure [sp_CreatePasscode]    Script Date: 2013/8/21 下午 12:22:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/21 下午 12:22:05
-- Description:	Stored Procedure for Create Passcode
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreatePasscode]
	-- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	-- Data
	@pin_value varchar(30),
	@generate_id int,
	@passcode_prefix_id int,
	@active_date datetime,
	@expiry_date datetime,
	
	@product_id int,
	@point float,

	@status int,
	@void_date datetime,
	@void_reason int,

	-- Output
	@new_passcode_id BIGINT OUTPUT,
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
    BEGIN TRAN sp_CreatePasscode
    
    -- Check Access Object
---	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1
    	
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO passcode
                    (
                        pin_value, 
                        generate_id, 
                        passcode_prefix_id, 
                        active_date, 
                        expiry_date, 

						product_id,
						point, 
                      
                        void_date, 
                        void_reason, 
                        status, 
                        crt_date, 
						crt_by_type,
						crt_by,

                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
                        @pin_value,  --pin_value
                        @generate_id,  --generate_id
                        @passcode_prefix_id,  --passcode_prefix_id
                        @active_date,  --active_date
                        @expiry_date,  --expiry_date
                        
						@product_id,
						@point,  --point                   
                        
                        @void_date,  --void_date
                        @void_reason,  --void_reason
                        @status,  --status
                        GETDATE(),  --crt_date
@access_object_type,
						@access_object_id,  --crt_by
						
						GETDATE(),  --upd_date
@access_object_type,
                        @access_object_id  --upd_by
                    )
                    
                    SET @sql_result = 1

					SET @new_passcode_id = SCOPE_IDENTITY()
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
    COMMIT TRAN sp_CreatePasscode
END
GO
