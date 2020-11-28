--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes all rows from the sell thru percentage table based on the hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SELL_THRU_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SELL_THRU WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
