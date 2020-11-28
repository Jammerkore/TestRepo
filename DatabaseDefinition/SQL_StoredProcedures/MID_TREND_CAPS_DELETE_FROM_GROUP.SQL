--dv =============================================
--dv Create date: 1/17/2014
--dv Description: Deletes trend caps based on method and store group
--dv History:		5/1/2015 TT#1517-MD -jsobek -Store Service Optimization
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TREND_CAPS_DELETE_FROM_GROUP] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM TREND_CAPS
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO