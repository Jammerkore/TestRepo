--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Reads store group level rid from the method copy group level forecast table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_COPY_GROUP_LEVEL_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SGL_RID
	FROM METHOD_COPY_GROUP_LEVEL
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY SGL_RID ASC

END
GO