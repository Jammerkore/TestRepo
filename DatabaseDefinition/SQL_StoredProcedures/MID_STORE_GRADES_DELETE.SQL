--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a row on the store grades table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GRADES_DELETE]
	@HN_RID int,
	@BOUNDARY int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_GRADES WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID 
	AND BOUNDARY = @BOUNDARY

	SELECT @@ROWCOUNT;

END
GO
