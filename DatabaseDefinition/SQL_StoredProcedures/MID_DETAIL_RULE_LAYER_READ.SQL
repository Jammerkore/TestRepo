--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Reads the detail rule layer for the header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DETAIL_RULE_LAYER_READ]
	@HDR_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM DETAIL_RULE_LAYER
	WHERE HDR_RID = @HDR_RID

END
GO