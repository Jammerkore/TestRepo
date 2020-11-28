--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Deletes size curve method basis details
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_DELETE]
    @METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_SIZE_CURVE_CRVE_BAS_DET
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
END
GO