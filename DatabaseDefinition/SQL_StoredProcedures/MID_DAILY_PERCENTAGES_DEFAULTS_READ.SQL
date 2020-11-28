--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads daily percentage defaults for the given hierarchy node rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DAILY_PERCENTAGES_DEFAULTS_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM DAILY_PERCENTAGES_DEFAULTS
	WHERE HN_RID = @HN_RID
	ORDER BY ST_RID

END
GO
