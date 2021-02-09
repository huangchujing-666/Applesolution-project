
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_MemberLogin]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_MemberLogin]
GO

/****** Object:  StoredProcedure [sp_MemberLogin]    Script Date: 2013/7/21 下午 06:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/21 下午 06:59:23
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_MemberLogin]
    @member_id int,
    @password NVARCHAR(MAX),
	@session NVARCHAR(MAX),

	@sql_result INT OUTPUT,
	@sql_remark VARCHAR(100) OUTPUT
AS
    DECLARE @memberValid INT SET @memberValid = 0

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
    
    -- Insert statements for procedure here 
	IF (@member_id>0)
		BEGIN
			SELECT @memberValid = 1
			FROM member_profile
			WHERE member_id = @member_id
				AND password = @password
				AND [status] = @statusActive 
				AND record_status <> @recDeleted
		END

    IF @memberValid = 1
		BEGIN     
			UPDATE member_profile
			SET session = @session
			WHERE member_id= @member_id

			SET @sql_remark = 'DONE'
            SET @sql_result = 1
		END
	ELSE
		BEGIN
		    SET @sql_remark = 'MemberInvalid'
		END

END
GO
