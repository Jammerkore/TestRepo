--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a size node based on the hn_rid and size code rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_NODE_DELETE]
	@HN_RID int,
	@SIZE_CODE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIZE_NODE WITH (ROWLOCK) 
	WHERE HN_RID = @HN_RID
		  AND SIZE_CODE_RID = @SIZE_CODE_RID

	SELECT @@ROWCOUNT;

END
GO
