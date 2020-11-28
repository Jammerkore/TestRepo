--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Reads all size alternate models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_ALTERNATE_MODEL_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM SIZE_ALTERNATE_MODEL 
	ORDER BY SIZE_ALTERNATE_NAME

END
GO
