--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes a Size Curve Group Join
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_GROUP_JOIN_DELETE]
	@SIZE_CURVE_GROUP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIZE_CURVE_GROUP_JOIN 
	WHERE SIZE_CURVE_GROUP_RID = @SIZE_CURVE_GROUP_RID

	SELECT @@ROWCOUNT;
END
GO