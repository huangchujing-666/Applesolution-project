
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateMember]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateMember]
GO

/****** Object:  StoredProcedure [sp_UpdateMember]    Script Date: 2013/7/19 下午 03:51:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/19 下午 03:51:20
-- Description:	Stored Procedure for UpdateMember
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateMember]
	-- Access Object
    @access_object_id INT,
	@access_object_type int, 

	-- Data
	@member_id int,
    @member_no varchar(20),

	@password NVARCHAR(MAX),
	@email varchar(100),
	@fbid varchar(100),
	@fbemail varchar(100),
	@mobile_no varchar(12),
	@salutation int,
	@firstname nvarchar(255),
	@middlename nvarchar(255),
	@lastname nvarchar(255),
	@fullname nvarchar(255),

	@birth_year int,
	@birth_month int,
	@birth_day int,
	@gender int,
	@hkid varchar(20),
	
	@address1 nvarchar(100),
	@address2 nvarchar(100),
	@address3 nvarchar(100),
	@district int,
	@region int,

	@reg_source int,
	@referrer int,
	@reg_status int,
	@reg_ip varchar(50),
	@activate_key varchar(50),
	@hash_key varchar(50),
	@status int,
	@opt_in int,
	@member_level_id int,
	@member_category_id int,
	@available_point float,


	@crt_by_type int,
	@crt_by int,
	@upd_by_type int,
	@upd_by int,
	
	-- Output
	@sql_result INT OUTPUT,
	@sql_remark NVARCHAR(MAX) OUTPUT
AS
	-- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')
        
    SET @sql_result = 0
	SET @sql_remark = ''
	    
	--
	DECLARE @updateSQL NVARCHAR(MAX)

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_UpdateMember
    
    -- Check Access Object
	--SELECT @accessObjectValid = 1, @accessObjectType = ao.type
 --   FROM v_accessObject ao
 --   WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    set @accessObjectValid = 1
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM member_profile
    		WHERE record_status <> @recDeleted AND member_id = @member_id
			
            IF @recValid = 1
    		    BEGIN
					SET @updateSQL = N' UPDATE member_profile SET'  
				IF (@member_no IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'member_no = N''' + @member_no + '''' 
				IF (@password IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'password = N''' + @password + '''' 
				IF (@email IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'email = N''' +  CAST(@email AS varchar)+ '''' 
				IF (@fbid IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'fbid = N''' +  CAST(@fbid AS varchar)+ '''' 
				IF (@fbemail IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'fbemail = N''' +  CAST(@fbemail AS varchar)+ '''' 
				IF (@mobile_no IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'mobile_no = N''' +  CAST(@mobile_no AS varchar)+ '''' 
				IF (@salutation IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'salutation = ' +  CAST(@salutation AS varchar)
				IF (@firstname IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'firstname = N''' +  CAST(@firstname AS nvarchar)+ '''' 
				IF (@middlename IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'middlename = N''' +  CAST(@middlename AS nvarchar)+ '''' 
				IF (@lastname IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'lastname = N''' +  CAST(@lastname AS nvarchar)+ '''' 
				IF (@fullname IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'fullname = N''' +  CAST(@fullname AS nvarchar)+ '''' 
				IF (@birth_year IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'birth_year = ' +  CAST(@birth_year AS varchar) 
				IF (@birth_month IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'birth_month = ' +  CAST(@birth_month AS varchar) 
				IF (@birth_day IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'birth_day = ' +  CAST(@birth_day AS varchar)
				IF (@gender IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'gender = ' +  CAST(@gender AS varchar)
				IF (@hkid IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'hkid = N''' +  CAST(@hkid AS varchar)+ ''''
				
				IF (@address1 IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'address1 = N''' +  CAST(@address1 AS varchar)+ '''' 
				IF (@address2 IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'address2 = N''' +  CAST(@address2 AS varchar)+ '''' 
				IF (@address3 IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'address3 = N''' +  CAST(@address3 AS varchar)+ '''' 
				IF (@district IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'district = ' +  CAST(@district AS varchar)
				IF (@region IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'region = ' +  CAST(@region AS varchar)

				IF (@reg_source IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'reg_source = ' +  CAST(@reg_source AS varchar) 
				IF (@referrer IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'referrer = ' +  CAST(@referrer AS varchar)
				IF (@reg_status IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'reg_status = ' +  CAST(@reg_status AS varchar)
				IF (@reg_ip IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'reg_ip = N''' +  CAST(@reg_ip AS varchar)+ '''' 
				IF (@activate_key IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'activate_key = N''' +  CAST(@activate_key AS varchar)+ '''' 
				IF (@hash_key IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'hash_key = N''' +  CAST(@hash_key AS varchar)+ '''' 
				IF (@status IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'status = ' +  CAST(@status AS varchar) 
				IF (@opt_in IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'opt_in = ' +  CAST(@opt_in AS varchar)
				IF (@member_level_id IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'member_level_id = ' +  CAST(@member_level_id AS varchar) 
				IF (@member_category_id IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'member_category_id = ' +  CAST(@member_category_id AS varchar) 
				IF (@available_point IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'available_point = ' +  CAST(@available_point AS varchar) 
		
			
				IF (@crt_by_type IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'crt_by_type = N''' +  CAST(@crt_by_type AS varchar)+ '''' 
				IF (@crt_by IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'crt_by = ' +  CAST(@crt_by AS varchar) 
				IF (@access_object_id IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'upd_by_type = ' +  CAST(@access_object_id AS varchar) 
				IF (@access_object_type IS NOT NULL) SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) +'upd_by = ' +  CAST(@access_object_type AS varchar) 

				SET @updateSQL = [dbo].[fn_buildUpdateSql](@updateSQL) + 'upd_date = GETDATE()'
               
					SET @updateSQL = @updateSQL + ' WHERE member_id = '+ CAST(@member_id AS varchar)
					--SET @sql_remark = @password
					EXEC sp_executesql @updateSQL
					
                    SET @sql_result = 1
    		    END
	    	ELSE
    		    BEGIN
        			SET @sql_remark = 'RecordInvalid'
			
    		    END
		END
	ELSE
		BEGIN
		    SET @sql_remark = 'UserInvalid'
		END
     
    COMMIT TRAN sp_UpdateMember
END
GO
