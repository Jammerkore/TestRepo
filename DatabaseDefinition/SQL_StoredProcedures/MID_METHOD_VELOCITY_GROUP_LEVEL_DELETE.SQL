--dv =============================================
--dv Create date:	1/27/2014
--dv Description:	Deletes a Velocity Matrix Group Level
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_GROUP_LEVEL_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_VELOCITY_GROUP_LEVEL  
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
END
GO