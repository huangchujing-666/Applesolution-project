DELETE FROM system_config
WHERE name = 'passcode_insert_method'
GO

DELETE FROM section
WHERE section_id = 304	-- Passcode Generate
GO

TRUNCATE TABLE passcode
GO

TRUNCATE TABLE passcode_format
GO

TRUNCATE TABLE passcode_generate
GO


TRUNCATE TABLE passcode_prefix
GO

TRUNCATE TABLE passcode_registry
GO

TRUNCATE TABLE passcode_failImport
GO

TRUNCATE TABLE transaction_earn
GO

TRUNCATE TABLE transaction_use
GO
