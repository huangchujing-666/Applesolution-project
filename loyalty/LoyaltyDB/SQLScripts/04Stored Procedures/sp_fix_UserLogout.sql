IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UserLogout]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UserLogout]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserLogout]    Script Date: 2013/7/2 ¤U¤È 06:23:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Leo
-- Create date: 2013-06-0. 15:35
-- Description:	Stored Procedure for User Logout Logging
-- Pending Task:
-- =============================================
CREATE PROCEDURE [dbo].[sp_UserLogout]
	-- Add the parameters for the stored procedure here
	@user_id INT
AS
	DECLARE @section_id INT
	DECLARE @description NVARCHAR(MAX)

	SET @section_id = 54	
	SET @description = N'Web Server Logout'
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	-- Take Log
	EXEC [dbo].[sp_TakeLog] @section_id=@section_id, @action_by =@user_id, @description=@description
END

GO


