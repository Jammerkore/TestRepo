--dv =============================================
--dv Create date:	9/20/2013
--dv Description:	Get size constraint models for a given size constraint
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CONSTRAINT_MODELS_GET]
	@SIZE_CONSTRAINT_RID INT
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
	WHERE SIZE_CONSTRAINT_RID = @SIZE_CONSTRAINT_RID

END
GO
