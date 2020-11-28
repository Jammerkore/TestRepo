--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads a row from the build pack criteria load info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BUILD_PACK_CRITERIA_LOAD_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM BUILD_PACK_CRITERIA_LOAD_INFO
	WHERE PROCESS_RID = @PROCESS_RID
		 	
END
GO
