--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Reads the modify sales method grades
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_GRADE_READ] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM METHOD_MOD_SALES_GRADE
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY BOUNDARY DESC

END
GO

