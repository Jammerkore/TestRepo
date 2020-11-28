--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads last upgrade row from the application services history
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_SERVICES_HISTORY_READ_LAST_UPGRADE]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 * 
	FROM APPLICATION_UPGRADE_HISTORY 
	ORDER BY UPGRADE_RID DESC

END
GO




