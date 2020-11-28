--dv =============================================
--dv Create date:	1/17/2014
--dv Description:	Get size constraint models for a given size constraint - case insensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME_UPPER]
	@SIZE_CONSTRAINT_NAME varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	SET @SIZE_CONSTRAINT_NAME=UPPER(@SIZE_CONSTRAINT_NAME)

    SELECT *
	FROM SIZE_CONSTRAINT_MODEL WITH (NOLOCK) 
	WHERE UPPER(SIZE_CONSTRAINT_NAME) LIKE @SIZE_CONSTRAINT_NAME
	ORDER BY SIZE_CONSTRAINT_NAME

END
GO