--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a row on the hierarchy characteristic and join tables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_DELETE]
	@HC_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM HIER_CHAR_JOIN WITH (ROWLOCK) 
	WHERE HC_RID = @HC_RID
	
    DELETE 
	FROM HIER_CHAR WITH (ROWLOCK) 
	WHERE HC_RID = @HC_RID

	SELECT @@ROWCOUNT;

END
GO
