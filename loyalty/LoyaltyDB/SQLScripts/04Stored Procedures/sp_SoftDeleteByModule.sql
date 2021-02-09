IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_SoftDeleteByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_SoftDeleteByModule]
GO


/****** Object:  StoredProcedure [dbo].[sp_SoftDeleteByModule]    Script Date: 2013/7/2 ¤U¤È 06:10:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Leo   
-- Create date: 2013-06-03
-- Description:	Stored Procedure for Delete Item By Module
-- =============================================
CREATE PROCEDURE [dbo].[sp_SoftDeleteByModule]
	-- Access Object
	@access_object_id INT,
    @access_object_type INT,

	-- Data
	@module nvarchar(100),
	@rec_id INT,

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
	
	DECLARE @tableName NVARCHAR(128)
	DECLARE @tableKeyName NVARCHAR(128)
	
	DECLARE @CheckRecordValidSQL NVARCHAR(max)
	DECLARE @SoftDeleteSQL NVARCHAR(max)

	SET @sql_result = 0
	SET @sql_remark = ''
	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	-- Begin Transaction
	BEGIN TRAN sp_SoftDeleteByModule

	-- Check Access Object
--	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
  --  FROM v_accessObject ao
 --   WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    -- get tableName and tableKeyName
    SELECT @tableName=table_name
    FROM table_structure
    WHERE LOWER(module)=LOWER(@module)

	SELECT TOP 1 @tableKeyName=COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME=@tableName
	
	SET @accessObjectValid = 1
    IF @accessObjectValid = 1
		BEGIN

		-- Check delete record valid or not
		SET @CheckRecordValidSQL='
			SELECT @recValid = 1
			FROM ' + @tableName + '
			WHERE ' + @tableKeyName + ' = @rec_id AND [record_status] <> @recDeleted'
		
		EXEC sp_executesql @CheckRecordValidSQL,
		N'@recDeleted INT,@rec_id INT,@recValid INT OUTPUT',
		@recDeleted, @rec_id, @recValid OUTPUT
		
	
		IF @recValid = 1
			BEGIN

			 --Perform Soft delete
			SET @SoftDeleteSQL =
			'UPDATE '+@tableName+' 
			SET [record_status] = @recDeleted, upd_date = GETDATE(), upd_by = @access_object_id, upd_by_type = @access_object_type 
			WHERE ' + @tableKeyName + ' = @rec_id AND [record_status] <> @recDeleted'
			
			EXEC sp_executesql @SoftDeleteSQL,
			N'@recDeleted INT, @access_object_id INT, @access_object_type INT, @rec_id INT',
			@recDeleted, @access_object_id, @access_object_type, @rec_id

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
		
	COMMIT TRAN sp_SoftDeleteByModule

END

GO