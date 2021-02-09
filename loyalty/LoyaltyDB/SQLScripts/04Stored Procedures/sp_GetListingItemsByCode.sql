IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetListingItemsByCode]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetListingItemsByCode]
GO

/****** Object:  StoredProcedure [sp_GetListingItemsByCode]    Script Date: 2013/7/18 下午 04:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/18 下午 04:03:33
-- Description:	Stored Procedure for Get Listing Items By Code
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetListingItemsByCode]
	-- Access Object
    @access_object_id INT,

	-- Data
    @code VARCHAR(40),

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
    
	DECLARE @listID INT SET @listID = 0
    
    SET @sql_result = 0
	SET @sql_remark = ''
    
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
		    
            SELECT @recValid = 1, @listID = list_id 
    		FROM [listing]
    		WHERE [status] = @statusActive AND UPPER(code) = UPPER(@code)
		
            IF @recValid = 1
    		    BEGIN
    			    
                    SELECT [value] 'id', [name] 'value'
					FROM [listing_item] 
					WHERE [list_id] = @listID AND [status] = @statusActive
					ORDER BY [display_order]
                    
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
     
END
GO