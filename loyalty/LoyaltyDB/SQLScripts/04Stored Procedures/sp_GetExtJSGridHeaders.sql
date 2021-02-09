/****** Object:  UserDefinedFunction [dbo].[sp_GetExtJSGridHeaders]    Script Date: 2013/7/2 ¤U¤È 06:25:17 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetExtJSGridHeaders]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetExtJSGridHeaders]
GO


/****** Object:  StoredProcedure [dbo].[sp_GetExtJSGridHeaders]    Script Date: 2013/7/2 ¤U¤È 06:12:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-02-05
-- Description:	Stored Procedure for Get EXTJS
--              Grid Headers by module name
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetExtJSGridHeaders]
	-- Access Object
	@access_object_id INT,

	-- Data
	@module VARCHAR(50),

	-- Output
	@status INT OUTPUT,
	@remark VARCHAR(100) OUTPUT,
	@title NVARCHAR(100) OUTPUT, 
	@link VARCHAR(100) OUTPUT,
	@pageSize INT OUTPUT,
	@searchTextHidden BIT OUTPUT,
	@addHidden BIT OUTPUT,
	@deleteHidden BIT OUTPUT,
	@exportHidden BIT OUTPUT,
	@checkboxHidden BIT OUTPUT
AS
    -- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')

	DECLARE @moduleValid INT
	DECLARE @moduleName NVARCHAR(100)
	DECLARE @settingValue NVARCHAR(MAX)

	DECLARE @local_grid_page_size INT
	DECLARE @local_grid_search_hidden INT
	DECLARE @local_grid_add_hidden INT
	DECLARE @local_grid_delete_hidden INT
	DECLARE @local_grid_export_hidden INT
	DECLARE @local_grid_checkbox_hidden INT
	
	SET @moduleValid = 0
	
	SET @status = 0
	SET @remark = ''
	SET @moduleName = ''
	SET @title = ''
	
	-- Get Grid Header Default Setting Value
	SET @pageSize = CONVERT(INT, [dbo].fn_GetSettingValueByName('default_listdata_page_size'))
	SET @searchTextHidden = CONVERT(BIT, [dbo].fn_GetSettingValueByName('default_grid_search_hidden'))
	SET @addHidden = CONVERT(BIT, [dbo].fn_GetSettingValueByName('default_grid_add_hidden'))
	SET @deleteHidden = CONVERT(BIT, [dbo].fn_GetSettingValueByName('default_grid_delete_hidden'))
	SET @exportHidden = CONVERT(BIT, [dbo].fn_GetSettingValueByName('default_grid_export_hidden'))
	SET @checkboxHidden = CONVERT(BIT, [dbo].fn_GetSettingValueByName('default_grid_checkbox_hidden'))

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
				@moduleValid = 1, 
				@moduleName = name,
				@link = link,
				@local_grid_page_size = grid_page_size,
				@local_grid_search_hidden = grid_search_hidden,
				@local_grid_add_hidden = grid_add_hidden,
				@local_grid_delete_hidden = grid_delete_hidden,
				@local_grid_export_hidden = grid_export_hidden,
				@local_grid_checkbox_hidden = grid_checkbox_hidden
			FROM section 
			WHERE module = @module 
			AND [status] = @statusActive
			
			IF @moduleValid = 1
				BEGIN
				
				-- Set Grid Headers Setting Value
				SET @title = @moduleName
				
				if (@local_grid_page_size = -1)
				BEGIN
					SET @settingValue = [dbo].fn_GetSettingValueByName(@module + '_listdata_page_size')
					IF @settingValue IS NOT NULL			
						SET @pageSize = CONVERT(INT, @settingValue)
				END
				ELSE
				BEGIN
					SET @pageSize = @local_grid_page_size
				END
				
				if (@local_grid_search_hidden = -1)
				BEGIN
					SET @settingValue = [dbo].fn_GetSettingValueByName(@module + '_grid_search_hidden')
					IF @settingValue IS NOT NULL			
						SET @searchTextHidden = CONVERT(BIT, @settingValue)
				END
				ELSE
				BEGIN
					SET @searchTextHidden = CONVERT(BIT, @local_grid_search_hidden)
				END
					
				if (@local_grid_add_hidden = -1)
				BEGIN
					SET @settingValue = [dbo].fn_GetSettingValueByName(@module + '_grid_add_hidden')
					IF @settingValue IS NOT NULL
						SET @addHidden = CONVERT(BIT, @settingValue)
				END
				ELSE
				BEGIN
					SET @addHidden = CONVERT(BIT, @local_grid_add_hidden)
				END
				
				if (@local_grid_delete_hidden = -1)
				BEGIN
					SET @settingValue = [dbo].fn_GetSettingValueByName(@module + '_grid_delete_hidden')
					IF @settingValue IS NOT NULL
						SET @deleteHidden = CONVERT(BIT, @settingValue)
				END
				ELSE
				BEGIN
					SET @deleteHidden = CONVERT(BIT, @local_grid_delete_hidden)
				END

				IF (@local_grid_export_hidden = -1)
				BEGIN
					SET @settingValue = [dbo].fn_GetSettingValueByName(@module + '_grid_export_hidden')
					IF @settingValue IS NOT NULL
						SET @exportHidden = CONVERT(BIT, @settingValue)
				END
				ELSE
				BEGIN
					SET @exportHidden = CONVERT(BIT, @local_grid_export_hidden)
				END
				
				if (@local_grid_checkbox_hidden = -1)
				BEGIN
					SET @settingValue = [dbo].fn_GetSettingValueByName(@module + '_grid_checkbox_hidden')
					IF @settingValue IS NOT NULL
						SET @checkboxHidden = CONVERT(BIT, @settingValue)
				END
				ELSE
				BEGIN
					SET @checkboxHidden = CONVERT(BIT, @local_grid_checkbox_hidden)
				END
							
				-- Get List Data Fields
				SELECT 
				display_label [header],
				json_name [dataIndex],
				grid_width [width],
				grid_type [type],
				NULL [renderer],
				CAST(show_in_grid AS BIT) [sortable],
				show_in_grid [column]
				FROM [table_structure]
				WHERE [status] <> @recDeleted
				AND module = @module
				ORDER BY grid_tab_index, field_tab_index
				
				SET @status = 1
				
				END
			ELSE
				BEGIN
				
				SET @remark = 'ModuleInvalid'
				SELECT NULL [header], NULL [name], NULL [width], NULL [type], NULL [renderer], NULL [sortable]		
				END
			END
		ELSE
			BEGIN
			SET @remark = 'ParameterInvalid'
			SELECT NULL [header], NULL [name], NULL [width], NULL [type], NULL [renderer], NULL [sortable]
			END		
		END
	ELSE
		BEGIN
		SET @remark = 'UserInvalid'
		SELECT NULL [header], NULL [name], NULL [width], NULL [type], NULL [renderer], NULL [sortable]
		END
		
	-- Pending Task: Add Action Log	
	
END

GO