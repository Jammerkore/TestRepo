--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Reads the rule layer info
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PACK_RULE_LAYER_READ_INFO]
	@HDR_PACK_RID INT,
	@LAYER_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM PACK_RULE_LAYER
	WHERE HDR_PACK_RID = @HDR_PACK_RID
	AND LAYER_ID = @LAYER_ID

END
GO