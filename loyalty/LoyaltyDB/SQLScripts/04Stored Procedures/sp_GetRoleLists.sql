IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetRoleLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetRoleLists]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetRoleLists]    Script Date: 2013/7/2 ¤U¤È 06:17:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chau Yun Pang
-- Create date: 2013-01-15
-- Description:	Stored Procedure for Get Role Lists
-- Pending Task: Add Action Log
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetRoleLists]
	-- Access Object
	@access_object_id INT,

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

	DECLARE @listingCode VARCHAR(50) SET @listingCode = 'Status'
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
		
		SELECT r.[role_id], r.[name], li.[name] as 'status', r.[status] as 'status_id'
		FROM [role] r, [listing] l, [listing_item] li
		WHERE
		r.[status] = li.[value]
		AND l.[code] = @listingCode
		AND l.[list_id] = li.[list_id]

		SET @sql_result = 1
		END
	ELSE
		BEGIN
		SET @sql_remark = 'UserInvalid'
		SELECT NULL role_id, NULL name, NULL [status], NULL crt_date, NULL upd_date, NULL crt_by, NULL upd_by
		END
		
	-- Pending Task: Add Action Log	
	
END

GO


