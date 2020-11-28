--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads a row from the size curve generate info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_GENERATE_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM SIZE_CURVE_GENERATE_INFO
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
