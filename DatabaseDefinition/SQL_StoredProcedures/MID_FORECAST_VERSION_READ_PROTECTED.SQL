--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Reads the protection indicator for the forecast version
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_VERSION_READ_PROTECTED]
	@FV_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		PROTECT_HISTORY_IND 
	FROM FORECAST_VERSION 
	WHERE FV_RID = @FV_RID

END
GO