--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes all size curves for similar stores based on the hierarchy node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_SIZE_CURVE_SIMILAR_STORE_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM NODE_SIZE_CURVE_SIMILAR_STORE WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
