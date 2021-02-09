/****** Object:  StoredProcedure [dbo].[sp_JSON]    Script Date: 2013/7/2 ¤U¤È 06:19:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_JSON]  (
	@ParameterSQL AS NVARCHAR(MAX),
	@JSON AS NVARCHAR(MAX) OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;
 
	DECLARE @SQL NVARCHAR(MAX)
	DECLARE @XMLString NVARCHAR(MAX)
	DECLARE @XML XML
	DECLARE @Paramlist NVARCHAR(MAX)
	SET @Paramlist = N'@XML XML OUTPUT'
	SET @SQL = 'WITH PrepareTable (XMLString) '
	SET @SQL = @SQL + 'AS ( '
	SET @SQL = @SQL + @ParameterSQL+ ' FOR XML RAW, TYPE, ELEMENTS '
	SET @SQL = @SQL + ') '
	SET @SQL = @SQL + 'SELECT @XML = XMLString FROM PrepareTable '
	SELECT @SQL
	EXEC sp_executesql @SQL, @Paramlist, @XML=@XML OUTPUT
	SET @XMLString = CAST(@XML AS NVARCHAR(MAX))
	 
	DECLARE @Row NVARCHAR(MAX)
	DECLARE @RowStart INT
	DECLARE @RowEnd INT
	DECLARE @FieldStart INT
	DECLARE @FieldEnd INT
	DECLARE @KEY NVARCHAR(MAX)
	DECLARE @Value NVARCHAR(MAX)
	 
	DECLARE @StartRoot NVARCHAR(100); SET @StartRoot = '<row>'
	DECLARE @EndRoot NVARCHAR(100); SET @EndRoot = '</row>'
	DECLARE @StartField NVARCHAR(100); SET @StartField = '<'
	DECLARE @EndField NVARCHAR(100); SET @EndField = '>'
	 
	SET @RowStart = CharIndex(@StartRoot, @XMLString, 0)
	SET @JSON = ''
	WHILE @RowStart > 0
	BEGIN
		SET @RowStart = @RowStart+Len(@StartRoot)
		SET @RowEnd = CharIndex(@EndRoot, @XMLString, @RowStart)
		SET @Row = SubString(@XMLString, @RowStart, @RowEnd-@RowStart)
		SET @JSON = @JSON+'{'
	 
		-- for each row
		SET @FieldStart = CharIndex(@StartField, @Row, 0)
		WHILE @FieldStart > 0
		BEGIN
			-- parse node key
			SET @FieldStart = @FieldStart+Len(@StartField)
			SET @FieldEnd = CharIndex(@EndField, @Row, @FieldStart)
			SET @KEY = SubString(@Row, @FieldStart, @FieldEnd-@FieldStart)
			SET @JSON = @JSON+'"'+@KEY+'":'
	 
			-- parse node value
			SET @FieldStart = @FieldEnd+1
			SET @FieldEnd = CharIndex('</', @Row, @FieldStart)
			SET @Value = SubString(@Row, @FieldStart, @FieldEnd-@FieldStart)
			IF ISNUMERIC(@Value) = 1 OR @Value = 'true' OR @Value = 'false'
				SET @JSON = @JSON+@Value+','
			ELSE IF @Value = '[[NULL]]'
				SET @JSON = @JSON+'null,'
			ELSE IF @Value = '[[EMPTY]]'
				SET @JSON = @JSON+'"",'
			ELSE 
				SET @JSON = @JSON+'"'+REPLACE(@Value, '"', '\"')+'",'
	 
			SET @FieldStart = @FieldStart+Len(@StartField)
			SET @FieldEnd = CharIndex(@EndField, @Row, @FieldStart)
			SET @FieldStart = CharIndex(@StartField, @Row, @FieldEnd)
		END	
		IF LEN(@JSON)>0 SET @JSON = SubString(@JSON, 0, LEN(@JSON))
		SET @JSON = @JSON+'},'
		--/ for each row
	 
		SET @RowStart = CharIndex(@StartRoot, @XMLString, @RowEnd)
	END
	IF LEN(@JSON)>0 SET @JSON = SubString(@JSON, 0, LEN(@JSON))
	SET @JSON = '[' + @JSON + ']'
	SELECT @JSON
END
GO


