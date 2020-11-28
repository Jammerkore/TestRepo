--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes all rows on the store eligibility table for the given hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_ELIGIBILITY_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_ELIGIBILITY WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
