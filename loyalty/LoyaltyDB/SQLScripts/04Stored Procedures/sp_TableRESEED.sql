IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_TableRESEED]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_TableRESEED]
GO

/****** Object:  StoredProcedure [sp_TableRESEED]    Script Date: 2013/10/8 下午 05:51:22 ******/
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
CREATE PROCEDURE [dbo].[sp_TableRESEED]
	-- Data
	@table_name VARCHAR(100),
	@identity INT

AS

BEGIN   
	DBCC CHECKIDENT (@table_name, RESEED, @identity)
END