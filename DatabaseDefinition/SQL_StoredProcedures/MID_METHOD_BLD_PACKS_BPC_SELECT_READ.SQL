--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads build pack criteria for the given method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_BLD_PACKS_BPC_SELECT_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		METHOD_RID, 
		BPC_COMBO_RID
    FROM METHOD_BLD_PACKS_BPC_SELECT 
	WHERE METHOD_RID=@METHOD_RID

END
GO