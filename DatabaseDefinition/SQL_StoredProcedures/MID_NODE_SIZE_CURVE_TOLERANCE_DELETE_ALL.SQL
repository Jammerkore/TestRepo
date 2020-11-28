--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes all rows for size curve tolerance based on the hierarchy node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_SIZE_CURVE_TOLERANCE_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM NODE_SIZE_CURVE_TOLERANCE WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
