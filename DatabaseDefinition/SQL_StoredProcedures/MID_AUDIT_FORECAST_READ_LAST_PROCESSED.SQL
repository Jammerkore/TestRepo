--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads the last processed row from the forecast audit table based on the method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_FORECAST_READ_LAST_PROCESSED]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 
		af.*,
        au.USER_NAME,
        au.USER_FULLNAME
    FROM AUDIT_FORECAST af 
		 LEFT OUTER JOIN APPLICATION_USER au ON af.USER_RID = au.USER_RID
    WHERE METHOD_RID = @METHOD_RID
    ORDER BY af.PROCESS_DATE_TIME DESC

END
GO
