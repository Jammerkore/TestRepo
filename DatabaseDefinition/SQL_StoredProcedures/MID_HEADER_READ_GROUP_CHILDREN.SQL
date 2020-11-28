--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Returns headers in the header group RID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_GROUP_CHILDREN]
	@HDR_GROUP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HEADER 
	WHERE HDR_GROUP_RID = @HDR_GROUP_RID

END
GO
