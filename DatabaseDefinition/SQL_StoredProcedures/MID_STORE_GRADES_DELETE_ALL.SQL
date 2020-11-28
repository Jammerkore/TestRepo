--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes all row on the store grades table based on the hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GRADES_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_GRADES WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID 

	SELECT @@ROWCOUNT;

END
GO
