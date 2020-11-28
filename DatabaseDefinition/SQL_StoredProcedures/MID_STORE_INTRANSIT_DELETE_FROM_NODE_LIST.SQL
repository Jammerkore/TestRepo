--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Deletes store intransit for the nodes in the list provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_INTRANSIT_DELETE_FROM_NODE_LIST]
	@HN_RID_LIST HN_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_INTRANSIT 
	WHERE HN_RID IN (SELECT HN_RID FROM @HN_RID_LIST)

	SELECT @@ROWCOUNT;

END
GO