IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetPasscodeLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetPasscodeLists]
GO

/****** Object:  StoredProcedure [sp_GetPasscodeLists]    Script Date: 2013/8/12 下午 03:33:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/12 下午 03:33:34
-- Description:	Stored Procedure for Get Passcode List
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetPasscodeLists]
	-- Access Object
    @access_object_id INT,
    
	-- Data
	@passcode_prefix_id INT,

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
    
	DECLARE @selectSQL NVARCHAR(MAX)
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = so.type
    FROM system_object so
    WHERE object_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		    
			-- Check Valid Prefix
			IF @passcode_prefix_id = 0
				SET @recValid = 1
			ELSE
			BEGIN
				SELECT @recValid = 1
    			FROM passcode_prefix
    			WHERE prefix_id = @passcode_prefix_id AND record_status <> @recDeleted
			END
            
			DECLARE @whereSearchParams NVARCHAR(MAX) SET @whereSearchParams = N''
            IF @recValid = 1
    		    BEGIN
					
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

							IF (@searchParamName = 'pin_value')
							BEGIN
								SET @whereSearchParams = @whereSearchParams + ' AND p.pin_value LIKE ''%' + @searchParamValue + '%'''
								--SET @search_pin_value = @searchParamValue
							END

							ELSE IF (@searchParamName = 'active_date')
							BEGIN
								SET @whereSearchParams = @whereSearchParams + ' AND p.active_date = ''' + @searchParamValue + ''''
								--SET @search_active_date = @searchParamValue
							END

							ELSE IF (@searchParamName = 'expiry_date')
							BEGIN
								SET @whereSearchParams = @whereSearchParams + ' AND p.expiry_date = ''' + @searchParamValue + ''''
								--SET @search_expiry_date = @searchParamValue
							END
							ELSE IF (@searchParamName = 'point')
							BEGIN
								SET @whereSearchParams = @whereSearchParams + ' AND p.point = ' + @searchParamValue
								--SET @search_point = @searchParamValue
							END
							ELSE IF (@searchParamName = 'member_no')
							BEGIN
								SET @whereSearchParams = @whereSearchParams + ' AND m.member_no = ''' + @searchParamValue + ''''
								--SET @search_point = @searchParamValue
							END
							ELSE IF (@searchParamName = 'point_range_lower')
							BEGIN
								SET @whereSearchParams = @whereSearchParams + ' AND p.point >= ' + @searchParamValue
								--SET @search_point = @searchParamValue
							END
							ELSE IF (@searchParamName = 'point_range_upper')
							BEGIN
								SET @whereSearchParams = @whereSearchParams + ' AND p.point <= ''' + @searchParamValue + ''''
								--SET @search_point = @searchParamValue
							END
						END
					END

					IF @passcode_prefix_id>0
					BEGIN
						SET @whereSearchParams = @whereSearchParams + ' AND p.passcode_prefix_id = ' + CAST(@passcode_prefix_id AS varchar) + ''
					END

					SET @selectSQL = 
						'SELECT * FROM ( 
							 SELECT CAST(COUNT(*) OVER() as BIGINT) as rowTotal, ROW_NUMBER() OVER (ORDER BY p.passcode_id) as row,
   
								p.passcode_id AS [passcode_id],
								p.pin_value AS [pin_value],
								p.generate_id AS [generate_id], 
								p.passcode_prefix_id AS [passcode_prefix_id],
								p.active_date AS [active_date],
								p.expiry_date AS [expiry_date],
							
								pt.name AS [product_name],
								p.registered AS [registered],
								p.point AS [point],
								m.member_id AS [member_id],
								m.member_no AS [member_no],
								li.name AS [registered_name],
								p.void_date AS [void_date],
								p.void_reason AS [void_reason],
								p.status AS [status],
								p.crt_date AS [crt_date],
								p.upd_date AS [upd_date], 
								p.crt_by AS [crt_by],
								p.upd_by AS [upd_by],
								p.record_status AS [record_status]
							 FROM  passcode p
							 LEFT OUTER JOIN transaction_earn te
								ON te.earn_source_id = p.passcode_id
							 LEFT OUTER JOIN member_profile m
								ON m.member_id = te.member_id
							INNER JOIN product pt
								ON p.product_id = pt.product_id
							INNER JOIN listing_item li
								ON p.registered = li.value
							INNER JOIN listing l
								ON l.list_id = li.list_id
							
							 WHERE 1=1
								AND l.code = ''YesNo''
								AND p.[record_status] <> ' + CAST(@recDeleted AS varchar) + @whereSearchParams +' 
						) t WHERE row > ' + CAST(@rowIndexStart AS varchar(MAX)) +' and row <= ' + CAST((@rowIndexStart+ @rowLimit) AS varchar(MAX))
               
					--EXEC sp_executesql @selectSQL


					SELECT CAST(COUNT(*) OVER() as BIGINT) as rowTotal, ROW_NUMBER() OVER (ORDER BY p.passcode_id) as row,
   
								p.passcode_id AS [passcode_id],
								p.pin_value AS [pin_value],
								p.generate_id AS [generate_id], 
								p.passcode_prefix_id AS [passcode_prefix_id],
								p.active_date AS [active_date],
								p.expiry_date AS [expiry_date],
							
								so.name AS [product_name],
								p.registered AS [registered],
								p.point AS [point],
								m.member_id AS [member_id],
								m.member_no AS [member_no],
								li.name AS [registered_name],
								p.void_date AS [void_date],
								p.void_reason AS [void_reason],
								p.status AS [status],
								p.crt_date AS [crt_date],
								p.upd_date AS [upd_date], 
								p.crt_by AS [crt_by],
								p.upd_by AS [upd_by],
								p.record_status AS [record_status]
							 FROM  passcode p
							 LEFT OUTER JOIN [transaction] t
								ON t.source_id = p.passcode_id
							 LEFT OUTER JOIN transaction_earn te
								ON te.transaction_id = t.transaction_id
							 LEFT OUTER JOIN member_profile m
								ON m.member_id = t.member_id
							INNER JOIN product pt
								ON p.product_id = pt.product_id
							INNER JOIN listing_item li
								ON p.registered = li.value
							INNER JOIN listing l
								ON l.list_id = li.list_id
							INNER JOIN system_object so
								ON so.object_id = p.product_id
							 WHERE 1=1
								AND l.code = 'YesNo'
								AND p.[record_status] <> @recDeleted 

					--EXEC sp_executesql @selectSQL, 
					--N' @passcode_prefix_id INT, @recDeleted INT, @rowIndexStart BIGINT, @rowLimit INT',
					--@passcode_prefix_id, @recDeleted, @rowIndexStart, @rowIndexStart, @rowLimit
                    
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