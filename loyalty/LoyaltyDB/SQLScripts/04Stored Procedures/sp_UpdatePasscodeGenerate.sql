IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdatePasscodeGenerate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdatePasscodeGenerate]
GO

/****** Object:  StoredProcedure [sp_UpdatePasscodeGenerate]    Script Date: 2013/8/27 下午 05:29:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/27 下午 05:29:36
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdatePasscodeGenerate]
	-- Access Object
    @access_object_id INT,
    @access_object_type INT,

	-- Data
	@generate_id int,
	@noToGenerate bigint,
	@generateCompleteCounter bigint,
	@insertErrorCounter bigint,
	@error_messgae nvarchar(max),
	@generate_status int,

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
    BEGIN TRAN sp_UpdatePasscodeGenerate
    
    -- Check Access Object
	--SELECT @accessObjectValid = 1, @accessObjectType = ao.type
   -- FROM v_accessObject ao
   -- WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
	SET @accessObjectValid = 1
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM passcode_generate
    		WHERE generate_id = @generate_id
            
            IF @recValid = 1
    		    BEGIN
                    
					DECLARE @updateSQL NVARCHAR(MAX)
					SET @updateSQL = N'UPDATE passcode_generate SET' 
                    
                    IF (@noToGenerate IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'noToGenerate = ' +  CAST(@noToGenerate AS varchar)
                    IF (@generateCompleteCounter IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'generateCompleteCounter = ' +  CAST(@generateCompleteCounter AS varchar)
                    IF (@insertErrorCounter IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'insertErrorCounter = ' +  CAST(@insertErrorCounter AS varchar)
                    IF (@error_messgae IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'error_messgae = ''' +   @error_messgae + ''''
                    IF (@generate_status IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'generate_status = ' +  CAST(@generate_status AS varchar)
                    
					SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'upd_date = GETDATE()'
					SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'upd_by_type = ' + CAST(@access_object_type AS varchar)
					SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'upd_by = ' + CAST(@access_object_id AS varchar)
                                        
                    SET @updateSQL = @updateSQL + ' WHERE generate_id = '+ CAST(@generate_id AS varchar)
                    
                    EXEC sp_executesql @updateSQL

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
    COMMIT TRAN sp_UpdatePasscodeGenerate
END
GO
    
    
