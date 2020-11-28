--dv =============================================
--dv Create date:	1/17/2014
--dv Description:	Reads size alternate models from the size alternate name
--dv				Case sensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME]
	@SIZE_ALTERNATE_NAME varchar(55)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SIZE_ALTERNATE_RID, 
		SIZE_ALTERNATE_NAME
	FROM SIZE_ALTERNATE_MODEL WITH (NOLOCK) 
	WHERE SIZE_ALTERNATE_NAME LIKE @SIZE_ALTERNATE_NAME
	ORDER BY SIZE_ALTERNATE_NAME

END
GO