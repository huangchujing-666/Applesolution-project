/****** Object:  UserDefinedFunction [dbo].[fn_GetListingIDByCode]    Script Date: 2013/7/2 ¤U¤È 06:24:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-01-16
-- Description:	Function for Get Listing ID 
--              by List Code 
-- =============================================
CREATE FUNCTION [dbo].[fn_GetListingIDByCode] 
(
	-- Add the parameters for the function here
	@code VARCHAR(20)
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @listID INT
	DECLARE @statusActive INT
	
	SET @listID = 0
	SET @statusActive = 1
	
	-- Add the T-SQL statements to compute the return value here
    SELECT @listID = list_id 
    FROM listing 
    WHERE code = @code AND [status] = @statusActive
    
	-- Return the result of the function
	RETURN @listID

END

GO


