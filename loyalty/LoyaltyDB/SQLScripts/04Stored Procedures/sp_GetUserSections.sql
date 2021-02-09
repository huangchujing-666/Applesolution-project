IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetUserSections]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetUserSections]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetUserSections]    Script Date: 2013/7/2 ¤U¤È 06:19:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-01-15 15:35
-- Description:	Stored Procedure for Get User Available Section List
-- Pending Task: Add Action Log, User Privilege
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUserSections] 
	-- Access Object
	@access_object_id INT,

	-- Data
	@ip VARCHAR(50),
	@parent_id INT,
	@lang_id INT,

	-- Output
	@status INT OUTPUT,
	@remark VARCHAR(100) OUTPUT,
	@total INT OUTPUT
AS
	-- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')

	DECLARE @privilegeTypeRole INT
	DECLARE @readYes INT
	DECLARE @displayYes INT
	DECLARE @menuIconClassID INT
	
	DECLARE @AvailableSections AS TABLE(
										tmp_section_id INT PRIMARY KEY CLUSTERED,
										tmp_read_status INT
										)
	
	SET @privilegeTypeRole = 1
	SET @readYes = 1
	SET @displayYes = 1
	SET @menuIconClassID = [dbo].fn_GetListingIDByCode('MenuIconClass')
	
	SET @status = 0
	SET @remark = ''
	SET @total = 0

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
		SET @status = 1
		
		-- Insert User Sections Into Temp Table Variable
		INSERT INTO @AvailableSections (tmp_section_id, tmp_read_status)		
		SELECT distinct section_id, read_status
		FROM privilege p
		INNER JOIN
		(
			SELECT role_id
			FROM user_role
			WHERE user_id = @access_object_id AND [status] = @statusActive
		) user_role
		ON p.object_id = user_role.role_id AND p.object_type = @privilegeTypeRole
		WHERE [status] = @statusActive AND read_status = @readYes
		
		SELECT @total = COUNT(1)
		FROM section
		INNER JOIN @AvailableSections ON section_id = tmp_section_id
		WHERE [status] = @statusActive AND display = @displayYes AND parent = @parent_id
		
		SELECT s.section_id [id], sl.name [text], listing_item.name [iconCls], 
		CAST(leaf AS BIT) [leaf], s.link [url], s.module [module]
		FROM section s
		INNER JOIN @AvailableSections ON s.section_id = tmp_section_id
		LEFT JOIN listing_item ON s.icon = listing_item.value AND listing_item.list_id = @menuIconClassID
		INNER JOIN [section_lang] sl ON s.[section_id] = sl.[section_id] 
		WHERE s.[status] = @statusActive AND s.display = @displayYes
			AND s.parent = @parent_id
			AND sl.[lang_id] = @lang_id
		ORDER BY s.display_order ASC
		END
	ELSE
		BEGIN
			SET @remark = 'UserInvalid'

			SELECT s.section_id [id], sl.name [text], listing_item.name [iconCls], 
			CAST(leaf AS BIT) [leaf], s.link [url], s.module [module]
			FROM section s
			INNER JOIN @AvailableSections ON s.section_id = tmp_section_id
			LEFT JOIN listing_item ON s.icon = listing_item.value AND listing_item.list_id = @menuIconClassID
			INNER JOIN [section_lang] sl ON s.[section_id] = sl.[section_id] 
			WHERE
				1 = 2
		END
		
	-- Pending Task: Add Action Log	

END

GO