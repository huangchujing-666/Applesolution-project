IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetMemberLevelLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetMemberLevelLists]
GO

/****** Object:  StoredProcedure [sp_GetMemberLevelLists]    Script Date: 2013/7/18 上午 11:13:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/18 上午 11:13:41
-- Description:	Stored Procedure for Get MemberLevel Lists
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetMemberLevelLists]
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
		    
			SELECT [level_id]
				,ml.[name]
				,[point_required]
				,[redeem_discount]
				,li.[name] as 'status'
				,ml.[status] as 'status_id'
			FROM [member_level] ml, [listing] l, [listing_item] li
			WHERE
				ml.[status] = li.[value]
				AND l.[code] = 'Status'
				AND l.[list_id] = li.[list_id]
				--AND ml.[record_status] <> @recDeleted
			ORDER BY ml.display_order
			   
            SET @sql_result = 1  
		END
	ELSE
		BEGIN
		    SET @sql_remark = 'UserInvalid'
		END

END
GO