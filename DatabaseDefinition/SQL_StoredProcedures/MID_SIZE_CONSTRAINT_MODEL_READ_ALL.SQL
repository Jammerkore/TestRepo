--dv =============================================
--dv Create date: 1/17/2014
--dv Description: Reads all size constraint models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CONSTRAINT_MODEL_READ_ALL] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SIZE_CONSTRAINT_RID, 
		SIZE_CONSTRAINT_NAME, 
		SG_RID,
		SIZE_GROUP_RID, 
		COALESCE(SIZE_CURVE_GROUP_RID,-1) SIZE_CURVE_GROUP_RID
	FROM SIZE_CONSTRAINT_MODEL WITH (NOLOCK) 
	ORDER BY SIZE_CONSTRAINT_NAME

END
GO