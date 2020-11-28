--dv =============================================
--dv Create date:	1/17/2014
--dv Description:	Get size constraint models for a given size constraint, case sensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CONSTRAINT_MODELS_READ_FROM_NAME]
	@SIZE_CONSTRAINT_NAME varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
	FROM SIZE_CONSTRAINT_MODEL WITH (NOLOCK) 
	WHERE SIZE_CONSTRAINT_NAME LIKE @SIZE_CONSTRAINT_NAME
	ORDER BY SIZE_CONSTRAINT_NAME

END
GO