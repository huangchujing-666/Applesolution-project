IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetMemberAdvanceSearchListMember]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetMemberAdvanceSearchListMember]
GO

/****** Object:  StoredProcedure [sp_GetMemberAdvanceSearchListMember]    Script Date: 2013/7/17 上午 11:56:18 ******/
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
CREATE PROCEDURE [dbo].[sp_GetMemberAdvanceSearchListMember]
	-- Access Object
    @access_object_id INT,
    
	-- Paging
	@rowIndexStart bigint,
	@rowLimit int,

	-- Data
	@search_id int,

	-- Output
	@sql_result INT OUTPUT
AS
    -- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')

	
    SET @sql_result = 0
    
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
		
			DECLARE	@field_availablePoint INT SET @field_availablePoint = 1
			DECLARE	@field_birthdayMonth INT SET @field_birthdayMonth = 2
			DECLARE	@field_email INT SET @field_email = 3
			DECLARE	@field_gender INT SET @field_gender = 4

			DECLARE	@condition_is INT SET @condition_is = 1
			DECLARE	@condition_like INT SET @condition_like = 2
			DECLARE	@condition_less_than INT SET @condition_less_than = 3
			DECLARE	@condition_less_or_equal INT SET @condition_less_or_equal = 4
			DECLARE	@condition_equal INT SET @condition_equal = 5
			DECLARE	@condition_larger_or_equal INT SET @condition_larger_or_equal = 6
			DECLARE	@condition_larger_than INT SET @condition_larger_than = 7
    
			DECLARE @ruleTable TABLE (
							search_id int,
							group_id int,
							row_id int,
							target_field int,
							target_condition int,
							target_value nvarchar(500)
							)
			DECLARE @groupList TABLE (
							group_id int
							)

			DECLARE @resultTable TABLE (
				member_id int,
				member_no varchar(20),
				email varchar(100),
				firstname nvarchar(100),
				middlename nvarchar(100),
				lastname nvarchar(100)
			)

			DECLARE @process_previous_group_id int SET @process_previous_group_id = 0
			DECLARE @process_group_id int SET @process_group_id = 0
			DECLARE @process_group_row_counter int SET @process_group_row_counter = 0
			DECLARE @process_row_id int
			DECLARE @process_target_field int
			DECLARE @process_target_condition int
			DECLARE @process_target_condition_str nvarchar(20)
			DECLARE @process_target_value nvarchar(500)

			DECLARE @group_count int SET @group_count = 0
			DECLARE @row_count int SET @row_count = 0
			
			DECLARE @sql_group_where NVARCHAR(MAX) SET @sql_group_where = ''

			DECLARE @sql_select NVARCHAR(MAX)
			DECLARE @sql_where NVARCHAR(MAX)
			DECLARE @sql NVARCHAR(MAX)

			INSERT INTO 
				@ruleTable 
			SELECT
				search_id, group_id, row_id, target_field, target_condition, target_value
			FROM
				member_advance_search_rule r
			WHERE
			1 = 1
			AND r.[record_status] <> @recDeleted
			AND r.search_id = @search_id
			ORDER BY r.group_id

			INSERT INTO 
				@groupList 
			SELECT
				group_id
			FROM
				@ruleTable r
			GROUP BY r.group_id

			SET @sql_select = 'SELECT m.[member_id], m.[member_no], m.[email], m.[firstname], m.[middlename], m.[lastname]'
			SET @sql_where = 'WHERE 1 = 1
				AND m.[record_status] <> -1'
				
			SELECT @group_count = COUNT(*)
			FROM
				@groupList

			WHILE @group_count > 0
			BEGIN
				
				SELECT TOP 1 @process_group_id = group_id
				FROM
					@groupList

				IF @process_previous_group_id = 0
				BEGIN
					SET @sql_group_where = ' AND (('
					SET @process_group_row_counter = 1
				END
				ELSE IF @process_previous_group_id != @process_group_id
				BEGIN
					SET @sql_group_where = @sql_group_where + ' OR ('
					SET @process_group_row_counter = 1
				END
				ELSE
				BEGIN
					SET @process_group_row_counter = @process_group_row_counter + 1
				END

				SELECT TOP 1 @process_row_id = row_id, @process_target_field = target_field,
				@process_target_condition = target_condition, @process_target_value = target_value
				FROM
					@ruleTable
				WHERE 
					group_id = @process_group_id
				
				-- set compare condition
				IF @process_target_condition = @condition_is
				BEGIN
					SET @process_target_condition_str = ' = '
				END
				ELSE IF @process_target_condition = @condition_like
				BEGIN
					SET @process_target_condition_str = ' LIKE '
				END
				ELSE IF @process_target_condition = @condition_less_than
				BEGIN
					SET @process_target_condition_str = ' < '
				END
				ELSE IF @process_target_condition = @condition_less_or_equal
				BEGIN
					SET @process_target_condition_str = ' <= '
				END
				ELSE IF @process_target_condition = @condition_equal
				BEGIN
					SET @process_target_condition_str = ' = '
				END
				ELSE IF @process_target_condition = @condition_larger_or_equal
				BEGIN
					SET @process_target_condition_str = ' >= '
				END
				ELSE IF @process_target_condition = @condition_larger_than
				BEGIN
					SET @process_target_condition_str = ' > '
				END

				-- set where
				IF @process_group_row_counter > 1
				BEGIN
					SET @sql_group_where = @sql_group_where + ' AND '
				END

				IF @process_target_field = @field_availablePoint
				BEGIN
					
					SET @sql_group_where = @sql_group_where + 'm.available_point' + @process_target_condition_str + @process_target_value
				END
				ELSE IF @process_target_field = @field_birthdayMonth
				BEGIN
					
					SET @sql_group_where = @sql_group_where + 'm.birth_day IN (' + @process_target_value + ' )'
				END
				ELSE IF @process_target_field = @field_email
				BEGIN
					
					SET @sql_group_where = @sql_group_where + 'LOWER(m.email) LIKE LOWER(''%' + @process_target_value + '%'')'
				END
				ELSE IF @process_target_field = @field_gender
				BEGIN
					
					SET @sql_group_where = @sql_group_where + 'm.gender = ' + @process_target_value
				END
				ELSE
				BEGIN
					SET @sql_group_where = @sql_group_where + '1 = 1'
				END

				DELETE FROM @ruleTable WHERE group_id = @process_group_id AND row_id = @process_row_id

				SELECT @row_count = COUNT(*)
				FROM @ruleTable WHERE group_id = @process_group_id

				IF @row_count = 0 -- need to go to next group
				BEGIN
	
					SET @sql_where = @sql_where + @sql_group_where + ' )'
					SET @sql_group_where = ''

					DELETE FROM @groupList WHERE group_id = @process_group_id

					SELECT @group_count = COUNT(*)
					FROM
						@groupList


					IF @group_count = 0 -- Close parent dynamic group
					BEGIN
						SET @sql_where = @sql_where + ')'
					END
				END

				SET @process_previous_group_id = @process_group_id
			END -- END WHILE LOOP

			SET @sql = @sql_select + ' FROM [member_profile] m ' + @sql_where
			
			--SELECT @sql
			
			INSERT INTO  @resultTable
			EXEC sp_executesql @sql

			SELECT * FROM @resultTable

	        SET @sql_result = 100 --normal
		END
	ELSE
		BEGIN
			SET @sql_result = 1111 --no_permission
		END
END
GO