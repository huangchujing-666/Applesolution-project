IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetExtJSSearchColumns]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetExtJSSearchColumns]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetExtJSSearchColumns]    Script Date: 2013/7/2 ¤U¤È 06:12:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-02-04
-- Description:	Stored Procedure for Get EXTJS
--              Search Columns by module name
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetExtJSSearchColumns]
	-- Access Object
	@access_object_id INT,

	-- Data
	@ip VARCHAR(50),
	@module VARCHAR(50),

	-- Output
	@status INT OUTPUT,
	@remark VARCHAR(100) OUTPUT
AS
    -- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')
	
	SET @status = 0
	SET @remark = ''

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		
		IF @module <> ''
			BEGIN
			
			SELECT 
				display_label [fieldLabel],
				search_tab_index [tabIndex],
				CAST(1 AS BIT) [allowBlank],
				field_name [name],
				data_type [type],
				data_source [datasource],
				null [display_value],
				'' [value],
				field_group [group],
				CAST(0 AS BIT) [autoFitErrors]
			FROM [table_structure]
			WHERE 
				[status] <> @recDeleted
			AND module = @module
			AND show_in_search = 1
			ORDER BY search_tab_index
			
			SET @status = 1
			
			END
		ELSE
			BEGIN
			SET @remark = 'ParameterInvalid'
			SELECT NULL [fieldLabel], NULL [tabIndex], NULL [allowBlank], NULL [name],
			NULL [type], NULL [datasource], NULL [display_value], NULL [value], NULL [group],
			NULL [autoFitErrors]
			END		
		END
	ELSE
		BEGIN
		SET @remark = 'UserInvalid'
		SELECT NULL [fieldLabel], NULL [tabIndex], NULL [allowBlank], NULL [name],
		NULL [type], NULL [datasource], NULL [display_value], NULL [value], NULL [group],
		NULL [autoFitErrors]
		END
		
	-- Pending Task: Add Action Log	
	
END

GO


