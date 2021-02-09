/****** Object:  UserDefinedFunction [dbo].[fn_GetSettingValueByName]    Script Date: 2013/7/2 ¤U¤È 06:25:17 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_GetSettingValueByName]'))
	DROP FUNCTION [dbo].[fn_GetSettingValueByName]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-01-16
-- Description:	Function for Get Setting Value 
--              by Name 
-- =============================================
CREATE FUNCTION [dbo].[fn_GetSettingValueByName] 
(
	-- Add the parameters for the function here
	@name VARCHAR(40)
)
RETURNS NVARCHAR(500)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @value NVARCHAR(MAX)
	DECLARE @statusActive INT
	
	SET @value = NULL
	SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	
	-- Add the T-SQL statements to compute the return value here
    SELECT @value = value 
    FROM system_config 
    WHERE name = @name AND [record_status] = 0
    
	-- Return the result of the function
	RETURN @value

END

GO