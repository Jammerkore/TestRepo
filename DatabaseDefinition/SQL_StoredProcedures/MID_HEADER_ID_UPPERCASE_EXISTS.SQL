--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Returns the row count based on the @HDR_ID passed in, case insensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_ID_UPPERCASE_EXISTS]
	@HDR_ID varchar(100),
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) MyCount 
	FROM HEADER 
	WHERE (UPPER(RTRIM(LTRIM(HDR_ID)))) = @HDR_ID
		AND HDR_RID != @HDR_RID

END
GO
