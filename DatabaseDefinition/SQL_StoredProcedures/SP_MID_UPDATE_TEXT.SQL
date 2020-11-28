--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description: Updates application text
--dv History: TT#2226 - JSmith - Modify upgrade process to not require text in upgrade.sql unless text has been changed
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_UPDATE_TEXT] 
	@TEXT_CODE INT,
	@TEXT_VALUE VARCHAR(250),
	@TEXT_MAX_LENGTH INT,
	@TEXT_TYPE INT,
	@TEXT_LEVEL INT,
	@TEXT_VALUE_TYPE INT,
	@TEXT_ORDER INT
AS
BEGIN
	EXEC [dbo].[SP_MID_SET_TEXT] @TEXT_CODE,
		@TEXT_VALUE,
		@TEXT_MAX_LENGTH,
		@TEXT_TYPE,
		@TEXT_LEVEL,
		@TEXT_VALUE_TYPE,
		@TEXT_ORDER,
		1
END
GO


