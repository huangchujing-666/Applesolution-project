IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_UpdateMemberCard]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_UpdateMemberCard]
GO

/****** Object:  StoredProcedure [sp_UpdateMemberCard]    Script Date: 2014/5/12 下午 05:38:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/5/12 下午 05:38:35
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateMemberCard]
    -- Access Object
 @access_object_type INT, 
    @access_object_id INT,
    
    -- Data
	@card_id int,
	@member_id int,
	@card_no varchar(20),
	@card_version int,
	@card_type int,
	@card_status int,
	@issue_date datetime,
	@old_card_id int,
	@remark nvarchar(max),
	@status int,
	
    -- Output
    @sql_result INT OUTPUT
AS
	-- access object params
    DECLARE @accessObjectValid INT SET @accessObjectValid = 0
	DECLARE @accessObjectType INT SET @accessObjectType = 0
    
    -- record status and validity
	DECLARE @recValid INT SET @recValid = 0
	DECLARE @statusActive INT SET @statusActive = [dbo].fn_GetListingItemValByCodeName('Status', 'Active')
	DECLARE @recDeleted INT SET @recDeleted  = [dbo].fn_GetListingItemValByCodeName('RecordStatus', 'Deleted')
        
    -- Set default output value        
    SET @sql_result = 0
    
BEGIN    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Begin Transaction
    BEGIN TRAN sp_UpdateMemberCard
    
    -- Check Access Object
---    SELECT @accessObjectValid = 1, @accessObjectType = ao.type
---    FROM v_accessObject ao
---    WHERE target_id = @access_object_id AND [status] = @statusActive AND record_status <> @recDeleted
    
SET @accessObjectValid = 1 
    IF @accessObjectValid = 1
		BEGIN
		    
            SELECT @recValid = 1
    		FROM member_card
    		WHERE card_id = @card_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                
                    UPDATE member_card
                    SET
                        member_id = @member_id, 
                        card_no = @card_no, 
						card_version = @card_version,
                        card_type = @card_type, 
                        card_status = @card_status, 
                        issue_date = @issue_date, 
                        old_card_id = @old_card_id, 
                        remark = @remark, 
                        status = @status, 
                       
                        upd_date = GETDATE(), 
upd_by_type = @access_object_type, 
                        upd_by = @access_object_id
                        
                    WHERE card_id = @card_id
                    
                    SET @sql_result = 100 --normal
                END
	    	ELSE
    		    BEGIN
        			SET @sql_result = 1112  --Record Invalid
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_result = 1111 --no_permission
		END
        
    -- Commit Transaction
    COMMIT TRAN sp_UpdateMemberCard
END
GO
