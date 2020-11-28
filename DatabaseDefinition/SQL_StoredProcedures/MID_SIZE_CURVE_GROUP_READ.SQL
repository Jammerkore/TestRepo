--dv =============================================
--dv Create date: 1/31/2014
--dv Description: Reads a size curve group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_GROUP_READ]
	@SIZE_CURVE_GROUP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SCG.SIZE_CURVE_GROUP_RID, 
		SCG.SIZE_CURVE_GROUP_NAME 
	FROM SIZE_CURVE_GROUP SCG
	WHERE SCG.SIZE_CURVE_GROUP_RID = @SIZE_CURVE_GROUP_RID

END
GO
