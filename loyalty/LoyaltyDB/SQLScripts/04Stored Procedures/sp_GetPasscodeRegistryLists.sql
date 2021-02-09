IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetPasscodeRegistryLists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetPasscodeRegistryLists]
GO

/****** Object:  StoredProcedure [sp_GetPasscodeRegistryLists]    Script Date: 2013/10/20 下午 05:12:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/20 下午 05:12:04
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetPasscodeRegistryLists]
	-- Access Object
    @access_object_id INT,
    
	-- Data
	@member_id INT,

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
    		FROM member_profile
    		WHERE member_id = @member_id AND record_status <> @recDeleted
            
            IF @recValid = 1
    		    BEGIN
  
                    SELECT 
                        te.transaction_id, 
                        t.type, 
                        t.source_id, 

                        t.member_id, 
                        te.point_earn, 
                        te.point_used, 
                        te.status, 
                        t.void_date, 
                        t.remark, 
                        t.display, 
                        te.crt_date, 
                        te.crt_by_type, 
                        te.crt_by, 
                        te.upd_date, 
                        te.upd_by_type, 
                        te.upd_by, 
                        te.record_status,
						p.pin_value,
						so.name 'member_name',
						li.name 'status_name'

                    FROM  transaction_earn te, [transaction] t, member_profile m, passcode p, listing l, listing_item li, system_object so
                    WHERE t.member_id = @member_id
					AND so.object_id = m.member_id
					AND t.member_id = m.member_id
					AND t.source_id = p.passcode_id
					AND te.transaction_id = t.transaction_id
                    AND te.[record_status] <> @recDeleted
					AND t.type = 1
					AND	 te.status = li.[value]
					AND l.[code] = 'Status'
					AND l.[list_id] = li.[list_id]
                    ORDER BY crt_date desc

                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                     SELECT 
                        te.transaction_id, 
                        t.type, 
                        t.source_id, 

                        t.member_id, 
                        te.point_earn, 
                        te.point_used, 
                        te.status, 
                        t.void_date, 
                        t.remark, 
                        t.display, 
                        te.crt_date, 
                        te.crt_by_type, 
                        te.crt_by, 
                        te.upd_date, 
                        te.upd_by_type, 
                        te.upd_by, 
                        te.record_status,
						p.pin_value,
						so.name 'member_name',
						li.name 'status_name'

                    FROM  transaction_earn te, [transaction] t, member_profile m, passcode p, listing l, listing_item li, system_object so
                    WHERE 1=2
        			SET @sql_remark = 'RecordInvalid'
    		    END
    	END
	ELSE
		BEGIN
		    SET @sql_remark = 'UserInvalid'
		END
        
END
GO