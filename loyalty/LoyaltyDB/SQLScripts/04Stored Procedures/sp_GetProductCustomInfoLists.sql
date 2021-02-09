IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetProductCustomInfoLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetProductCustomInfoLists]
GO

/****** Object:  StoredProcedure [sp_GetProductCustomInfoLists]    Script Date: 2013/8/28 下午 02:46:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/28 下午 02:46:43
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetProductCustomInfoLists]
    @user_id INT,
    @product_id INT,

    @sql_result INT OUTPUT,
    @sql_remark VARCHAR(100) OUTPUT
AS
    DECLARE @module VARCHAR(50) SET @module = 'product_custom_info'
	DECLARE @userValid INT SET @userValid = 0

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
    
    -- Insert statements for procedure here
 --   SELECT @userValid = 1 
 --   FROM user_profile
 --   WHERE user_id = @user_id AND [status] = @statusActive AND record_status <> @recDeleted
    
 --   IF @userValid = 1
	--	BEGIN
		    
 --           SELECT TOP 1 @recValid = 1
 --   		FROM custom_field
 --   		WHERE module = @module AND record_status <> @recDeleted
            
 --           IF @recValid = 1
 --   		    BEGIN
                    
 --                   SELECT 
 --                       p.info_id, 
 --                       p.custom_field_id, 
 --                       p.field_value, 
 --                       cf.field_name
 --                   FROM  product_custom_info p, custom_field cf
 --                   WHERE p.custom_field_id = cf.field_id
	--				AND p.product_id = @product_id
 --                   AND p.[record_status] <> @recDeleted
	--				AND cf.[record_status] <> @recDeleted
                    
 --                   SET @sql_result = 1
 --               END
	--    	ELSE
 --   		    BEGIN
 --       			SET @sql_remark = 'RecordInvalid'
 --   		    END
 --   	END
	--ELSE
	--	BEGIN
	--	    SET @sql_remark = 'UserInvalid'
	--	END
END
GO
    
    
