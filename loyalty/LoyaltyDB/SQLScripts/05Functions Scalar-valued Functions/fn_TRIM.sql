IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'fn_TRIM') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION fn_TRIM
GO


CREATE FUNCTION dbo.fn_TRIM(@string VARCHAR(MAX))
RETURNS VARCHAR(MAX)

BEGIN

	RETURN LTRIM(RTRIM(@string))
END

GO