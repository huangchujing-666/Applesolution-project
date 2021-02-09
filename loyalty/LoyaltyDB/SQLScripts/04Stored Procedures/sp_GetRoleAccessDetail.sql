IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetRoleAccessDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetRoleAccessDetail]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetRoleAccessDetail]    Script Date: 2013/11/3 下午 01:20:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/16 下午 04:14:43
-- Description:	Stored Procedure for sp_GetRoleAccessDetail
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetRoleAccessDetail]
	-- Access Object
	@access_object_id INT,
	
	-- Data
	@role_id INT,

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
    
	SET @accessObjectValid = 0
    
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

			SELECT s.[name], s.[leaf], s.[parent], [privilege_id]
				  ,p.object_type
				  ,p.object_id
				  ,s.[section_id]
				  ,[read_status]
				  ,[insert_status]
				  ,[update_status]
				  ,[delete_status]
				  ,p.[status]
				  ,p.[crt_date]
				  ,p.[crt_by_type]
				  ,p.[crt_by]
				  ,p.[upd_date]
				  ,p.[upd_by_type]
				  ,p.[upd_by]
			  FROM [section] s
			  LEFT JOIN [privilege] p
				ON p.[section_id] = s.[section_id] AND p.object_id = @role_id AND [p].object_type = 1 -- user
			  WHERE s.status = 1

			  ORDER BY s.[parent], s.[display_order]

	        SET @sql_result = 1
		END
	ELSE
		BEGIN
		SET @sql_remark = 'UserInvalid'
		
		END
END