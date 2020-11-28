--dv =============================================
--dv Create date:	11/5/2014
--dv Description:	Deletes custom conversion function entries from the queue
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_DB_CONVERSION_QUEUE_DELETE_FROM_FUNCTION]
	@CC_FUNCTION_NAME VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM APPLICATION_DB_CONVERSION_QUEUE
	WHERE CC_FUNCTION_NAME = @CC_FUNCTION_NAME

	SELECT @@ROWCOUNT

END
GO
