IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateLog]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateLog]
GO

/****** Object:  StoredProcedure [sp_CreateLog]    Script Date: 2013/11/19 下午 05:38:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/11/19 下午 05:38:27
-- Description:	Stored Procedure for Create Log
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateLog]
	-- Access Object
    @access_object_id INT,
    @access_object_type INT, 
	-- Data
	@action_ip varchar(50),
	@action_channel int,
	@action_type int,

	@target_obj_type_id bigint,
	@target_obj_id bigint,	
	@target_obj_name nvarchar(100),
	@action_detail nvarchar(max),
	
	-- Output
	@new_log_id int OUTPUT,
    @sql_result int OUTPUT
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
    BEGIN TRAN sp_CreateLog
    
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
                    
                    INSERT INTO log
                    (
                        action_ip, 
                        action_channel, 
                        action_type, 
                        target_obj_id, 
						target_obj_type_id,
						target_obj_name,
                        action_detail, 
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
                        @action_ip,  --action_ip
                        @action_channel,  --action_channel
                        @action_type,  --action_type
                        @target_obj_id,  --target_obj_id
						@target_obj_type_id, --target_obj_type_id
						@target_obj_name, --target_obj_name
                        @action_detail,  --action_detail
                        GETDATE(),  --crt_date
                        @access_object_type,  --crt_by_type
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
                        @access_object_type,  --upd_by_type
                        @access_object_id  --upd_by
                    )
                              
					SET @new_log_id = SCOPE_IDENTITY()
					
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
    COMMIT TRAN sp_CreateLog

END
GO