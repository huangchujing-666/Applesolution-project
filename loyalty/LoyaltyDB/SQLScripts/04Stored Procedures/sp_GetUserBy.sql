IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetUserBy]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetUserBy]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetUserBy]    Script Date: 2013/7/2 ¤U¤È 06:18:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		LEO
-- Create date: 2013-06-03 15:00
-- Description:	Stored Procedure for Get User List By
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUserBy]
	@user_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [user_id], [login_id], so.[name] , [email], li.[name] as 'status', so.[status] as 'status_id', u.[action_ip]
	FROM [user_profile] u, [system_object] so, [listing] l, [listing_item] li
	WHERE
			u.user_id = so.object_id
		AND so.[status] = li.[value]
		AND l.[code] = 'Status'
		AND l.[list_id] = li.[list_id]
		AND user_id = @user_id
END

GO


