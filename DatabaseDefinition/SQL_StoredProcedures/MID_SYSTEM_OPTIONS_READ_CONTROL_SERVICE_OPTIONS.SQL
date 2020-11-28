--dv =============================================
--dv Create date: 12/20/2013
--dv Description: Reads control service system options
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_READ_CONTROL_SERVICE_OPTIONS]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		USE_BATCH_ONLY_MODE,
		CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON
	FROM SYSTEM_OPTIONS 
	
END
GO