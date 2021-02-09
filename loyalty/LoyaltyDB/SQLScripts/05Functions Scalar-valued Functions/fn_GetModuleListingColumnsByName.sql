/****** Object:  UserDefinedFunction [dbo].[fn_GetModuleListingColumnsByName]    Script Date: 2013/7/2 ¤U¤È 06:25:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-02-05
-- Description:	Function for Get Module Listing 
--              Select Columns by Name 
-- =============================================
CREATE FUNCTION [dbo].[fn_GetModuleListingColumnsByName] 
(
	-- Add the parameters for the function here
	@name VARCHAR(50)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @sql NVARCHAR(MAX)
	DECLARE @selectTable TABLE (
								table_id INT PRIMARY KEY CLUSTERED,
								table_name VARCHAR(100),
								field_name NVARCHAR(MAX),
								json_name NVARCHAR(100)
								)
	DECLARE @statusActive INT
	DECLARE @tableCount INT
	
	SET @sql = ''
	SET @tableCount = 0
	SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	
	-- Add the T-SQL statements to compute the return value here
	INSERT INTO @selectTable (table_id, table_name, field_name, json_name)
	SELECT table_id, table_name, field_name, json_name
	FROM table_structure
	WHERE [status] = @statusActive
	AND module = @name
	ORDER BY field_tab_index
    
    -- Loop Listing Fields
    SELECT @tableCount = COUNT(1) FROM @selectTable
    WHILE @tableCount > 0
		BEGIN
		
		DECLARE @select_table_id INT
		DECLARE @select_table_name VARCHAR(100)
		DECLARE @select_field_name NVARCHAR(MAX)
		DECLARE @select_json_name NVARCHAR(100)
		
		SELECT TOP 1 
		@select_table_id = table_id, @select_table_name = table_name,
		@select_field_name = field_name, @select_json_name = json_name
		FROM @selectTable
		
		DELETE FROM @selectTable WHERE table_id = @select_table_id
	    SELECT @tableCount = COUNT(1) FROM @selectTable
		
		IF @select_table_name <> ''	
			SET @sql = @sql + CAST(@select_table_name AS NVARCHAR) + '.'
			
		IF @select_field_name <> ''
			SET @sql = @sql + @select_field_name + ' '
		
		IF @select_json_name <> ''
			SET @sql = @sql + '[' + @select_json_name + ']'
		
		IF @tableCount > 0
			SET @sql = @sql + ', '
		
		END
    
	-- Return the result of the function
	RETURN @sql

END

GO


