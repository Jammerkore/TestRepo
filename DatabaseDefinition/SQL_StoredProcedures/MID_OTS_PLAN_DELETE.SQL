--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Deletes an OTS plan
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OTS_PLAN_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM OTS_PLAN 
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO