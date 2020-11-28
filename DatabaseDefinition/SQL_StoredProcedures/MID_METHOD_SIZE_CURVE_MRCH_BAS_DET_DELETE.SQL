--dv =============================================
--dv Create date:	6/24/2014
--dv Description:	Deletes merchandise basis details for a size curve method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_SIZE_CURVE_MRCH_BAS_DET
	WHERE METHOD_RID=@METHOD_RID

	SELECT @@ROWCOUNT;
END
GO