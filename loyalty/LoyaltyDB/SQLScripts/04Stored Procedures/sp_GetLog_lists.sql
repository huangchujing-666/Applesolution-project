IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetLog_lists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetLog_lists]
GO

/****** Object:  StoredProcedure [sp_GetLog_lists]    Script Date: 2013/11/19 下午 09:35:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/11/19 下午 09:35:39
-- Description:	Stored Procedure for Get Log
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetLog_lists]
	-- Access Object
    @access_object_id INT,
    
	-- Paging
	@rowIndexStart bigint,
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
		    
			DECLARE @selectSQL NVARCHAR(MAX)

            SET @recValid = 1
    		
            IF @recValid = 1
    		    BEGIN
                      
					SET @selectSQL = 
						'SELECT * FROM ( 
								SELECT CAST(COUNT(*) OVER() as BIGINT) as rowTotal, ROW_NUMBER() OVER (ORDER BY l.crt_date desc) as row,

									l.log_id, 
									l.action_ip, 
									l.action_channel, 
									l.action_type, 
									l.target_obj_id, 
									l.action_detail, 
									l.crt_date, 
									l.crt_by_type, 
									l.crt_by,
									so_a.name ''access_object_name'',
									so_a.type ''access_object_type'',
									so.name ''target_object_name'',
									li_ac.name ''action_channel_name'',
									li_at.name ''action_type_name'',
									li_ot.name ''target_object_type_name''

								FROM  log l , 
									 system_object so, -- for target_object_type
									 system_object so_a, -- for access_object
									 [listing] l_ac, [listing_item] li_ac,   -- for ActionChannel
									 [listing] l_at, [listing_item] li_at,   -- for ActionType
									 [listing] l_ot, [listing_item] li_ot   -- for ObjectType
								WHERE
					
								 l.target_obj_id = so.object_id
								AND l.crt_by = so_a.object_id
								AND so.[type] = li_ot.[value]
								AND l_ot.[code] = ''ObjectType''
								AND l_ot.[list_id] = li_ot.[list_id]

								AND l.[action_channel] = li_ac.[value]
								AND l_ac.[code] = ''ActionChannel''
								AND l_ac.[list_id] = li_ac.[list_id]

								AND l.[action_type] = li_at.[value]
								AND l_at.[code] = ''ActionType''
								AND l_at.[list_id] = li_at.[list_id]

								AND l.[record_status] <> '+CAST(@recDeleted AS varchar(MAX))
								+ ') t WHERE row > ' + CAST(@rowIndexStart AS varchar(MAX)) +' and row <= ' + CAST((@rowIndexStart+ @rowLimit) AS varchar(MAX))
                    
					EXEC sp_executesql @selectSQL

                    SET @sql_result = 1
                END
	    	ELSE
    		    BEGIN
                      SELECT 
                        l.log_id, 
                        l.action_ip, 
                        l.action_channel, 
                        l.action_type, 
                        l.target_obj_id, 
                        l.action_detail, 
                        l.crt_date, 
                        l.crt_by_type, 
                        l.crt_by,
						so_a.name 'access_object_name',
						so.name 'target_object_name',
						li_ac.name 'action_channel_name',
						li_at.name 'action_type_name',
                        li_ot.name 'target_object_type_name'

                    FROM  log l , 
						 system_object so, -- for target_object_type
						 system_object so_a, -- for access_object
						 [listing] l_ac, [listing_item] li_ac,   -- for ActionChannel
						 [listing] l_at, [listing_item] li_at,   -- for ActionType
						 [listing] l_ot, [listing_item] li_ot   -- for ObjectType
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