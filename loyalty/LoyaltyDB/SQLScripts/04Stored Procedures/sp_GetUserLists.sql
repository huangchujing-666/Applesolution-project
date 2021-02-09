IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetUserLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetUserLists]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetUserLists]    Script Date: 2013/7/2 ¤U¤È 06:18:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		LEO
-- Create date: 2013-06-03 15:00
-- Description:	Stored Procedure for Get User List
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUserLists]
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

	DECLARE @section_id INT
	DECLARE @action_by INT
	DECLARE @description NVARCHAR(MAX)
	DECLARE @rec_normal INT

	SET @section_id = 4	
	SET @action_by = 1
	SET @description = N'Get User List'
	
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

				IF (@searchParamName = 'login_id')
				BEGIN
					SET @whereSearchParams = @whereSearchParams + ' AND u.login_id LIKE ''%' + @searchParamValue + '%'''
					--SET @search_pin_value = @searchParamValue
				END
				ELSE IF (@searchParamName = 'name')
				BEGIN
					SET @whereSearchParams = @whereSearchParams + ' AND so.name LIKE ''%' + @searchParamValue + '%'''
					--SET @search_pin_value = @searchParamValue
				END
				ELSE IF (@searchParamName = 'email')
				BEGIN
					SET @whereSearchParams = @whereSearchParams + ' AND u.email LIKE ''%' + @searchParamValue + '%'''
					--SET @search_pin_value = @searchParamValue
				END
			END
		END
		-- [END] Searching Part (@whereSearchParams)

		SET @selectSQL = 
			'SELECT * FROM ( 
					SELECT CAST(COUNT(*) OVER() as BIGINT) as rowTotal, ROW_NUMBER() OVER (ORDER BY u.login_id) as row,

					u.user_id,
					u.login_id,
					so.name,
					u.email, 
					li.name as status,
					so.status as status_id,
					u.action_ip

					FROM user_profile u, system_object so, listing l, listing_item li
					WHERE
						u.user_id = so.object_id
						AND so.status = li.value
						AND l.code = ''Status''
						AND l.list_id = li.list_id
						AND u.record_status <> '+CAST(@recDeleted AS varchar(MAX)) + @whereSearchParams +' 
			) t WHERE row > ' + CAST(@rowIndexStart AS varchar(MAX)) +' and row <= ' + CAST((@rowIndexStart+ @rowLimit) AS varchar(MAX))

		--EXEC sp_executesql @selectSQL

		SELECT CAST(COUNT(*) OVER() as BIGINT) as rowTotal, ROW_NUMBER() OVER (ORDER BY u.login_id) as row,

					u.user_id,
					u.login_id,
					so.name,
					u.email, 
					li.name as status,
					so.status as status_id,
					u.action_ip

					FROM user_profile u, system_object so, listing l, listing_item li
					WHERE
						u.user_id = so.object_id
						AND so.status = li.value
						AND l.code = 'Status'
						AND l.list_id = li.list_id
						AND u.record_status <> @recDeleted 

		SET @sql_result = 1
	END
	ELSE
	BEGIN
		SET @sql_remark = 'AccessInvalid'
	END
END
GO