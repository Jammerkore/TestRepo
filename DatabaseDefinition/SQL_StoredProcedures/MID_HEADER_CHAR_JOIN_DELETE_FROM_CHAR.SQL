--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Deletes header characteristics
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_JOIN_DELETE_FROM_CHAR] 
	@HC_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM HEADER_CHAR_JOIN 
	WHERE HC_RID = @HC_RID

	SELECT @@ROWCOUNT;
END
GO