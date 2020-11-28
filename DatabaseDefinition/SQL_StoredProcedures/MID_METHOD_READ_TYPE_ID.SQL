--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads the type id for the method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_TYPE_ID] 
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