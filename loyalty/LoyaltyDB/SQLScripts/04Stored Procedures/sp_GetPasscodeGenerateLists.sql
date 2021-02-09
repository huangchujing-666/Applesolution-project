IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetPasscodeGenerateLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetPasscodeGenerateLists]
GO

/****** Object:  StoredProcedure [sp_GetPasscodeGenerateLists]    Script Date: 2013/8/19 下午 02:28:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/8/19 下午 02:28:39
-- Description:	Stored Procedure for Get Passcode Generate Job List
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetPasscodeGenerateLists]
	-- Access Object
    @access_object_id INT,
    
	-- Paging
	@rowIndexStart int,
	@rowLimit int,

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
        
    SET @sql_result = 0
	SET @sql_remark = ''

	DECLARE @selectSQL NVARCHAR(MAX)
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    
    -- Check Access Object
	SELECT @accessObjectValid = 1, @accessObjectType = ao.type
    FROM v_accessObject ao
    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
    IF @accessObjectValid = 1
		BEGIN

            SET @recValid = 1

            IF @recValid = 1
    		    BEGIN
                      
					SET @selectSQL = 
						'SELECT * FROM ( 
							 SELECT CAST(COUNT(*) OVER() as INT) as rowTotal, ROW_NUMBER() OVER (ORDER BY p.crt_date desc) as row,
							p.generate_id,
							p.noToGenerate,
							p.generateCompleteCounter,
							p.insertErrorCounter,
							p.error_messgae,
							p.generate_status [generate_status_id], 
							li.name [generate_status_name], 
							p.crt_date, 
							p.upd_date,
							u.login_id [crt_by_name],
							u.user_id

							FROM  passcode_generate p
							INNER JOIN user_profile u
								ON u.user_id = p.crt_by
							INNER JOIN listing_item li
								ON p.generate_status = li.value
							INNER JOIN listing l
								ON l.list_id = li.list_id
							WHERE 
								1=1
								AND l.[code] = ''PasscodeGenerateStatus''
						
						) t WHERE row > ' + CAST(@rowIndexStart AS varchar(MAX)) +' and row <= ' + CAST((@rowIndexStart+ @rowLimit) AS varchar(MAX))
					
					--EXEC sp_executesql @selectSQL

					 SELECT CAST(COUNT(*) OVER() as INT) as rowTotal, ROW_NUMBER() OVER (ORDER BY p.crt_date desc) as row,
							p.generate_id,
							p.noToGenerate,
							p.generateCompleteCounter,
							p.insertErrorCounter,
							p.error_messgae,
							p.generate_status [generate_status_id], 
							li.name [generate_status_name], 
							p.crt_date, 
							p.upd_date,
							u.login_id [crt_by_name],
							u.user_id

							FROM  passcode_generate p
							INNER JOIN user_profile u
								ON u.user_id = p.crt_by
							INNER JOIN listing_item li
								ON p.generate_status = li.value
							INNER JOIN listing l
								ON l.list_id = li.list_id
							WHERE 
								1=1
								AND l.[code] = 'PasscodeGenerateStatus'

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
END
GO