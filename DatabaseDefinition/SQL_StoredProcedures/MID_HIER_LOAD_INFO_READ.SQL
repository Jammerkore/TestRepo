--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads hierarchy load info based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_LOAD_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HIER_LOAD_INFO
    WHERE PROCESS_RID=@PROCESS_RID

END
GO
