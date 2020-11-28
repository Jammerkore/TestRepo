--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes rows on the similar stores table based on the hierarchy node rid and store rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIMILAR_STORES_DELETE]
	@HN_RID int,
	@ST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIMILAR_STORES WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID
		  AND ST_RID = @ST_RID

	SELECT @@ROWCOUNT;

END
GO
