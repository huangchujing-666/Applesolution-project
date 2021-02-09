/****** Object:  UserDefinedFunction [dbo].[fn_GetListingItemValByCodeName]    Script Date: 2013/7/2 ¤U¤È 06:25:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-01-15
-- Description:	Function for Get Listing Item Value 
--              by List Code and List Item Name 
-- =============================================
CREATE FUNCTION [dbo].[fn_GetListingItemValByCodeName] 
(
	-- Add the parameters for the function here
	@code VARCHAR(20),
	@name NVARCHAR(100)
)
RETURNS INT
AS
	
BEGIN
	-- Declare the return variable here
	DECLARE @listID INT
	DECLARE @statusActive INT
	DECLARE @value INT
	
	SET @listID = 0
	SET @statusActive = 1
	SET @value = 0
	
	-- Add the T-SQL statements to compute the return value here
    SELECT @listID = list_id 
    FROM listing 
    WHERE code = @code AND [status] = @statusActive
    
    SELECT @value = value
    FROM listing_item 
    WHERE list_id = @listID AND [status] = @statusActive
    AND name = @name

	-- Return the result of the function
	RETURN @value

END

GO


