IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fsp_RandomInteger]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[fsp_RandomInteger]
GO

/****** Object:  StoredProcedure [dbo].[fsp_RandomInteger]    Script Date: 2013/8/10 下午 07:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/9 下午 06:39:07
-- Description:	Stored Procedure for Random an Integer
-- Attention, rand() cannot be used inside function. Therefore, make store procedure here.
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[fsp_RandomInteger]
	@lower INT,
	@upper INT,
	@randomInteger INT OUTPUT
AS
   
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- random range [@lower - @upper]
	SET @RandomInteger =  @Lower + CONVERT(INT, (@Upper-@Lower+1)*RAND())
END