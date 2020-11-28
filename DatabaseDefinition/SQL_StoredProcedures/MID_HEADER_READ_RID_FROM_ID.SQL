--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Returns the HDR_RID based on the HDR_ID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_RID_FROM_ID]
	@HDR_ID varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_RID 
	FROM HEADER 
	WHERE HDR_ID = @HDR_ID

END
GO
