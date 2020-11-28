--dv =============================================
--dv Create date:	11/5/2014
--dv Description:	Reads functions for deferred execution
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_DB_CONVERSION_QUEUE_READ_DEFERRED]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT CC_FUNCTION_NAME
	FROM APPLICATION_DB_CONVERSION_QUEUE
	WHERE IS_DEFERRED = 1

END
GO
