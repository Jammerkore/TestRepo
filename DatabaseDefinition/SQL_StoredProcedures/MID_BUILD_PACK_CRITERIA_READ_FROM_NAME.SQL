--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads build pack criteria from the given name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BUILD_PACK_CRITERIA_READ_FROM_NAME]
	@BPC_NAME varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM VW_BUILD_PACK_CRITERIA
	WHERE BPC_NAME = @BPC_NAME
	ORDER BY COMBO_RID

END
GO