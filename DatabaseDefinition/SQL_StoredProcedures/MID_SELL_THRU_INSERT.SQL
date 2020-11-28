--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row into the sell thru percentage table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SELL_THRU_INSERT]
	@HN_RID int,
	@SELL_THRU_PCT int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SELL_THRU(HN_RID, SELL_THRU_PCT)
	VALUES (@HN_RID, @SELL_THRU_PCT)

	SELECT @@ROWCOUNT;

END
GO
