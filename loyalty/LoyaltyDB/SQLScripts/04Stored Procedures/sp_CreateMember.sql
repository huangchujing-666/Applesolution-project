IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_CreateMember]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_CreateMember]
GO

/****** Object:  StoredProcedure [sp_CreateMember]    Script Date: 2013/7/21 下午 05:36:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/7/21 下午 05:36:36
-- Description:	Stored Procedure for Create member
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_CreateMember]
	-- Access Object
    @access_object_id INT,
    @access_object_type int, 
	-- Data
	-- @member_id INT,
	@member_no varchar(20),
	@password varchar(255),
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
	@hkid varchar(30),
	
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

	-- Output
	@new_member_id INT OUTPUT,
	@sql_result INT OUTPUT,
	@sql_remark VARCHAR(100) OUTPUT
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
    
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Check Access Object

	--SELECT @accessObjectValid = 1, @accessObjectType = ao.type
 --   FROM v_accessObject ao
 --   WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
	set @accessObjectValid = 1
    IF @accessObjectValid = 1
		BEGIN
			
			DECLARE @duplicated_member_no INT SET @duplicated_member_no = 0

            SELECT @duplicated_member_no = 1
			FROM member_profile m
			WHERE
				m.[record_status] <> @recDeleted
				AND m.member_no = @member_no

			IF @duplicated_member_no = 0
				SET @recValid = 1
 		
		--SET  @recValid = 1

            IF @recValid = 1
    		    BEGIN

					-- SET IDENTITY_INSERT [dbo].member_profile ON

					INSERT INTO member_profile (
						-- member_id,
						member_no, 
						password, 
						email, 
						fbid,
						fbemail, 
						mobile_no, 
						salutation, 
						firstname, 
						middlename, 
						lastname, 
						fullname, 

						birth_year, 
						birth_month, 
						birth_day, 
						gender, 
						hkid, 
						
						address1, 
						address2, 
						address3, 
						district, 
						region,

						reg_source, 
						referrer, 
						reg_status, 
						reg_ip, 
						activate_key, 
						hash_key, 
						status, 
						opt_in, 
						member_level_id, 
						member_category_id, 
						available_point,

						crt_date, 
						upd_date, 
						crt_by_type, 
						crt_by, 
						upd_by_type, 
						upd_by
						)
					VALUES (
						-- @member_id,
						@member_no, 
						@password,
						@email, 
						@fbid, 
						@fbemail, 
						@mobile_no, 
						@salutation, 
						@firstname, 
						@middlename, 
						@lastname,
						@fullname,  
						
						@birth_year, 
						@birth_month, 
						@birth_day, 
						@gender, 
						@hkid, 
						
						@address1, 
						@address2, 
						@address3, 
						@district, 
						@region,
						
						@reg_source, 
						@referrer, 
						@reg_status, 
						@reg_ip, 
						@activate_key, 
						@hash_key, 
						@status, 
						@opt_in, 
						@member_level_id, 
						@member_category_id,
						@available_point, 

						GETDATE(), 
						GETDATE(), 
						@access_object_type, 
						@access_object_id, 
						@access_object_type, 
						@access_object_id
					)

					SET @new_member_id = SCOPE_IDENTITY()

                    SET @sql_result = 1

					-- SET IDENTITY_INSERT [dbo].member_profile OFF
					
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
END
GO