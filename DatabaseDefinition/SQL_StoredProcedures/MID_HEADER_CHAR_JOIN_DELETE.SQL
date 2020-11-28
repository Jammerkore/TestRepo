---dv =============================================
--dv Create date: 2/5/2014
--dv Description: Deletes header characteristics
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_JOIN_DELETE] 
	@HDR_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM HEADER_CHAR_JOIN 
	WHERE HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;
END
GO