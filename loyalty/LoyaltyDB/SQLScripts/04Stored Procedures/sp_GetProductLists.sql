IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetProductLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetProductLists]
GO

/****** Object:  StoredProcedure [sp_GetProductLists]    Script Date: 2013/7/29 上午 01:00:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/29 上午 01:00:21
-- Description:	Stored Procedure for Get Product Lists
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetProductLists]
	-- Access Object
    @access_object_id INT,
    
	-- Paging
	@rowIndexStart bigint,
	@rowLimit int,

	-- Searching Params
	@searchParams NVARCHAR(MAX),

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
		    
            SET @recValid = 1
    	
            IF @recValid = 1
			    BEGIN
					DECLARE @selectSQL NVARCHAR(MAX)

					-- [START] Searching Part (@whereSearchParams)
					DECLARE @whereSearchParams NVARCHAR(MAX) SET @whereSearchParams = N''

					IF (len(@searchParams)>0)
					BEGIN
						DECLARE @extractJson TABLE (
								element_id int,
								parent_id int,
								object_id int,
								name NVARCHAR(MAX),
								stringvalue NVARCHAR(MAX),
								valuetype NVARCHAR(MAX)
								)
						DECLARE @main_object_id INT
						DECLARE @searchParamsCount INT
						DECLARE @searchParamName NVARCHAR(100)
						DECLARE @searchParamValue NVARCHAR(MAX)
							
						DECLARE @search_passcode_id NVARCHAR(MAX) SET @search_passcode_id = NULL
						DECLARE @search_active_date VARCHAR(10) SET @search_active_date = NULL
						DECLARE @search_expiry_date VARCHAR(10) SET @search_expiry_date = NULL
						DECLARE @search_point float SET @search_point = NULL

						INSERT INTO @extractJson
							SELECT * FROM parseJSON(@searchParams)

						SELECT @main_object_id = object_id from @extractJson where parent_id IS NULL
						SELECT @searchParamsCount = COUNT(*) from @extractJson where parent_id = @main_object_id

						DECLARE @loopSearchParamsCounter INT SET @loopSearchParamsCounter = 0
						WHILE @loopSearchParamsCounter < @searchParamsCount
						BEGIN
							SET @loopSearchParamsCounter = @loopSearchParamsCounter + 1

							SELECT @searchParamName = stringValue from @extractJson where parent_id = @loopSearchParamsCounter AND name ='property'
							SELECT @searchParamValue = stringValue from @extractJson where parent_id = @loopSearchParamsCounter AND name ='value'

							IF (@searchParamName = 'product_no')
							BEGIN
								SET @whereSearchParams = @whereSearchParams + ' AND p.product_no LIKE ''%' + @searchParamValue + '%'''
								--SET @search_pin_value = @searchParamValue
							END
							ELSE IF (@searchParamName = 'name')
							BEGIN
								SET @whereSearchParams = @whereSearchParams + ' AND pl.name LIKE ''%' + @searchParamValue + '%'''
								--SET @search_pin_value = @searchParamValue
							END
						END
					END
					-- [END] Searching Part (@whereSearchParams)

					SET @selectSQL = 
					'SELECT * FROM ( 
							SELECT CAST(COUNT(*) OVER() as BIGINT) as rowTotal, ROW_NUMBER() OVER (ORDER BY p.product_no) as row,
                    
								p.product_id,
								p.product_no,
								
								so.name, 
								
								p.price, 
								p.point, 
								p.consumption_period, 
								p.lost_customer_period, 
						
								
								li.name ''status'', 
								p.status ''status_id'', 
								p.crt_date, 
								p.upd_date, 
								p.crt_by, 
								p.upd_by,

								pp.file_name,
								pp.file_extension
							FROM product p
							INNER JOIN listing_item li
								ON p.[status] = li.[value]
							INNER JOIN listing l
								ON l.[list_id] = li.[list_id]
							INNER JOIN system_object so
								ON so.object_id = p.product_id

							LEFT OUTER JOIN (SELECT * FROM product_photo WHERE record_status <> '+ CAST(@recDeleted AS varchar(MAX))+') pp
								ON p.product_id = pp.product_id
							WHERE
								l.code = ''Status''
								
								AND p.[record_status] <> '+CAST(@recDeleted AS varchar(MAX)) + @whereSearchParams +' 
						) t WHERE row > ' + CAST(@rowIndexStart AS varchar(MAX)) +' and row <= ' + CAST((@rowIndexStart+ @rowLimit) AS varchar(MAX))

					--EXEC sp_executesql @selectSQL

					SELECT CAST(COUNT(*) OVER() as BIGINT) as rowTotal, ROW_NUMBER() OVER (ORDER BY p.product_no) as row,
                    
								p.product_id,
								p.product_no,
								
								so.name, 
								
								p.price, 
								p.point, 
								p.consumption_period, 
								p.lost_customer_period, 
						
								
								li.name 'status', 
								p.status 'status_id', 
								p.crt_date, 
								p.upd_date, 
								p.crt_by, 
								p.upd_by,

								pp.file_name,
								pp.file_extension
							FROM product p
							INNER JOIN listing_item li
								ON p.[status] = li.[value]
							INNER JOIN listing l
								ON l.[list_id] = li.[list_id]
							INNER JOIN system_object so
								ON so.object_id = p.product_id
							LEFT OUTER JOIN (SELECT * FROM product_photo WHERE record_status <> @recDeleted ) pp
								ON p.product_id = pp.product_id
							WHERE
								l.code = 'Status'
								
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