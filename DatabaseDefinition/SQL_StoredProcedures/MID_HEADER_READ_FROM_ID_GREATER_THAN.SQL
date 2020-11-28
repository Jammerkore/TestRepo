--dv =============================================
--dv Create date:	11/11/2013
--dv Description:	Returns the HDR_ID, HDR_RID where HDR_ID is greater than @HDR_ID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_FROM_ID_GREATER_THAN]
	@HDR_ID varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_ID, HDR_RID 
	FROM HEADER
    WHERE HDR_ID > @HDR_ID
	ORDER BY HDR_ID

END
GO
