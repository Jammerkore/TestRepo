--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads sell thru percentage based on the hierarchy node rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SELL_THRU_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HN_RID, 
		SELL_THRU_PCT 
	FROM SELL_THRU 
	WHERE HN_RID=@HN_RID
	ORDER BY SELL_THRU_PCT DESC

END
GO
