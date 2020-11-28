--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Returns Method Size Curve Curve Basis Detail
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_READ]
	@METHOD_RID int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		BASIS_SEQ,
		COALESCE(SIZE_CURVE_GROUP_RID,'0') SIZE_CURVE_GROUP_RID,
		COALESCE(WEIGHT,1) WEIGHT
    FROM METHOD_SIZE_CURVE_CRVE_BAS_DET 
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY BASIS_SEQ

END
GO