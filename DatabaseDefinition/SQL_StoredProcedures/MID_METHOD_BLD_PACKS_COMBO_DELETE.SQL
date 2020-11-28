--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Deletes combo and combo pattern rows for build packs based on the method and combo rid list
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_BLD_PACKS_COMBO_DELETE]
	@METHOD_RID int,
	@COMBO_RID_LIST COMBO_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_BLD_PACKS_COMBO_PATTERN 
	WHERE METHOD_RID=@METHOD_RID 
		  AND COMBO_RID IN (SELECT COMBO_RID FROM @COMBO_RID_LIST)

	DELETE 
	FROM METHOD_BLD_PACKS_COMBO 
	WHERE METHOD_RID=@METHOD_RID 
		  AND COMBO_RID IN (SELECT COMBO_RID FROM @COMBO_RID_LIST)

	SELECT @@ROWCOUNT;

END
GO