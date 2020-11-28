--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes all rows on the similar stores table based on the hierarchy node rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIMILAR_STORES_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIMILAR_STORES WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
