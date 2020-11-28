--dv =============================================
--dv Create date:	1/17/2014
--dv Description:	Reads size curve groups from size curve group name
--dv				Case sensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_GROUP_READ_FROM_NAME]
	@SIZE_CURVE_GROUP_NAME varchar(255)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SCG.SIZE_CURVE_GROUP_RID, 
		SCG.SIZE_CURVE_GROUP_NAME 
	FROM SIZE_CURVE_GROUP SCG
	WHERE SCG.SIZE_CURVE_GROUP_NAME LIKE @SIZE_CURVE_GROUP_NAME				
	ORDER BY SIZE_CURVE_GROUP_NAME

END
GO