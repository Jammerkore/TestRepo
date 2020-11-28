--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads the header rids for the placeholder
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_RID_FROM_PLACEHOLDER]
	@PLACEHOLDER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_RID 
	FROM HEADER 
	WHERE PLACEHOLDER_RID = @PLACEHOLDER_RID

END
GO