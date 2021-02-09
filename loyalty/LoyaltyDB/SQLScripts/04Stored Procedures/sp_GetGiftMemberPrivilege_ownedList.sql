IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetGiftMemberPrivilege_ownedList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetGiftMemberPrivilege_ownedList]
GO

/****** Object:  StoredProcedure [sp_GetGiftMemberPrivilege_ownedList]    Script Date: 2014/1/2 下午 06:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2014/1/2 下午 06:09:40
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetGiftMemberPrivilege_ownedList]
    -- Access Object
    @access_object_id INT,
    
	-- Data
	@gift_id INT,
    
    -- Paging
	--@rowIndexStart bigint,
	--@rowLimit int,
	-- Searching
	--@searchParams NVARCHAR(MAX),

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
        
    -- Set default output value        
    SET @sql_result = 0
	SET @sql_remark = ''
    
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
		    
            SELECT @recValid = 1
    		FROM gift
    		WHERE gift_id = @gift_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
                      
                    SELECT 
                        g.id, 
                        g.gift_id, 
                        g.member_level_id, 
                        g.allow_redeem, 
                        g.status, 
                        g.crt_date, 
                        g.crt_by_type, 
                        g.crt_by, 
                        g.upd_date, 
                        g.upd_by_type, 
                        g.upd_by,
						ml.name 'member_level_name'

                    FROM  gift_member_privilege g, member_level ml
                    WHERE g.gift_id = @gift_id
					AND g.member_level_id = ml.level_id
                    AND g.[record_status] <> @recDeleted
                    
                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                    SELECT 
                        g.id, 
                        g.gift_id, 
                        g.member_level_id, 
                        g.allow_redeem, 
                        g.status, 
                        g.crt_date, 
                        g.crt_by_type, 
                        g.crt_by, 
                        g.upd_date, 
                        g.upd_by_type, 
                        g.upd_by

                    FROM  gift_member_privilege g
                    WHERE 1=2
        			SET @sql_remark = 'RecordInvalid'
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_remark = 'AccessObjectInvalid'
		END
        
END
GO