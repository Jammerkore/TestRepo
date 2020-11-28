--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Reads a spread forecast method by hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SPREAD_FORECAST_READ_FROM_NODE] 
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		msf.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID,
		m.USER_RID, au.USER_NAME
	FROM METHOD_SPREAD_FORECAST msf, 
		 METHOD m, 
		 APPLICATION_USER au
	WHERE msf.HN_RID = @HN_RID
		  AND msf.METHOD_RID = m.METHOD_RID
		  AND m.USER_RID = au.USER_RID

END
GO





