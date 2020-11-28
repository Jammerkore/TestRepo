--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads a row from the rollup info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM ROLLUP_INFO
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
