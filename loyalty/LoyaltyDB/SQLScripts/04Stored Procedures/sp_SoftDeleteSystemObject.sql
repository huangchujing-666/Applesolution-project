IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_SoftDeleteSystemObject]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_SoftDeleteSystemObject]
GO

/****** Object:  StoredProcedure [sp_SoftDeleteSystemObject]    Script Date: 2013/11/19 下午 04:44:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/11/19 下午 04:44:43
-- Description:	Stored Procedure for Soft Delete System Object
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_SoftDeleteSystemObject]
	-- Access Object
    @access_object_id INT,
     @access_object_type INT,
    
	-- Data
	@object_id int,
	
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
    BEGIN TRAN sp_SoftDeleteSystemObject
    
    -- Check Access Object
	--SELECT @accessObjectValid = 1, @accessObjectType = ao.type
   -- FROM v_accessObject ao
  --  WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
	SET @accessObjectValid = 1
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM system_object
    		WHERE object_id = @object_id
            
            IF @recValid = 1
    		    BEGIN
                    UPDATE system_object
                    SET
                        record_status = @recDeleted, 
						  upd_date = GETDATE(), 
							upd_by_type = @access_object_type, 
                        upd_by = @access_object_id
                    WHERE object_id = @object_id
                    
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
    COMMIT TRAN sp_SoftDeleteSystemObject
END
GO