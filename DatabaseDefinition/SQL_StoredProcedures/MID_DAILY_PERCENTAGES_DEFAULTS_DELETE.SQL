--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Deletes daily percentage defaults for the given hierarchy node rid and store rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DAILY_PERCENTAGES_DEFAULTS_DELETE]
	@HN_RID int,
	@ST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM DAILY_PERCENTAGES_DEFAULTS WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID
		  AND ST_RID = @ST_RID

	SELECT @@ROWCOUNT;

END
GO
