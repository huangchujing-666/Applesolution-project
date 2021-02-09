/****** Object:  StoredProcedure [dbo].[sp_UpdateDisplayOrderByTable]    Script Date: 2013/7/2 ¤U¤È 06:20:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_UpdateDisplayOrderByTable]
	@tableName varchar(20)
AS
	BEGIN
	declare @length int
	declare @selectLengthSQL nvarchar(max)
	DECLARE @statusActive INT
	DECLARE @statusDeleted INT

	SET @statusDeleted = -1
	set @length=1
	set @statusActive=1
	set @selectLengthSQL='
	select @length=count(*)  from '+@tableName +' where status <> '+convert(varchar,@statusDeleted)
	EXEC sp_executesql @selectLengthSQL,N'@length int output',@length output

	begin
		DECLARE @tableKeyName nvarchar(128)
		set @tableKeyName=''
		select top 1 @tableKeyName=COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where TABLE_NAME=@tableName
		declare @updateSQL nvarchar(max)
		declare @SelectKeySQL nvarchar(max)
		declare @id int		
		set @SelectKeySQL='
		IF object_id(''[tempdb].[dbo].##temp_loyalty'') IS NOT NULL
            drop table ##temp_loyalty
		create table ##temp_loyalty (
			table_id INT PRIMARY KEY CLUSTERED,
			[row_num] int
		)
		insert into ##temp_loyalty (row_num,table_id) SELECT ROW_NUMBER() OVER (ORDER BY display_order) AS RowNum,'+@tableKeyName+'
								from '+@tableName +' where status <> '+convert(varchar,@statusDeleted)
		exec sp_executesql @selectKeySQL
		while @length>0
		begin
			set @SelectKeySQL='select @id=table_id from ##temp_loyalty
								where row_num='+convert(nvarchar,@length)
			exec sp_executesql @selectKeySQL,
			N'@id int output',@id output
			set @updateSQL='update '+@tableName+' set display_order='+convert(nvarchar,@length)+' where '+@tableKeyName+'='+convert(nvarchar,@id)
			exec sp_executesql @updateSQL
			set @length=@length-1
		end
	end
	END

GO


