--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Deletes a computation group based on the group rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_GROUP_DELETE]
	@CG_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM COMPUTATION_GROUP
	WHERE CG_RID = @CG_RID

	SELECT @@ROWCOUNT;

END
GO
