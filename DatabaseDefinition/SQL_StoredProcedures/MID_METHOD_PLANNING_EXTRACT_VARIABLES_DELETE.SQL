--dv =============================================
--dv Create date: 2/23/2018
--dv Description: Deletes planning extract method child variables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_PLANNING_EXTRACT_VARIABLES_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_PLANNING_EXTRACT_VARIABLES 
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO