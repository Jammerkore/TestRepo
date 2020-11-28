--dv =============================================
--dv Create date:	1/17/2014
--dv Description:	Reads size alternate models from the size alternate name
--dv				Case insensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_ALTERNATE_MODEL_READ_FROM_NAME_UPPER]
	@SIZE_ALTERNATE_NAME varchar(55)
AS
BEGIN
	SET NOCOUNT ON;

	SET @SIZE_ALTERNATE_NAME = UPPER(@SIZE_ALTERNATE_NAME);

	SELECT 
		SIZE_ALTERNATE_RID, 
		SIZE_ALTERNATE_NAME
	FROM SIZE_ALTERNATE_MODEL WITH (NOLOCK) 
	WHERE UPPER(SIZE_ALTERNATE_NAME) LIKE @SIZE_ALTERNATE_NAME
	ORDER BY SIZE_ALTERNATE_NAME

END
GO