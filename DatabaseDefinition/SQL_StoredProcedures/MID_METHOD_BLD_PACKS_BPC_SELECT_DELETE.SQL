--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Deletes select criteria for build packs for the given method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_BLD_PACKS_BPC_SELECT_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_BLD_PACKS_BPC_SELECT 
	WHERE METHOD_RID=@METHOD_RID

	SELECT @@ROWCOUNT;

END
GO