--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Reads the pack rule layer for the header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PACK_RULE_LAYER_READ]
	@HDR_PACK_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM PACK_RULE_LAYER
	WHERE HDR_PACK_RID = @HDR_PACK_RID

END
GO