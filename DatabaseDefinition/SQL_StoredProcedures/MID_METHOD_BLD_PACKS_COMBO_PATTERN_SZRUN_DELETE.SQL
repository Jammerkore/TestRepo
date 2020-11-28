--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Deletes rows from the METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN table for the given method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN 
	WHERE METHOD_RID=@METHOD_RID

	SELECT @@ROWCOUNT;

END
GO