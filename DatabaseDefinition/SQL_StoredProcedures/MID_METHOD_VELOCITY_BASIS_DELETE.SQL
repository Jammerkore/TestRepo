--dv =============================================
--dv Create date:	1/27/2014
--dv Description:	Deletes a Velocity Basis Method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_BASIS_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_VELOCITY_BASIS  
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
END
GO