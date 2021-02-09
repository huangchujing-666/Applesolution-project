IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetGiftLangDetailBy]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetGiftLangDetailBy]
GO

/****** Object:  StoredProcedure [sp_GetGiftLangDetailBy]    Script Date: 2013/10/15 下午 04:19:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/15 下午 04:19:28
-- Description:	Stored Procedure for Get Gift Lang Detail By Gift ID
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetGiftLangDetailBy]
	-- Access Object
    @access_object_id INT,

	-- Data
	@gift_id INT,

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

            SELECT @recValid = 1
    		FROM gift_lang
    		WHERE gift_id = @gift_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                    SELECT 
                        g.gift_lang_id,
                        g.gift_id,
                        g.lang_id,
                        g.name,
                        g.description
                    FROM  gift_lang g
                    WHERE gift_id = @gift_id
                    AND g.[record_status] <> @recDeleted
                    
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