IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetMemberLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetMemberLists]
GO

/****** Object:  StoredProcedure [sp_GetMemberLists]    Script Date: 2013/7/17 上午 11:56:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/17 上午 11:56:18
-- Description:	Stored Procedure for Get Member Lists
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetMemberLists]
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
		
			DECLARE @selectSQL NVARCHAR(MAX)

			-- Searching Part
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

					IF (@searchParamName = 'member_no')
					BEGIN
						SET @whereSearchParams = @whereSearchParams + ' AND member_no LIKE ''%' + @searchParamValue + '%'''
					END
					ELSE IF (@searchParamName = 'email')
					BEGIN
						SET @whereSearchParams = @whereSearchParams + ' AND email LIKE ''%' + @searchParamValue + '%'''
					END
					ELSE IF (@searchParamName = 'mobile_no')
					BEGIN
						SET @whereSearchParams = @whereSearchParams + ' AND mobile_no LIKE ''%' + @searchParamValue + '%'''
					END
					ELSE IF (@searchParamName = 'hkid')
					BEGIN
						SET @whereSearchParams = @whereSearchParams + ' AND hkid LIKE ''%' + @searchParamValue + '%'''
					END
					ELSE IF (@searchParamName = 'name')
					BEGIN
						SET @whereSearchParams = @whereSearchParams + ' AND name LIKE ''%' + @searchParamValue + '%'''
					END
				END
			END

			SET @selectSQL = 
			'SELECT * FROM ( 
					SELECT CAST(COUNT(*) OVER() as BIGINT) as rowTotal, ROW_NUMBER() OVER (ORDER BY member_no) as row,

						[member_id], [member_no], [email], [mobile_no], [hkid], name, firstname, lastname 

					FROM
						[member_profile]
					WHERE [record_status] <> '+CAST(@recDeleted AS varchar(MAX)) + @whereSearchParams +' 
			) t WHERE row > ' + CAST(@rowIndexStart AS varchar(MAX)) +' and row <= ' + CAST((@rowIndexStart+ @rowLimit) AS varchar(MAX))

			--EXEC sp_executesql @selectSQL

			SELECT CAST(COUNT(*) OVER() as BIGINT) as rowTotal, ROW_NUMBER() OVER (ORDER BY member_no) as row,

						[member_id], [member_no], [email], [mobile_no], [hkid], so.name

					FROM
						[member_profile] m, [system_object] so
					WHERE 
						so.object_id = m.member_id
					AND m.[record_status] <> @recDeleted

	        SET @sql_result = 1
		END
	ELSE
		BEGIN
		SET @sql_remark = 'AccessObjectInvalid'
		
		END
     
END
GO