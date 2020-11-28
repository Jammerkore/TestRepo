--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes rows on the store eligibility table for the given hierarchy node and store rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_ELIGIBILITY_DELETE]
	@HN_RID int,
	@ST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_ELIGIBILITY WITH (ROWLOCK) 
	WHERE HN_RID = @HN_RID
		  AND ST_RID = @ST_RID

	SELECT @@ROWCOUNT;

END
GO
