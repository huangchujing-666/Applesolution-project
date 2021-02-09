IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'fn_buildUpdateSql') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION fn_buildUpdateSql
GO

/****** Object:  UserDefinedFunction [dbo].[fn_GetSettingValueByName]    Script Date: 2013/7/2 ¤U¤È 06:25:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Leo
-- Create date: 2013-01-16
-- Description:	Function for Get Setting Value 
--              by Name 
-- =============================================
CREATE FUNCTION [dbo].[fn_buildUpdateSql] 
(
	-- Add the parameters for the function here
	@updateSQL NVARCHAR(MAX)
	--@addUpdateParameter NVARCHAR(MAX)
)
RETURNS  NVARCHAR(MAX)
AS
BEGIN
	DECLARE @value NVARCHAR(MAX)listing
	DECLARE @substring VARCHAR(20)

	SET @substring = SUBSTRING(@updateSQL, LEN(@updateSQL)-4+1, 4)

	IF (@substring = ' SET')
		SET @value = @updateSQL + ' '  --SET @value = @updateSQL + ' ' + @addUpdateParameter + ' = @'+@addUpdateParameter
	ELSE
		SET @value = @updateSQL + ' , ' --SET @value = @updateSQL + ' , ' + @addUpdateParameter + ' = @'+@addUpdateParameter
	
	RETURN @value
END

GO


