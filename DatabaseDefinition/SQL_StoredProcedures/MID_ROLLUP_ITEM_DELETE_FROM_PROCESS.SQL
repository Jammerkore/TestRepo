--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes rollup items for the process
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_DELETE_FROM_PROCESS]
	@PROCESS int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM ROLLUP_ITEM 
	WHERE PROCESS = @PROCESS

	SELECT @@ROWCOUNT;
END
GO