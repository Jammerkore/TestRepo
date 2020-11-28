--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes all rows for chain plan set percentages based on the hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_SET_PERCENT_SET_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CHAIN_SET_PERCENT_SET 
    WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
