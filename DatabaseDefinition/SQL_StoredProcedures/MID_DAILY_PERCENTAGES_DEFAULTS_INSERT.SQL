--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Inserts daily percentage defaults for the given hierarchy node rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DAILY_PERCENTAGES_DEFAULTS_INSERT]
	@HN_RID int,
	@ST_RID int,
	@DAY1 float,
	@DAY2 float,
	@DAY3 float,
	@DAY4 float,
	@DAY5 float,
	@DAY6 float,
	@DAY7 float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO DAILY_PERCENTAGES_DEFAULTS(HN_RID, ST_RID, DAY1, DAY2, DAY3, DAY4, DAY5, DAY6, DAY7)
	VALUES (@HN_RID, @ST_RID, @DAY1, @DAY2, @DAY3, @DAY4, @DAY5, @DAY6, @DAY7)

	SELECT @@ROWCOUNT;

END
GO
