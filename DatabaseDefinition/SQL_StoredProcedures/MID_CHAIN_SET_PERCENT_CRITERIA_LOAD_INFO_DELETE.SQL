--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes a row from the chain set percent criteria load info
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
