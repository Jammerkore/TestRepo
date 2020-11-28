--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes all store capacity rows based on the hierarchy node rid 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CAPACITY_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_CAPACITY WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
