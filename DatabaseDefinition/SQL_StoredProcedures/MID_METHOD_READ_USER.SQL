--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads the user for the given method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_USER]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT USER_RID 
	FROM METHOD 
	WHERE METHOD_RID = @METHOD_RID

END
GO


