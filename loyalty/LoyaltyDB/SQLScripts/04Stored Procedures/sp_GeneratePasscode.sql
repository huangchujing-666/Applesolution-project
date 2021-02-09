IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GeneratePasscode]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GeneratePasscode]
GO

/****** Object:  StoredProcedure [sp_GeneratePasscode]    Script Date: 2013/8/9 下午 06:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/9 下午 06:39:07
-- Description:	Stored Procedure for Generate Passcode
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GeneratePasscode]
	-- Access Object
    @access_object_id INT,
  
	-- Data
	@passcode_prefix_id int,
	@noToGenerate bigint,

	@active_date datetime,

	@status int,
	@void_date datetime,
	@void_reason int,

	-- Output
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

    
    DECLARE @passcode_format VARCHAR(30)
	DECLARE @prefix_value VARCHAR(10)
	DECLARE @generate_id INT

	DECLARE @expiry_date datetime
	DECLARE @noOfMonthToExpiry INT

	-- for generate
	declare @randomInteger int
	declare @alphabet varchar(36) SET @alphabet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'
	declare @alphabet_number varchar(36) SET @alphabet_number = '0123456789'
	declare @getLetter varchar(1)
	declare @loopPasscodeFormatComplete int SET @loopPasscodeFormatComplete = 0
	declare @loopAllGenerateComplete int SET @loopAllGenerateComplete = 0
	declare @generateCompleteCounter bigint SET @generateCompleteCounter = 0
	declare @insertErrorCounter int SET @insertErrorCounter = 0
	declare @counter int
	declare @buffChar varchar(1)
	declare @passcodeGenerated varchar(30) SET @passcodeGenerated = ''
	declare @product_id int
	declare @point float SET @point = 0

        
    SET @sql_result = 0
	SET @sql_remark = ''
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_GeneratePasscode
    
    -- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN
		    
			-- GET basic data of passcode and check valid
			SELECT @recValid = 1,  @passcode_format = pf.passcode_format, @prefix_value = pp.prefix_value, @noOfMonthToExpiry = pf.expiry_month, @product_id = pp.product_id
            FROM passcode_prefix pp, passcode_format pf
			WHERE
				pp.format_id = pf.format_id 
				AND pp.prefix_id = @passcode_prefix_id
				AND pp.status = @statusActive
				AND pf.status = @statusActive
				
            IF @recValid = 1
    		    BEGIN

					-- Get Point of product
					SELECT @point = p.point
					FROM product p
					WHERE p.product_id = @product_id


                    
					SET @expiry_date = DATEADD(month, @noOfMonthToExpiry, @active_date)

					-- CREATE generate job
					INSERT INTO passcode_generate
                    (                     
                        noToGenerate, 
                        generateCompleteCounter, 
                        insertErrorCounter,                     
                        generate_status, 
                        crt_date, 
						crt_by_type,
						crt_by,
                        upd_date,                        
						upd_by_type,
                        upd_by
                    )
                    VALUES (                    
                        @noToGenerate,  --noToGenerate
                        0,  --generateCompleteCounter
                        0,  --insertErrorCounter
                        0,  --generate_status
                        GETDATE(),  --crt_date
						1,
						@access_object_id,
                        GETDATE(),  --upd_date
						1,
                        @access_object_id --crt_by
                    )          
					SET @generate_id = SCOPE_IDENTITY()
					-- END generate job

					SET @loopAllGenerateComplete = 0
					SET @generateCompleteCounter = 0

					WHILE @loopAllGenerateComplete = 0
					BEGIN
						BEGIN TRY --TRY generate a passcode and insert it

							-- START generate a passcode
							SET @passcodeGenerated = @prefix_value

							SET @counter = 1
							SET @loopPasscodeFormatComplete = 0
							WHILE @loopPasscodeFormatComplete = 0
							BEGIN
								SET @buffChar = substring(@passcode_format,  @counter, 1)

								IF (@buffChar = '$')
								BEGIN
									EXEC fsp_RandomInteger @lower = 1, @upper =36, @randomInteger=@randomInteger output

									SET @passcodeGenerated = @passcodeGenerated + substring(@alphabet,  @randomInteger, 1)
								END
								ELSE IF (@buffChar = '#')
								BEGIN
									EXEC fsp_RandomInteger @lower = 1, @upper =10, @randomInteger=@randomInteger output
										SET @passcodeGenerated = @passcodeGenerated + substring(@alphabet_number,  @randomInteger, 1)
								END
								ELSE IF (@buffChar = '-')
								BEGIN
										SET @passcodeGenerated = @passcodeGenerated +  '-'
								END

								SET @counter = @counter + 1

								IF (@counter = LEN(@passcode_format))
								BEGIN
									SET @loopPasscodeFormatComplete = 1
								END
							END
							-- END generate a passcode
				
							-- TRY insert
							INSERT INTO passcode
							(
								pin_value,
								generate_id,
								passcode_prefix_id,
                      
								active_date,
								expiry_date,
							
								point,
								product_id,
								status,
								registered,
								void_date,
								void_reason,
								
								crt_date,
								crt_by_type,
								crt_by,
								upd_date,
								upd_by_type,
								upd_by
							)
							VALUES (
								@passcodeGenerated,
								@generate_id,
								@passcode_prefix_id,  --passcode_prefix_id
                                     
								@active_date,  --active_date
								@expiry_date,  --expiry_date
								
			
								@point,  --point
								@product_id, --product_id
								@status,  --status
								0, --registered,
								@void_date,  --void_date
								@void_reason,  --void_reason
							
								GETDATE(),  --crt_date
								1,
								@access_object_id,
								GETDATE(),  --upd_date
								1,  
								@access_object_id  --upd_by
							)   

							SET @generateCompleteCounter = @generateCompleteCounter + 1
							
					
						END TRY
						
						BEGIN CATCH
							SET @insertErrorCounter = @insertErrorCounter + 1

							SET @sql_remark = 'InsertError'

							IF (@insertErrorCounter = 10) -- Hit Max error, stop progress
							BEGIN 
								SET @loopAllGenerateComplete = 1

								-- UPDATE generate job status
								UPDATE passcode_generate
								SET
									generateCompleteCounter = @generateCompleteCounter,
									insertErrorCounter = @insertErrorCounter,
									generate_status = -1, --fail
									error_messgae = ERROR_MESSAGE(),
									upd_date = GETDATE()

								WHERE
									generate_id = @generate_id
								-- [END] UPDATE generate job status

								-- UPDATE Prefix usage
								EXEC sp_UpdatePasscodePrefix_usage @access_object_id = @access_object_id,  @prefix_id = @passcode_prefix_id, @sql_result = null, @sql_remark = null

							END
						END CATCH
		
						IF (@generateCompleteCounter = @noToGenerate) -- ALL COMPLETE
						BEGIN
							SET @loopAllGenerateComplete = 1

							-- UPDATE generate job status
							UPDATE passcode_generate
							SET
								generateCompleteCounter = @generateCompleteCounter,
								generate_status = 1,-- success status
								upd_date = GETDATE()
							WHERE
								generate_id = @generate_id
							-- [END] UPDATE generate job status

							-- UPDATE Prefix usage
							EXEC sp_UpdatePasscodePrefix_usage @access_object_id = @access_object_id,  @prefix_id = @passcode_prefix_id, @sql_result = null, @sql_remark = null

							SET @sql_result = 1
						END
						ELSE-- still progressing
						BEGIN
							-- UPDATE generate job status
							UPDATE passcode_generate
							SET
								generateCompleteCounter = @generateCompleteCounter,
								upd_date = GETDATE()
							WHERE
								generate_id = @generate_id
							-- [END] UPDATE generate job status
						END
					END 
                  
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
        
    -- Commit Transaction
    COMMIT TRAN sp_GeneratePasscode
END
GO
    
    
