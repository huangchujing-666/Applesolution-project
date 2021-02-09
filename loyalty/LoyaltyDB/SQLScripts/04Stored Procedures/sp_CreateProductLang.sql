IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateProductLang]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateProductLang]
GO

/****** Object:  StoredProcedure [sp_CreateProductLang]    Script Date: 2013/10/11 下午 05:41:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/11 下午 05:41:43
-- Description:	Stored Procedure for CreateProduct_lang
-- Pending Task:
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateProductLang]
	-- Access Object
 @access_object_type INT, 
    @access_object_id INT,

	-- Data
	@product_id int,
	@lang_id int,
	@name nvarchar(100),
	@description nvarchar(max),
	@status int,
	
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
    
    -- Begin Transaction
    BEGIN TRAN sp_CreateProduct_lang
    
    -- Check Access Object
------	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
------    FROM v_accessObject ao
------    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted

SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN

            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                    
                    INSERT INTO product_lang
                    (
                        product_id, 
                        lang_id, 
                        name, 
                        description, 
                        status, 
                        crt_date, 
                        crt_by_type, 
                        crt_by, 
                        upd_date, 
                        upd_by_type, 
                        upd_by
                    )
                    VALUES (
                        @product_id,  --product_id
                        @lang_id,  --lang_id
                        @name,  --name
                        @description,  --description
                        @status,  --status
                        GETDATE(),  --crt_date
@access_object_type,
                        @access_object_id,  --crt_by
                        GETDATE(),  --upd_date
@access_object_type,
                        @access_object_id  --upd_by
                    )
                      
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
        
    -- Commit Transaction
    COMMIT TRAN sp_CreateProduct_lang
END
GO
