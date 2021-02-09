IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetProductDetailBy]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetProductDetailBy]
GO

/****** Object:  StoredProcedure [sp_GetProductDetailBy]    Script Date: 2013/7/29 下午 04:29:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/29 下午 04:29:43
-- Description:	Stored Procedure for Get Product Detail By Product ID or NO
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetProductDetailBy]
	-- Access Object
    @access_object_id INT,
	
	-- Data
	@product_id INT,
	@product_no VARCHAR(20),

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

	--SELECT @recValid = 1
   -- 		FROM product
   -- 		WHERE record_status <> @recDeleted
			--AND (product_no = @product_pk) OR (product_id = CONVERT(INT, @product_pk))   

			IF (@product_no IS NOT NULL)
			BEGIN
				SELECT @recValid = 1
    			FROM product
    			WHERE record_status <> @recDeleted
				AND product_no = @product_no
			END
			ELSE IF (@product_id IS NOT NULL)
			BEGIN
				SELECT @recValid = 1
    			FROM product
    			WHERE record_status <> @recDeleted
				AND product_id = @product_id
			END

            IF @recValid = 1
    		    BEGIN
                    SELECT 
						p.product_id,
						p.product_no,
						
						p.price,
						p.point, 
						p.consumption_period,
						p.lost_customer_period,
						
						li.name 'status',
						p.status 'status_id',
						p.crt_date, 
						p.upd_date,
						p.crt_by, 
						p.upd_by
					FROM product p, listing l, listing_item li
					WHERE 
						p.product_no = ISNULL(@product_no, p.product_no)  
						AND p.product_id = ISNULL(@product_id, p.product_id)
					--(p.product_no = @product_pk OR p.product_id = CONVERT(INT, @product_pk))
					--(p.product_id = @product_id OR p.product_no = @product_no)

						AND p.[status] = li.[value]
						AND l.[list_id] = li.[list_id]
						AND l.code = 'Status'
						AND p.[record_status] <> @recDeleted

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