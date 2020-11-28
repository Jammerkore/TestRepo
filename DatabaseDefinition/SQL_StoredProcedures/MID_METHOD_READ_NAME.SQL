--dv =============================================
--dv Create date:	12/03/2013
--dv Description:	Get the method name if it exists.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_NAME]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		METHOD_NAME
	FROM METHOD
	WHERE METHOD_RID = @METHOD_RID

END
GO