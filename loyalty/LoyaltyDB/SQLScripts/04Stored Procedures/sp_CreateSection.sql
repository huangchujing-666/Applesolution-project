IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateSection]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateSection]
GO

/****** Object:  StoredProcedure [sp_CreateSection]    Script Date: 2013/8/21 下午 03:08:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/21 下午 03:08:23
-- Description:	Stored Procedure for Create Section
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateSection]
	-- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	-- Data
	@section_id int,
	@parent int,
	@name nvarchar(100),
	@icon int,
	@link varchar(100),
	@display int,
	@display_order int,
	@module varchar(40),
	@status int,
	
	@leaf int,

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
    BEGIN TRAN sp_CreateSection
    
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
                    
					IF (@section_id IS NULL)
					BEGIN
						INSERT INTO section
						(
							
							parent, 
							name, 
							icon, 
							link, 
							display, 
							display_order, 
							module, 
							status, 
							crt_date, 
							upd_date, 
							crt_by, 
							upd_by, 
							leaf
						)
						VALUES (
							
							@parent,  --parent
							@name,  --name
							@icon,  --icon
							@link,  --link
							@display,  --display
							@display_order,  --display_order
							@module,  --module
							@status,  --status
							GETDATE(),  --crt_date
							GETDATE(),  --upd_date
@access_object_type,
							@access_object_id,  --upd_by
							@leaf --leaf
						)                   
                      
						SET @sql_result = 1
					END
					ELSE
					BEGIN
						SET IDENTITY_INSERT section ON

						INSERT INTO section
						(
							section_id, 
							parent, 
							name, 
							icon, 
							link, 
							display, 
							display_order, 
							module, 
							status, 
							crt_date, 
							upd_date, 
							crt_by, 
							upd_by, 
							leaf
						)
						VALUES (
							@section_id,  --section_id
							@parent,  --parent
							@name,  --name
							@icon,  --icon
							@link,  --link
							@display,  --display
							@display_order,  --display_order
							@module,  --module
							@status,  --status
							GETDATE(),  --crt_date
							GETDATE(),  --upd_date
@access_object_type,
							@access_object_id,  --upd_by
							@leaf --leaf
						)                   
                      
						SET @sql_result = 1

						SET IDENTITY_INSERT section OFF
					END

						
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
    COMMIT TRAN sp_CreateSection
END
GO
    
    
