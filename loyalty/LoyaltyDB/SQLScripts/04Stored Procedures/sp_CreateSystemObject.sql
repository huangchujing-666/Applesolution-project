IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateSystemObject]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateSystemObject]
GO

/****** Object:  StoredProcedure [sp_CreateSystemObject]    Script Date: 2013/11/18 下午 03:21:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/11/18 下午 03:21:38
-- Description:	Stored Procedure for sp_CreateSystemObject
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateSystemObject]
	-- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	-- Data
	@type int,
	@target_id int,
	@name nvarchar(100),
	@status int,
	@power_search nvarchar(max),
	
	-- Output
	@new_object_id INT OUTPUT,
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
    
    SET @recValid = 0
        
    SET @sql_result = 0
	SET @sql_remark = ''
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_CreateSystemObject
    
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

                    INSERT INTO system_object
                    (
                        type,
						target_id, 
						name,
                        status, 
                        power_search, 
                        crt_date, 
                        crt_by_type,
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
                        @type,  --type
						@target_id,
						@name,
                        @status,  --status
                        @power_search,  --power_search
                        GETDATE(),  --crt_date
@access_object_type,
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
@access_object_type,
                        @access_object_id  --upd_by
                    )

                    SET @sql_result = 1

					SET @new_object_id = SCOPE_IDENTITY()
                END
	    	ELSE
    		    BEGIN
        			SET @sql_remark = 'RecordInvalid'
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_remark = 'AccessObjectInvalid'
		END
        
    -- Commit Transaction
    COMMIT TRAN sp_CreateSystemObject
END
GO
