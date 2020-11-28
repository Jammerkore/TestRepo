--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Reads the description version text for the version forecast
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_VERSION_READ_DESCRIPTION]
	@FV_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		DESCRIPTION 
	FROM FORECAST_VERSION 
	WHERE FV_RID = @FV_RID

END
GO