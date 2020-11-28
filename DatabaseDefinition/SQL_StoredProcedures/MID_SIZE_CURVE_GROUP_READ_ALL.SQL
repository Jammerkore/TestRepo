--dv =============================================
--dv Create date: 1/31/2014
--dv Description: Reads all size curve groups
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_GROUP_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SCG.SIZE_CURVE_GROUP_RID, 
		SCG.SIZE_CURVE_GROUP_NAME 
	FROM SIZE_CURVE_GROUP SCG 
	ORDER BY SCG.SIZE_CURVE_GROUP_NAME

END
GO

