--dv =============================================
--dv Create date:	1/17/2014
--dv Description:	Reads size constraint models by exact constraint name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_EXACT]
	@SIZE_CONSTRAINT_NAME varchar(50)
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
	WHERE SIZE_CONSTRAINT_NAME = @SIZE_CONSTRAINT_NAME

END
GO