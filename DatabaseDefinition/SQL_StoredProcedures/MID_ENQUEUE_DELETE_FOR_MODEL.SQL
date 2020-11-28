--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Deletes enqueue data for forecast models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_FOR_MODEL]
	@ENQUEUE_TYPE int,
	@RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM MID_ENQUEUE 
	WHERE ENQUEUE_TYPE = @ENQUEUE_TYPE
		  AND RID = @RID

	SELECT @@ROWCOUNT;

END
GO
