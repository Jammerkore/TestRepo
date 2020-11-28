--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes a row into the build pack criteria load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BUILD_PACK_CRITERIA_LOAD_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM BUILD_PACK_CRITERIA_LOAD_INFO 
	WHERE PROCESS_RID = @PROCESS_RID
		
	SELECT @@ROWCOUNT;

END
GO
