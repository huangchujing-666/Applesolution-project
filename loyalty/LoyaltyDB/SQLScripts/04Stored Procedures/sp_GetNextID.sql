IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetNextID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetNextID]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetNextID]    Script Date: 2013/7/2 ¤U¤È 06:18:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		LEO
-- Create date: 2013-06-03 15:00
-- Description:	Stored Procedure for Get Next identity id
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetNextID]
	@table_name VARCHAR(20),
	@id INT OUTPUT
AS
	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @id =(IDENT_CURRENT (@table_name) + IDENT_INCR (@table_name))

END

GO


