IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetTableCurrentIdentity]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetTableCurrentIdentity]
GO

/****** Object:  StoredProcedure [sp_GetTableCurrentIdentity]    Script Date: 2013/10/8 下午 05:51:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/8 下午 05:51:22
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetTableCurrentIdentity]
	-- Data
	@table_name VARCHAR(100),

	-- Output
    @identity INT OUTPUT
AS
   

BEGIN   
	SELECT @identity = IDENT_CURRENT( @table_name )
END