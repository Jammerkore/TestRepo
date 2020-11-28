--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Returns the HDR_ID based on the HDR_RID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_ID]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_ID 
	FROM HEADER 
	WHERE HDR_RID = @HDR_RID

END
GO
