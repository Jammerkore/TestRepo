--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads the color code rid from the header bulk color
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_READ_COLOR_CODE] 
	@HDR_BC_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		COLOR_CODE_RID 
	FROM HEADER_BULK_COLOR 
	WHERE HDR_BC_RID = @HDR_BC_RID

END
GO