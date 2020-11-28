--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads build pack criteria
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BUILD_PACK_CRITERIA_READ]
	@BPC_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM VW_BUILD_PACK_CRITERIA
	WHERE BPC_RID = @BPC_RID
	ORDER BY COMBO_RID

END
GO