--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads a count of headers for the characteristic
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_JOIN_READ_COUNT]
	@HC_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*)AS MyCount 
	FROM HEADER_CHAR_JOIN
	WHERE HC_RID = @HC_RID

END
GO