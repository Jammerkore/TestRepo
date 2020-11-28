--dv =============================================
--dv Create date: 02/04/2014
--dv Description: Sets the one time purge indicator for system options
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_UPDATE_ONE_TIME_PURGE]
	@PERFORM_ONETIME_SPECIAL_PURGE_IND char
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE SYSTEM_OPTIONS 
	SET 
		PERFORM_ONETIME_SPECIAL_PURGE_IND = @PERFORM_ONETIME_SPECIAL_PURGE_IND

	SELECT @@ROWCOUNT;
END
GO






