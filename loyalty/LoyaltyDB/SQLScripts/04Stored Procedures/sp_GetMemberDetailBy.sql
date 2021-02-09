IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetMemberDetailBy]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetMemberDetailBy]
GO

/****** Object:  StoredProcedure [sp_GetMemberDetailBy]    Script Date: 2013/7/17 下午 02:29:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/17 下午 02:29:45
-- Description:	Stored Procedure for Get Member Detail By MemberID
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetMemberDetailBy]
	-- Access Object
    @access_object_id INT,
	
	-- Data
	@member_id INT,
	@session NVARCHAR(MAX),

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
    
    SET @sql_result = 0

	-- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted

    IF @accessObjectValid = 1
		BEGIN
			SELECT m.[member_id], m.[member_no], m.[email], m.[fbid], m.[fbemail], m.[mobile_no], m.[password], m.[salutation], m.[firstname], m.[middlename], m.[lastname],
				m.[birth_year], m.[birth_month], m.[birth_day],  m.[gender], m.[hkid], 
				m.[address1], m.[address2], m.[address3],
				m.[district], m.[region],
				m.[reg_source], m.[reg_status],
				m.[activate_key], m.[reg_ip], m.[referrer], m.[status],
				m.[opt_in], m.[member_level_id], m.[member_category_id], ml.name 'member_level_name', m.[crt_date], m.[upd_date], 1 'sql_status', '' 'sql_remark'
			FROM
				[member_profile] m , [member_level] ml
			WHERE
				[member_id] = @member_id
				AND m.[member_level_id] = ml.[level_id]

	        SET @sql_result = 1
		END
	ELSE
		BEGIN
			SELECT 0 [member_id], '' [member_no], '' [email], '' [fbid], '' [fbemail], '' [password], '' [mobile_no], 0 [salutation],
			'' [firstname], '' [middlename], '' [lastname], 0 [birth_year],
			0 [birth_month], 0 [birth_day], 0 [gender], '' [hkid], 
			'' [address1], '' [address2], '' [address3],
			0 [district], 0 [region],
			0 [reg_source], 0 [referrer], 0 [reg_status], '' [reg_ip], '' [activate_key],
			0 [status], 0 [opt_in], 0 [member_level_id], 0 [member_category_id], '' 'member_level_name',
			GETDATE() [crt_date], GETDATE() [upd_date], -1 'sql_status', @sql_remark 'sql_remark'
	
			SET @sql_remark = 'UserAccessInvalid'
		END
END
GO