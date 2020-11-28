--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Deletes daily percentages for the given hierarchy node rid, store rid, and calender date range rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DAILY_PERCENTAGES_DELETE]
	@HN_RID int,
	@ST_RID int,
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM DAILY_PERCENTAGES WITH (ROWLOCK) 
	WHERE HN_RID = @HN_RID 
          AND ST_RID = @ST_RID 
          AND CDR_RID = @CDR_RID

	SELECT @@ROWCOUNT;

END
GO
