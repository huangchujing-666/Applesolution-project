IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_GetSystemConfigList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[sp_GetSystemConfigList]
GO

/****** Object:  StoredProcedure [sp_GetSystemConfigList]    Script Date: 2013/10/8 下午 07:12:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Leo
-- Create date: 2013/10/8 下午 07:12:01
-- Description:	Stored Procedure for 
-- Pending Task: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetSystemConfigList]
	-- Access Object
    @access_object_id INT,

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
		    
            SET @recValid = 1
    	    
			IF @recValid = 1
    		    BEGIN
                     
                    SELECT
                        s.config_id, 
                        s.name,
                        s.value,
						s.data_type,
						s.display,
						s.display_order,
						s.edit,
                        s.crt_date, 
                        s.crt_by_type, 
                        s.crt_by, 
                        s.upd_date, 
                        s.upd_by_type, 
                        s.upd_by, 
                        s.record_status
                    FROM  system_config s
					WHERE 1=1
                    AND s.[record_status] <> @recDeleted
					AND s.display = 1
					UNION
					SELECT 
						-1000 as config_id,
						'database_db_name' as name,
						DB_NAME() as value,
						'' as data_type,
						'TRUE' as display,
						-10 as display_order,
						'FALSE' as edit,
						'' as crt_date,
						0 as crt_by_type, 
						0 as crt_by,
						'' as upd_date, 
						0 as upd_by_type, 
						0 as upd_by, 
						0 as record_status
						UNION
					SELECT 
						-1001 as config_id,
						'database_sql_version' as name,
						 convert(varchar(max),SERVERPROPERTY('productversion'))+', '+ convert(varchar(max),SERVERPROPERTY('productlevel'))+', '+ convert(varchar(max),SERVERPROPERTY('edition')) as value,
						'' as data_type,
						'TRUE' as display,
						-9 as display_order,
						'FALSE' as edit,
						'' as crt_date,
						0 as crt_by_type, 
						0 as crt_by,
						'' as upd_date, 
						0 as upd_by_type, 
						0 as upd_by, 
						0 as record_status	
					UNION 
					SELECT -- SQL2005 not support CONNECTIONPROPERTY
						-1002 as config_id,
						'database_ip' as name,
						 convert(varchar(max),CONNECTIONPROPERTY('local_net_address')) as value,
						'' as data_type,
						'TRUE' as display,
						-8 as display_order,
						'FALSE' as edit,
						'' as crt_date,
						0 as crt_by_type, 
						0 as crt_by,
						'' as upd_date, 
						0 as upd_by_type, 
						0 as upd_by, 
						0 as record_status
					UNION
					SELECT 
						-1003 as config_id,
						'database_server_name' as name,
						 @@SERVERNAME as value,
						'' as data_type,
						'TRUE' as display,
						-7 as display_order,
						'FALSE' as edit,
						'' as crt_date,
						0 as crt_by_type, 
						0 as crt_by,
						'' as upd_date, 
						0 as upd_by_type, 
						0 as upd_by, 
						0 as record_status
		            
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