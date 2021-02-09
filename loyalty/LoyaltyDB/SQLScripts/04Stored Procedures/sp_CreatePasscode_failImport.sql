IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreatePasscode_failImport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreatePasscode_failImport]
GO

/****** Object:  StoredProcedure [sp_CreatePasscode_failImport]    Script Date: 2013/10/4 下午 05:49:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/4 下午 05:49:14
-- Description:	Stored Procedure for Create Passcode Fail Import records
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreatePasscode_failImport]
	-- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	-- Data
	@generate_id int,
	@excel_row_no int,
	@passcode_id bigint,
	@error_message nvarchar(max),
	
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
    
    -- Begin Transaction
    BEGIN TRAN sp_CreatePasscode_failImport
    
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
                    
                    INSERT INTO passcode_failImport
                    (
                        generate_id, 
                        excel_row_no, 
                        passcode_id, 
                        error_message,
                        crt_date,
                        crt_by_type,
                        crt_by,
                        upd_date,
                        upd_by_type,
                        upd_by
                    )
                    VALUES (
                        
                        @generate_id,  --generate_id
                        @excel_row_no,  --excel_row_no
                        @passcode_id,  --passcode_id
                        @error_message,  --error_message
                        GETDATE(),  --crt_date
@access_object_type,
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
@access_object_type,
                        @access_object_id  --upd_by
                    )

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
    COMMIT TRAN sp_CreatePasscode_failImport
END
GO
