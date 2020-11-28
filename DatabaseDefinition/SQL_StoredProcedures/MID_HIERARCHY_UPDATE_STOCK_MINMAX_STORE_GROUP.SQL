--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Updates the store group rid for a stock min max row based on the hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_UPDATE_STOCK_MINMAX_STORE_GROUP]
	@HN_RID int,
	@SG_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE HIERARCHY_NODE WITH (ROWLOCK) 
	SET 
		STOCK_MIN_MAX_SG_RID = @SG_RID
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
