--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes size curves for similar stores based on the hierarchy node and store rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE]
	@HN_RID int,
	@ST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM NODE_SIZE_CURVE_SIMILAR_STORE WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID
		  AND ST_RID = @ST_RID

	SELECT @@ROWCOUNT;

END
GO
