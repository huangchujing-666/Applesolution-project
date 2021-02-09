IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateGiftCategory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateGiftCategory]
GO

/****** Object:  StoredProcedure [sp_CreateGiftCategory]    Script Date: 2013/10/17 下午 02:22:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/17 下午 02:22:26
-- Description:	Stored Procedure for CreateGiftCategory
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateGiftCategory]
    -- Access Object
    @access_object_id INT,
    @access_object_type INT, 
	-- Data
	@parent_id INT,
	@leaf INT,
	@photo_file_name varchar(1000),
	@photo_file_extension varchar(10),
	@display_order int,
	@status int,
	
	-- OUTPUT
	@category_id INT OUTPUT,
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
    
	-- Set default output value        
    SET @sql_result = 0
	SET @sql_remark = ''
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_CreateGiftCategory
    
    -- Check Access Object
    --SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    --FROM v_accessObject ao
    --WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
	SET @accessObjectValid =1
    IF @accessObjectValid = 1
		BEGIN
		    
            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO gift_category
                    (
						parent_id,
						leaf,
                        photo_file_name, 
                        photo_file_extension, 
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
						@parent_id, --parent_id
						@leaf,
                        @photo_file_name,  --photo_file_name
                        @photo_file_extension,  --photo_file_extension
                        @display_order,  --display_order
                        @status,  --status
                        GETDATE(),  --crt_date
                        @access_object_type,  --crt_by_type
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
                        @access_object_type,  --upd_by_type
                        @access_object_id --upd_by
                    )                   
                    
                    SET @sql_result = 1

					SET @category_id = SCOPE_IDENTITY()
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
    COMMIT TRAN sp_CreateGiftCategory
END
GO