--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes rollup items for the process and product hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_DELETE]
	@PROCESS int,
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM ROLLUP_ITEM 
	WHERE PROCESS = @PROCESS 
		  AND PH_RID = @PH_RID

	SELECT @@ROWCOUNT;
END
GO