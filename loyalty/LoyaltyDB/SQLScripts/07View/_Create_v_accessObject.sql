IF OBJECT_ID ('dbo.v_accessObject', 'V') IS NOT NULL
DROP VIEW v_accessObject
GO

-- [type] should be same as table listing_item where code = ObjectType

CREATE VIEW v_accessObject AS
SELECT 
	[type],
	[target_id],
	[name],
	[status],
	[record_status]
FROM system_object
UNION
SELECT 
	1 [type],
	user_id [target_id],
	[name],
	status [status],
	[record_status]
FROM user_profile
UNION
SELECT 
	10 [type],
	member_id [target_id],
	fullname  [name],
	status [status],
	[record_status]
FROM member_profile

	 