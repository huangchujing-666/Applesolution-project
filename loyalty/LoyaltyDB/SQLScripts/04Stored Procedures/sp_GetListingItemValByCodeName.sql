/****** Object:  StoredProcedure [dbo].[sp_GetListingItemValByCodeName]    Script Date: 2013/7/2 ¤U¤È 06:15:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-01-15
-- Description:	Stored Procedure for Get Listing Item Value 
--              by List Code and List Item Name 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetListingItemValByCodeName]
	-- Add the parameters for the stored procedure here
	@code VARCHAR(20),
	@name NVARCHAR(100),
	@value INT OUTPUT
AS
	DECLARE @listID INT
	DECLARE @statusActive INT
	
	SET @listID = 0
	SET @statusActive = 1
	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT @listID = list_id 
    FROM listing 
    WHERE code = @code AND [status] = @statusActive
    
    SELECT @value = value
    FROM listing_item 
    WHERE list_id = @listID AND [status] = @statusActive
    AND name = @name
    
END

GO


