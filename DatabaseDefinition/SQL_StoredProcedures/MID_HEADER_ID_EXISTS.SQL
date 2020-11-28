--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Returns the row count based on the @HDR_ID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_ID_EXISTS]
	@HDR_ID varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) MyCount 
	FROM HEADER 
	WHERE HDR_ID = @HDR_ID

END
GO
