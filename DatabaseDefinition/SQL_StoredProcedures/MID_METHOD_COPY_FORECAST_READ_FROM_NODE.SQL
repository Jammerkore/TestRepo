--dv =============================================
--dv Create date:	12/10/2013
--dv Description:	Reads rows from the method copy forecast based on hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_COPY_FORECAST_READ_FROM_NODE]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mcf.METHOD_RID, 
		mcf.PLAN_TYPE, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID,
		m.USER_RID, 
		au.USER_NAME
	FROM METHOD_COPY_FORECAST mcf, 
		 METHOD m, 
		 APPLICATION_USER au
	WHERE mcf.HN_RID = @HN_RID
		  AND mcf.METHOD_RID = m.METHOD_RID
		  AND m.USER_RID = au.USER_RID

END
GO