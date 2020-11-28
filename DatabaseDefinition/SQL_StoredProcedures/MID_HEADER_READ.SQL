--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Returns the header based on the RID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM UDF_HEADER_READ (@HDR_RID)

END
GO
