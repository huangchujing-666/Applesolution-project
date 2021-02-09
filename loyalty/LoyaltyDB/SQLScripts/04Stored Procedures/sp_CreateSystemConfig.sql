IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateSystemConfig]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateSystemConfig]
GO

/****** Object:  StoredProcedure [sp_CreateSystemConfig]    Script Date: 2013/8/19 下午 05:04:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/19 下午 05:04:54
-- Description:	Stored Procedure for Create System Config
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateSystemConfig]
	-- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	-- Data
	@name varchar(50),
	@value varchar(100),
	@display bit,
	@display_order int,
	
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

	DECLARE @nameDuplicate INT SET @nameDuplicate   = 0
        
    SET @sql_result = 0
	SET @sql_remark = ''
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_CreateSystemConfig
    
    -- Check Access Object
---	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF (@accessObjectValid = 1)
		BEGIN
		    
			SELECT @nameDuplicate = 1
			FROM system_config
			WHERE name = @name

            IF (@nameDuplicate = 0)
				SET @recValid = 1

			IF @recValid = 1
    		    BEGIN
                    
					 INSERT INTO system_config
                    (
                        name, 
                        value, 
                        display, 
                        display_order, 
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
                      
                        @name,  --name
                        @value,  --value
                        @display,  --display
                        @display_order,  --display_order
                        GETDATE(),  --crt_date
@access_object_type,
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
@access_object_type,
                        @access_object_id --upd_by
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
    COMMIT TRAN sp_CreateSystemConfig
END
GO
