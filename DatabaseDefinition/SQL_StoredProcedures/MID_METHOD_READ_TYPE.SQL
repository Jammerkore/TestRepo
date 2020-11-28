--dv =============================================
--dv Create date:	12/03/2013
--dv Description:	Get the method type if it exists.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_TYPE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		METHOD_TYPE_ID
	FROM METHOD
	WHERE METHOD_RID = @METHOD_RID

END
GO