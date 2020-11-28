--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Deletes enqueue data for a product hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_FOR_HIERARCHY]
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HIERARCHY_TYPE int
	SET @HIERARCHY_TYPE = 5

	DELETE 
	FROM MID_ENQUEUE 
	WHERE ENQUEUE_TYPE=@HIERARCHY_TYPE
		  AND PH_RID=@PH_RID

	SELECT @@ROWCOUNT;

END
GO
