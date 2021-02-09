IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateUserPassword]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateUserPassword]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		LEO
-- Create date: 2013-06-03 15:00
-- Description:	Stored Procedure for Update User Password
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateUserPassword]

    -- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
	@user_id int,
	@oldPassword NVARCHAR(MAX),
	@newPassword NVARCHAR(MAX),
	@status INT OUTPUT,
	@remark VARCHAR(100) OUTPUT
AS
	DECLARE @recValid INT
	DECLARE @section_id INT
	DECLARE @action_by INT
	DECLARE @description NVARCHAR(MAX)
	
	SET @recValid = 0	
	SET @status = 0
	SET @remark = ''

	SET @section_id = 4	
	SET @action_by = 1
	SET @description = N'Update User Password'
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here

	-- Begin Transaction
	BEGIN TRAN sp_UpdateUserPassword

	--SELECT @recValid = 1 
    --FROM user_profile
    --WHERE [user_id] = @user_id AND [password] = @oldPassword

	SET @recValid = 1
	IF @recValid = 1
	BEGIN
		UPDATE [user_profile]
		SET [password]=@newPassword, 
						upd_date = GETDATE(), 
						upd_by_type = @access_object_type, 
                        upd_by = @access_object_id

		where [user_id] = @user_id

		SET @status = 1
	END
	ELSE
	BEGIN
		SET @remark = 'OldPasswordInvalid'
	END

	-- Take Log
	-- SET @description = @description +  CONVERT(varchar, @user_id) 
	-- EXEC [dbo].[sp_TakeLog] @section_id=@section_id, @action_by =@action_by, @description=@description

	COMMIT TRAN sp_UpdateUserPassword
END
GO
